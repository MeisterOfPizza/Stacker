using System.Collections;
using UnityEngine;

namespace Stacker.Components
{

    class Projectile : MonoBehaviour
    {

        #region Editor

        [SerializeField] private Rigidbody rigidbody;

        [Space]
        [SerializeField] private float moveSpeed = 5f;

        #endregion

        #region Private variables

        private bool hitStructure;

        #endregion

        public IEnumerator FireProjectile()
        {
            Vector3 target = -transform.position;
            float distanceToTarget = float.MaxValue;
            hitStructure = false;

            // Fire the projectile towards the target with the speed of moveSpeed.
            rigidbody.AddForce((target - transform.position) * moveSpeed);

            while (distanceToTarget > 0.1f && !hitStructure)
            {
                distanceToTarget = Vector3.Distance(transform.position, target);

                yield return new WaitForEndOfFrame();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            hitStructure = true;

            // TODO: Send msg to player that the projectile hit something.
        }

    }

}
