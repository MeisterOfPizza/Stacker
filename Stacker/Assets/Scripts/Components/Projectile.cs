using Stacker.Building;
using Stacker.Controllers;
using Stacker.Extensions.Utils;
using System;
using System.Collections;
using UnityEngine;

#pragma warning disable 0108
#pragma warning disable 0649

namespace Stacker.Components
{

    class Projectile : MonoBehaviour, IChainEventable
    {

        #region Editor

        [Header("References")]
        [SerializeField] private Rigidbody      rigidbody;
        [SerializeField] private MeshRenderer   meshRenderer;
        [SerializeField] private Collider       collider;
        [SerializeField] private ParticleSystem collisionParticleSystem;
        [SerializeField] private ParticleSystem trailParticleSystem;
        [SerializeField] private Spin           spin;

        [Header("Values")]
        [SerializeField] private float fireSpeed = 100f;

        [Header("Audio")]
        [SerializeField] private AudioSource soundEffectsSource;
        [SerializeField] private AudioSource projectileLaunchSource;

        [Space]
        [SerializeField] private AudioClip[] projectileCollisionSoundEffects;
        [SerializeField] private AudioClip[] projectileLaunchSoundEffects;

        [Header("Warning material")]
        [SerializeField] private Material _warningMaterial;

        #endregion

        #region Private variables

        private Material[] defaultMaterials;

        private bool collisionDetected;
        private bool hitStructure;

        #endregion

        #region MonoBehaviour methods

        private void Awake()
        {
            defaultMaterials = meshRenderer.materials;
        }

        private void OnDisable()
        {
            rigidbody.velocity        = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        #endregion

        #region Projectile events

        /// <summary>
        /// Sets this vehicle as a warning.
        /// </summary>
        public void SetAsWarning()
        {
            spin.Stop();
            trailParticleSystem.Stop();

            rigidbody.useGravity = false;
            collider.enabled     = false;
            ChangeToWarningMaterials();

            // Set up the rotation to make the effects look better.
            Vector3 target = new Vector3(-transform.position.x, transform.position.y, -transform.position.z);
            transform.rotation = Quaternion.LookRotation(target - transform.position);
        }

        private IEnumerator FireProjectile(Action doneCallback)
        {
            spin.Play();
            trailParticleSystem.Play();

            rigidbody.useGravity = true;
            collider.enabled     = true;
            ChangeToDefaultMaterials();

            PlayLaunchSoundEffect();

            Vector3 target = new Vector3(-transform.position.x, transform.position.y, -transform.position.z);
            float distanceToTarget = float.MaxValue;

            collisionDetected = false;
            hitStructure      = false;

            // Fire the projectile towards the target with the speed of moveSpeed.
            rigidbody.AddForce((target - transform.position) * fireSpeed);

            // Notify the challenge controller that a projectile has been fired:
            ChallengesController.ProjectilesFired++;

            while (distanceToTarget > 0.1f && !collisionDetected)
            {
                distanceToTarget = Vector3.Distance(transform.position, target);

                // Make the projectile look in the direction of the target. This to make effects look better.
                transform.rotation = Quaternion.LookRotation(rigidbody.velocity.normalized);

                yield return new WaitForEndOfFrame();
            }

            doneCallback();
        }

        #endregion

        #region Physics

        private void OnCollisionEnter(Collision collision)
        {
            if (!collisionDetected)
            {
                StopLaunchSoundEffect();
                PlayCollisionSoundEffect();
                PlayCollisionEffect(collision);
                spin.Stop();
                trailParticleSystem.Stop();
            }

            if (UtilExtensions.IsLayerInLayerMask(ProjectileController.Singleton.StructureLayerMask, collision.gameObject.layer) && !hitStructure)
            {
                hitStructure = true;

                if (collision.gameObject.GetComponent<BuildingBlockCopy>() is BuildingBlockCopy)
                {
                    ChallengesController.BlocksHitByProjectiles++;
                }
            }

            collisionDetected = true;
        }

        #endregion

        #region FX

        private void PlayCollisionEffect(Collision collision)
        {
            ContactPoint contactPoint = collision.contacts[0];

            collisionParticleSystem.transform.SetPositionAndRotation(contactPoint.point, Quaternion.LookRotation(contactPoint.normal, Vector3.up));

            collisionParticleSystem.Play(true);
        }

        private void ChangeToWarningMaterials()
        {
            Material[] materials = meshRenderer.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = _warningMaterial;
            }

            meshRenderer.materials = materials;
        }

        private void ChangeToDefaultMaterials()
        {
            meshRenderer.materials = defaultMaterials;
        }

        #endregion

        #region Audio

        private void PlayLaunchSoundEffect()
        {
            projectileLaunchSource.PlayOneShot(projectileLaunchSoundEffects[UnityEngine.Random.Range(0, projectileLaunchSoundEffects.Length)], AudioController.EffectsVolume);
        }

        private void StopLaunchSoundEffect()
        {
            projectileLaunchSource.Stop();
        }

        private void PlayCollisionSoundEffect()
        {
            soundEffectsSource.PlayOneShot(projectileCollisionSoundEffects[UnityEngine.Random.Range(0, projectileCollisionSoundEffects.Length)], AudioController.EffectsVolume);
        }

        #endregion

        #region IChainEventable

        public void TriggerChainEvent(Action nextCallback)
        {
            StartCoroutine("FireProjectile", nextCallback);
        }

        #endregion

    }

}
