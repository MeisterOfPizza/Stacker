using Stacker.Controllers;
using Stacker.Extensions.Utils;
using System;
using System.Collections;
using UnityEngine;

#pragma warning disable 0108
#pragma warning disable 0649

namespace Stacker.Components
{

    [RequireComponent(typeof(MeshRenderer), typeof(Rigidbody), typeof(Collider))]
    class Vehicle : MonoBehaviour, IChainEventable
    {

        #region Private constants
        
        private string VEHICLE_ANIMATION_RUN = "Run vehicle";

        #endregion

        #region Editor

        [Header("References")]
        [SerializeField] private Rigidbody      rigidbody;
        [SerializeField] private MeshRenderer[] meshRenderers;
        [SerializeField] private Collider[]     colliders;
        [SerializeField] private ParticleSystem collisionParticleSystem;

        [Header("FX")]
        [SerializeField] private ParticleSystem[] wheelDustParticleSystems;

        [Header("Animations")]
        [SerializeField] private Animator animator;

        [Header("Values")]
        [SerializeField] private float moveSpeed = 5;

        [Header("Warning material")]
        [SerializeField] private Material _warningMaterial;

        #endregion

        #region Private variables

        private Material[][] defaultMaterials;

        private bool hitStructure;

        #endregion

        #region MonoBehaviour methods

        private void Awake()
        {
            defaultMaterials = new Material[meshRenderers.Length][];

            for (int i = 0; i < meshRenderers.Length; i++)
            {
                defaultMaterials[i] = meshRenderers[i].materials;
            }
        }

        private void OnDisable()
        {
            rigidbody.velocity        = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            StopWheelDustEffect();
        }

        #endregion

        #region Vehicle events

        /// <summary>
        /// Sets this vehicle as a warning.
        /// </summary>
        public void SetAsWarning()
        {
            rigidbody.useGravity = false;
            SetCollidersActive(false);
            ChangeToWarningMaterials();
        }

        private IEnumerator StartVehicle(Action doneCallback)
        {
            rigidbody.useGravity = true;
            SetCollidersActive(true);
            ChangeToDefaultMaterials();

            // Play effects and animations:
            PlayWheelDustEffect();
            animator.Play(VEHICLE_ANIMATION_RUN);

            Vector3 target = -transform.position; // Invert the position to find the target to drive to.
            float distanceToTravel = transform.position.magnitude + 0.01f; // Distance to middle with small extra distance added to remove any risk at floating-point value issues.
            float currentDistance = 0;
            
            hitStructure = false;
            
            while (currentDistance < distanceToTravel && !hitStructure)
            {
                rigidbody.velocity = transform.forward * moveSpeed;
                currentDistance = transform.position.magnitude;

                yield return new WaitForFixedUpdate();
            }

            // If we did not hit a structure, then we want to continue the chain event.
            if (!hitStructure)
            {
                doneCallback();
            }
        }

        #endregion

        #region Physics

        private void OnCollisionEnter(Collision collision)
        {
            if (UtilExtensions.IsLayerInLayerMask(VehicleController.Singleton.StructureLayerMask, collision.gameObject.layer))
            {
                StopWheelDustEffect();

                if (!hitStructure)
                {
                    PlayCollisionEffect(collision);
                }

                hitStructure = true;

                ChallengesController.VehicleHitStructure = true;

                RoundController.Singleton.EndRound();
            }
        }

        private void SetCollidersActive(bool active)
        {
            foreach (var collider in colliders)
            {
                collider.enabled = active;
            }
        }

        #endregion

        #region FX

        private void PlayCollisionEffect(Collision collision)
        {
            ContactPoint contactPoint = collision.contacts[0];

            collisionParticleSystem.transform.SetPositionAndRotation(contactPoint.point, Quaternion.LookRotation(contactPoint.normal, Vector3.up));

            collisionParticleSystem.Play(true);
        }

        private void PlayWheelDustEffect()
        {
            foreach (var ps in wheelDustParticleSystems)
            {
                ps.Play();
            }
        }

        private void StopWheelDustEffect()
        {
            foreach (var ps in wheelDustParticleSystems)
            {
                ps.Stop();
            }
        }

        private void ChangeToWarningMaterials()
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                Material[] materials = meshRenderers[i].materials;

                for (int j = 0; j < materials.Length; j++)
                {
                    materials[j] = _warningMaterial;
                }

                meshRenderers[i].materials = materials;
            }
        }

        private void ChangeToDefaultMaterials()
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].materials = defaultMaterials[i];
            }
        }

        #endregion

        #region IChainEventable

        public void TriggerChainEvent(Action nextCallback)
        {
            StartCoroutine("StartVehicle", nextCallback);
        }

        #endregion

    }

}
