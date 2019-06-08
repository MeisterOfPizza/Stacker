using System.Collections;
using UnityEngine;

namespace Stacker.Components
{

    [RequireComponent(typeof(MeshRenderer))]
    class Vehicle : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private Rigidbody    rigidbody;
        [SerializeField] private MeshRenderer meshRenderer;

        [Header("Values")]
        [SerializeField] private float moveSpeed = 5;

        [Header("Shaders")]
        [SerializeField] private Shader _defaultVehicleShader;
        [SerializeField] private Shader _vehicleWarningShader;

        #endregion

        #region Private variables

        private bool hitStructure;

        #endregion

        /// <summary>
        /// Sets this vehicle as a warning.
        /// </summary>
        public void SetAsWarning()
        {
            rigidbody.useGravity = false;
            ChangeToWarningMaterials();
        }

        public IEnumerator StartVehicle()
        {
            rigidbody.useGravity = true;
            ChangeToDefaultMaterials();

            Vector3 target = -transform.position; // Invert the position to find the target to drive to.
            float distanceToTarget = float.MaxValue;
            hitStructure = false;

            while (distanceToTarget > 0.1f && !hitStructure)
            {
                rigidbody.AddForce((target - transform.position) * moveSpeed * Time.fixedDeltaTime);
                distanceToTarget = Vector3.Distance(transform.position, target);

                yield return new WaitForFixedUpdate();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            hitStructure = true;

            // TODO: Send msg to player that the vehicle hit something.
        }

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

    }

}
