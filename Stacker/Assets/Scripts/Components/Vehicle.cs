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

    [RequireComponent(typeof(MeshRenderer))]
    class Vehicle : MonoBehaviour, IChainEventable
    {

        #region Editor

        [Header("References")]
        [SerializeField] private Rigidbody    rigidbody;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Collider     collider;

        [Header("Values")]
        [SerializeField] private float moveSpeed = 5;

        [Header("Shaders")]
        [SerializeField] private Shader _defaultVehicleShader;
        [SerializeField] private Shader _vehicleWarningShader;

        #endregion

        #region Private variables
        
        private bool hitStructure;

        #endregion

        #region Vehicle events

        /// <summary>
        /// Sets this vehicle as a warning.
        /// </summary>
        public void SetAsWarning()
        {
            rigidbody.useGravity = false;
            collider.enabled     = false;
            ChangeToWarningMaterials();
        }

        private IEnumerator StartVehicle(Action doneCallback)
        {
            rigidbody.useGravity = true;
            collider.enabled     = true;
            ChangeToDefaultMaterials();

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

            // If we made it to the target without hitting anything, deactive vehicle:
            if (currentDistance > distanceToTravel && !hitStructure)
            {
                gameObject.SetActive(false);
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
                hitStructure = true;

                ChallengesController.VehicleHitStructure = true;

                RoundController.Singleton.LoseRound();
            }
        }

        #endregion

        #region FX

        private void ChangeToWarningMaterials()
        {
            foreach (Material material in meshRenderer.materials)
            {
                material.shader = _vehicleWarningShader;
            }
        }

        private void ChangeToDefaultMaterials()
        {
            foreach (Material material in meshRenderer.materials)
            {
                material.shader = _defaultVehicleShader;
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
