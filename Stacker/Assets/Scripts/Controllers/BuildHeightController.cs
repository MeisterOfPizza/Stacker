using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Controllers
{

    class BuildHeightController : Controller<BuildHeightController>
    {

        #region Constants

        private const float START_HEIGHT = 100f;
        private const float CHECK_HEIGHT = 0.1f;
        private const float INCREASE_Y   = CHECK_HEIGHT / 2f;

        #endregion

        #region Editor

        [SerializeField] private LayerMask buildingBlockLayerMask;

        #endregion

        #region Public properties

        public static float CurrentBuildHeight { get; private set; }

        #endregion

        public static float CalculateCurrentBuildHeight()
        {
            Vector3 pos     = new Vector3(0, START_HEIGHT, 0);
            Vector3 size    = new Vector3(RoundController.Singleton.CurrentRound.BuildRadius, CHECK_HEIGHT, RoundController.Singleton.CurrentRound.BuildRadius);
            float   current = START_HEIGHT;

            while (!Physics.CheckBox(pos, size, Quaternion.identity, Singleton.buildingBlockLayerMask, QueryTriggerInteraction.Ignore) && current >= 0)
            {
                pos.y -= INCREASE_Y;
                current = pos.y;
            }

            CurrentBuildHeight = current;

            return current;
        }

    }

}
