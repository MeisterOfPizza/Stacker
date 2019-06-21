using Stacker.Building;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Controllers
{

    class StackHeightController : Controller<StackHeightController>
    {

        #region Constants

        private const float START_ALTITUDE = 100f;
        private const float CHECK_HEIGHT   = 0.1f;

        #endregion

        #region Editor

        [SerializeField] private LayerMask buildingBlockLayerMask;

        #endregion

        #region Public properties

        public static float CurrentBuildAltitude { get; private set; }

        #endregion

        /// <summary>
        /// Calculate at which altitude the construction building block should be at.
        /// </summary>
        public static void CalculateCurrentBuildAltitude()
        {
            Vector3 center = new Vector3(0, START_ALTITUDE, 0);
            Vector3 size = new Vector3(RoundController.Singleton.CurrentRound.BuildRadius, CHECK_HEIGHT, RoundController.Singleton.CurrentRound.BuildRadius);

            Physics.BoxCast(center, size, Vector3.down, out RaycastHit hit, Quaternion.identity, START_ALTITUDE + 10, Singleton.buildingBlockLayerMask, QueryTriggerInteraction.Ignore);

            CurrentBuildAltitude = hit.point.y;
        }

        /// <summary>
        /// Calculates the height of the stack (all building blocks) and returns the point at which the stack is the tallest.
        /// </summary>
        public static Vector3 CalculateStackHeight()
        {
            Vector3 center = new Vector3(0, START_ALTITUDE, 0);
            Vector3 size = new Vector3(RoundController.Singleton.CurrentRound.BuildRadius, CHECK_HEIGHT, RoundController.Singleton.CurrentRound.BuildRadius);

            RaycastHit[] hits = Physics.BoxCastAll(center, size, Vector3.down, Quaternion.identity, START_ALTITUDE + 10, Singleton.buildingBlockLayerMask, QueryTriggerInteraction.Ignore);

            if (hits.Length > 0)
            {
                int index = hits.Length - 1;

                while (index >= 0)
                {
                    if (hits[index].collider.GetComponent<BuildingBlockCopy>().IsGrounded)
                    {
                        return hits[index].point;
                    }
                    else
                    {
                        index--;
                    }
                }
            }

            return Vector3.zero;
        }

    }

}
