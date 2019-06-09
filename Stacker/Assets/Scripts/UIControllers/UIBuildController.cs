using Stacker.Building;
using Stacker.Controllers;
using Stacker.Extensions.Utils;
using Stacker.UI.Building;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.UIControllers
{

    class UIBuildController : Controller<UIBuildController>
    {

        #region Editor

        [Header("Anchors")]
        [SerializeField] private RectTransform uiBuildMenuAnchor;
        [SerializeField] private RectTransform uiBuildingBlockAnchor;

        [Header("Prefabs")]
        [SerializeField] private GameObject uiBuildingBlockPrefab;

        #endregion

        #region Public properties

        public RectTransform UIBuildMenuAnchor
        {
            get
            {
                return uiBuildMenuAnchor;
            }
        }

        #endregion

        public void BeginBuildPhaseUI(IList<BuildingBlock> buildingBlocks)
        {
            uiBuildingBlockAnchor.Clear();

            foreach (var buildingBlock in buildingBlocks)
            {
                GameObject uiElement = Instantiate(uiBuildingBlockPrefab, uiBuildingBlockAnchor);
                UIBuildingBlock uiBuildingBlock = uiElement.GetComponent<UIBuildingBlock>();
                uiBuildingBlock.Initialize(buildingBlock);
                buildingBlock.SetUIElement(uiBuildingBlock);
            }
        }

        public void StopBuildPhaseUI()
        {
            // TODO: Play some animations.
        }

    }

}
