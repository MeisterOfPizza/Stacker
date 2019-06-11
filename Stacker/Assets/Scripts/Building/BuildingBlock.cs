using Stacker.Controllers;
using Stacker.Extensions.Components;
using Stacker.Templates.Rounds;
using Stacker.UI.Building;
using UnityEngine;

namespace Stacker.Building
{

    class BuildingBlock
    {

        #region Public properties

        public RoundBuildingBlockTemplate RoundBuildingBlockTemplate { get; private set; }
        public UIBuildingBlock            UIBuildingBlock            { get; private set; }
        public BuildingBlockCopy[]        BuildingBlockCopies        { get; private set; }

        public GameObjectPool<BuildingBlockCopy> BuildingBlockCopyPool { get; private set; }
        public int                               QuantityLeft          { get; private set; }

        #endregion

        public BuildingBlock(RoundBuildingBlockTemplate roundBuildingBlockTemplate)
        {
            this.RoundBuildingBlockTemplate = roundBuildingBlockTemplate;
            this.QuantityLeft               = roundBuildingBlockTemplate.Quantity;

            SetupBuildingBlockCopyPool();
        }

        public void SetUIElement(UIBuildingBlock uiBuildingBlock)
        {
            this.UIBuildingBlock = uiBuildingBlock;
        }

        private void SetupBuildingBlockCopyPool()
        {
            BuildingBlockCopyPool = new GameObjectPool<BuildingBlockCopy>(BuildController.BuildContainer, RoundBuildingBlockTemplate.Prefab, RoundBuildingBlockTemplate.Quantity);

            foreach (var buildingBlock in BuildingBlockCopyPool.AvailableGameObjects)
            {
                buildingBlock.Initialize(this);
            }
        }

        private void UpdateUI()
        {
            UIBuildingBlock.UpdateUI();
        }

        public void PreviewCopy(Vector2 screenPosition)
        {
            if (QuantityLeft > 0)
            {
                Ray ray = CameraController.MainCamera.ScreenPointToRay(screenPosition);

                BuildController.Singleton.PreviewCopy(this);
                BuildController.Singleton.MoveConstructionBuildingBlock(ray);
            }
        }

        public BuildingBlockCopy AddCopy()
        {
            QuantityLeft--;
            UpdateUI();

            return BuildingBlockCopyPool.Spawn();
        }

        public void RemoveCopy(BuildingBlockCopy buildingBlockCopy)
        {
            BuildingBlockCopyPool.Despawn(buildingBlockCopy);

            QuantityLeft++;
            UpdateUI();
        }

        public void ClearCopies()
        {
            BuildingBlockCopyPool.DespawnAll();

            QuantityLeft = RoundBuildingBlockTemplate.Quantity;
            UpdateUI();
        }

    }

}
