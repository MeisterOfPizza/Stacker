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
            float distanceToTarget = float.MaxValue;
            
            hitStructure = false;

            while (distanceToTarget > 0.1f && !hitStructure)
            {
                rigidbody.velocity = transform.forward * moveSpeed;
                distanceToTarget = Vector3.Distance(transform.position, target);

                yield return new WaitForFixedUpdate();
            }

            // If we made it to the target without hitting anything, deactive vehicle:
            if (distanceToTarget <= 0.1f && !hitStructure)
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
            if (MathExtensions.IsLayerInLayerMask(VehicleController.Singleton.StructureLayerMask, collision.gameObject.layer))
            {
                hitStructure = true;

                // TODO: Send msg to player that the vehicle hit a structure (building block).
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
