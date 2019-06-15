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

        [Header("References")]
        [SerializeField] private Animator animator;

        [Header("Anchors")]
        [SerializeField] private RectTransform uiBuildMenuAnchor;
        [SerializeField] private RectTransform uiBuildingBlockAnchor;

        [Header("Containers")]
        [SerializeField] private Transform ui3DOverlayBuildingBlockContainer;

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
            ui3DOverlayBuildingBlockContainer.Clear();

            foreach (var buildingBlock in buildingBlocks)
            {
                GameObject uiElement = Instantiate(uiBuildingBlockPrefab, uiBuildingBlockAnchor);
                UIBuildingBlock uiBuildingBlock = uiElement.GetComponent<UIBuildingBlock>();
                GameObject icon3D = Instantiate(buildingBlock.RoundBuildingBlockTemplate.Icon3DPrefab, ui3DOverlayBuildingBlockContainer);

                uiBuildingBlock.Initialize(buildingBlock, icon3D);
                buildingBlock.SetUIElement(uiBuildingBlock);
            }

            animator.SetTrigger("Reveal");
        }

        public void StopBuildPhaseUI()
        {
            animator.SetTrigger("Hide");
        }

    }

}
