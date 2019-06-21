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
        /// Calculates the height of the stack (all building blocks) and returns the stack height.
        /// </summary>
        public static float CalculateStackHeight()
        {
            Vector3 center = new Vector3(0, START_ALTITUDE, 0);
            Vector3 size = new Vector3(RoundController.Singleton.CurrentRound.BuildRadius, CHECK_HEIGHT, RoundController.Singleton.CurrentRound.BuildRadius);

            RaycastHit[] hits = Physics.BoxCastAll(center, size, Vector3.down, Quaternion.identity, START_ALTITUDE + 10, Singleton.buildingBlockLayerMask, QueryTriggerInteraction.Ignore);

            if (hits.Length > 0)
            {
                float tallest = 0;
                int index = 0;

                while (index < hits.Length)
                {
                    if (hits[index].collider.GetComponent<BuildingBlockCopy>().IsGrounded && hits[index].point.y > tallest)
                    {
                        tallest = hits[index].point.y;
                    }

                    index++;
                }

                return tallest;
            }

            return 0;
        }

    }

}
