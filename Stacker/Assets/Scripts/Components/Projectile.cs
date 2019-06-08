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
        [SerializeField] private Rigidbody    rigidbody;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Collider     collider;

        [Header("Values")]
        [SerializeField] private float fireSpeed = 5f;

        [Header("Shaders")]
        [SerializeField] private Shader _defaultProjectileShader;
        [SerializeField] private Shader _projectileWarningShader;

        #endregion

        #region Private variables

        private bool collisionDetected;
        private bool hitStructure;

        #endregion

        #region Projectile events

        /// <summary>
        /// Sets this vehicle as a warning.
        /// </summary>
        public void SetAsWarning()
        {
            rigidbody.useGravity = false;
            collider.enabled     = false;
            ChangeToWarningMaterials();
        }

        private IEnumerator FireProjectile(Action doneCallback)
        {
            rigidbody.useGravity = true;
            collider.enabled     = true;
            ChangeToDefaultMaterials();

            Vector3 target = new Vector3(-transform.position.x, transform.position.y, -transform.position.z);
            float distanceToTarget = float.MaxValue;

            collisionDetected = false;
            hitStructure      = false;

            // Fire the projectile towards the target with the speed of moveSpeed.
            rigidbody.AddForce((target - transform.position) * fireSpeed);

            while (distanceToTarget > 0.1f && !collisionDetected)
            {
                distanceToTarget = Vector3.Distance(transform.position, target);

                yield return new WaitForEndOfFrame();
            }

            // Deactive vehicle:
            gameObject.SetActive(false);

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
            if (Extensions.Utils.Extensions.IsLayerInLayerMask(ProjectileController.Singleton.StructureLayerMask, collision.gameObject.layer))
            {
                hitStructure = true;

                // TODO: Send msg to player that the projectile hit a structure (building block).
            }

            collisionDetected = true;
        }

        #endregion

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

        #region IChainEventable

        public void TriggerChainEvent(Action nextCallback)
        {
            StartCoroutine("FireProjectile", nextCallback);
        }

        #endregion

    }

}
