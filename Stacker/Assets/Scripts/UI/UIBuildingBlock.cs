using Stacker.Components;
using Stacker.Extensions.Components;
using Stacker.Templates.Rounds;
using TMPro;
using UnityEngine;

namespace Stacker.UI
{

    class UIBuildingBlock : MonoBehaviour
    {

        #region Editor

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

            buildingBlockPool = new GameObjectPool<BuildingBlock>(null, roundBuildingBlock.Prefab, roundBuildingBlock.Quantity);

            UpdateUI();
        }

        private void UpdateUI()
        {
            quantityLeftText.text = "x" + quantityLeft;
        }

        #region Block events

        public void ResetBlock(BuildingBlock buildingBlock)
        {
            buildingBlockPool.Despawn(buildingBlock);
            quantityLeft++;
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
