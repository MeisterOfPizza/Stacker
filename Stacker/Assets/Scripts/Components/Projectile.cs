using System.Collections;
using UnityEngine;

namespace Stacker.Components
{

    class Projectile : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private Rigidbody    rigidbody;
        [SerializeField] private MeshRenderer meshRenderer;

        [Header("Values")]
        [SerializeField] private float moveSpeed = 5f;

        [Header("Shaders")]
        [SerializeField] private Shader _defaultProjectileShader;
        [SerializeField] private Shader _projectileWarningShader;

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

        public IEnumerator FireProjectile()
        {
            rigidbody.useGravity = true;
            ChangeToDefaultMaterials();

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

        #region FX

        private void ChangeToWarningMaterials()
        {
            foreach (Material material in meshRenderer.materials)
            {
                material.shader = _projectileWarningShader;
            }
        }

        private void ChangeToDefaultMaterials()
        {
            foreach (Material material in meshRenderer.materials)
            {
                material.shader = _defaultProjectileShader;
            }
        }

        #endregion

    }

}
