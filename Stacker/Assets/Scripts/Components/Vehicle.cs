using System.Collections;
using UnityEngine;

namespace Stacker.Components
{

    [RequireComponent(typeof(MeshRenderer))]
    class Vehicle : MonoBehaviour
    {

        #region Editor

        [SerializeField] private Rigidbody rigidbody;

        [Space]
        [SerializeField] private float moveSpeed = 5;

        #endregion

        #region Private variables

        private bool hitStructure;

        #endregion

        /// <summary>
        /// Sets this vehicle as a warning.
        /// This will make the vehicle red.
        /// </summary>
        public void SetAsWarning()
        {

        }

        public IEnumerator StartVehicle()
        {
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

    }

}
