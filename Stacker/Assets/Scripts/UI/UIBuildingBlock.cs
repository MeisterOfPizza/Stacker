using Stacker.Components;
using Stacker.Controllers;
using Stacker.Extensions.Components;
using Stacker.Templates.Rounds;
using TMPro;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.UI
{

    class UIBuildingBlock : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private TMP_Text quantityLeftText;

        #endregion

        #region Private variables

        private RoundBuildingBlock roundBuildingBlock;

        private GameObjectPool<BuildingBlock> buildingBlockPool;

        private int quantityLeft;

        #endregion

        #region Public properties

        public RoundBuildingBlock RoundBuildingBlock
        {
            get
            {
                return roundBuildingBlock;
            }
        }

        #endregion

        public void Initialize(RoundBuildingBlock roundBuildingBlock)
        {
            this.roundBuildingBlock = roundBuildingBlock;
            this.quantityLeft       = roundBuildingBlock.Quantity;

            SetupBuildingBlockPool();
            UpdateUI();
        }

        private void SetupBuildingBlockPool()
        {
            buildingBlockPool = new GameObjectPool<BuildingBlock>(null, roundBuildingBlock.Prefab, roundBuildingBlock.Quantity);

            foreach (var buildingBlock in buildingBlockPool.AvailableGameObjects)
            {
                buildingBlock.Initialize(roundBuildingBlock, this);
            }
        }

        private void UpdateUI()
        {
            quantityLeftText.text = "x" + quantityLeft;
        }

        #region Block events

        public void InitializeBlock(Vector2 dropPixelPosition)
        {
            if (quantityLeft > 0)
            {
                Ray ray = CameraController.MainCamera.ScreenPointToRay(dropPixelPosition);

                quantityLeft--;
                UpdateUI();

                BuildController.Singleton.InitializeBuildingBlockFromDrag(buildingBlockPool.Spawn(BuildController.Singleton.TemporaryBuildingBlockPosition, Quaternion.identity), ray);
            }
        }

        public void ResetBlock(BuildingBlock buildingBlock)
        {
            buildingBlockPool.Despawn(buildingBlock);
            quantityLeft++;

            UpdateUI();
        }

        public void ResetAllBlocks()
        {
            buildingBlockPool.DespawnAll();

            quantityLeft = roundBuildingBlock.Quantity;

            UpdateUI();
        }

        #endregion

    }

}
