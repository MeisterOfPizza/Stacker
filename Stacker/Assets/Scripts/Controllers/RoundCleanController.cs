using System.Collections;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Controllers
{

    /// <summary>
    /// Controls the round cleaning where all the building block copies, projectiles vehicles etc. at the end of each round.
    /// </summary>
    class RoundCleanController : Controller<RoundCleanController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private Transform roundContainer;

        [Header("Cleaning settings")]
        [SerializeField] private float cleanSpeed               = 5f;
        [SerializeField] private float roundCleanTravelDistance = 100f;

        #endregion

        #region Private variables

        private Rigidbody[] rigidbodies;

        /// <summary>
        /// Stores the values of the <see cref="Rigidbody.isKinematic"/> boolean from
        /// each <see cref="Rigidbody"/> found on <see cref="roundContainer"/>.
        /// </summary>
        private bool[] rigidbodyIsKinematicValues;

        /// <summary>
        /// Stores the values of the <see cref="Rigidbody.collisionDetectionMode"/> enum from
        /// each <see cref="Rigidbody"/> found on <see cref="roundContainer"/>.
        /// </summary>
        private CollisionDetectionMode[] collisionDetectionModes;

        #endregion

        /// <summary>
        /// Cleans the round area by moving all the objects out of the way.
        /// </summary>
        public void CleanRound()
        {
            rigidbodies = roundContainer.GetComponentsInChildren<Rigidbody>(true);
            rigidbodyIsKinematicValues = new bool[rigidbodies.Length];
            collisionDetectionModes    = new CollisionDetectionMode[rigidbodies.Length];

            for (int i = 0; i < rigidbodies.Length; i++)
            {
                collisionDetectionModes[i] = rigidbodies[i].collisionDetectionMode;
                rigidbodies[i].collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

                rigidbodyIsKinematicValues[i] = rigidbodies[i].isKinematic;
                rigidbodies[i].isKinematic = true;
            }

            StartCoroutine("CleanRoundGround");
        }

        /// <summary>
        /// Finishes the clean by restoring values changed to previous ones.
        /// </summary>
        public void ResetRound()
        {
            StopCoroutine("CleanRoundGround");

            BuildController.Singleton.ClearCopies();

            roundContainer.position = Vector3.zero;

            if (rigidbodies != null)
            {
                for (int i = 0; i < rigidbodies.Length; i++)
                {
                    rigidbodies[i].isKinematic            = rigidbodyIsKinematicValues[i]; // Set the boolean value to what it was set to BEFORE isKinematic was set to false.
                    rigidbodies[i].collisionDetectionMode = collisionDetectionModes[i];
                }
            }
        }

        /// <summary>
        /// Cleans the round by dragging all the round "items" to the left of the camera's current rotation.
        /// </summary>
        private IEnumerator CleanRoundGround()
        {
            Vector3 target = CameraController.LeftOfCamera * roundCleanTravelDistance;
            float distance = float.MaxValue;

            while (distance > 0.1f)
            {
                distance = Vector3.Distance(roundContainer.position, target);

                roundContainer.position = Vector3.Lerp(roundContainer.position, target, cleanSpeed * Time.deltaTime);

                yield return new WaitForEndOfFrame();
            }
        }

    }

}
