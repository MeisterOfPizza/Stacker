using Stacker.Controllers;
using Stacker.Extensions.Utils;
using Stacker.Rounds;
using Stacker.Templates.Rounds;
using Stacker.UI;
using UnityEngine;

namespace Stacker.UIControllers
{

    class UIBuildController : Controller<UIBuildController>
    {

        #region Editor

        [Header("Anchors")]
        [SerializeField] private RectTransform uiBuildingBlockAnchor;

        [Header("Prefabs")]
        [SerializeField] private GameObject uiBuildingBlockPrefab;

        #endregion

        public void StartBuildPhaseUI(RoundBuildingBlock[] roundBuildingBlocks)
        {
            uiBuildingBlockAnchor.Clear();

            foreach (var roundBuildingBlock in roundBuildingBlocks)
            {
                Instantiate(uiBuildingBlockPrefab, uiBuildingBlockAnchor).GetComponent<UIBuildingBlock>().Initialize(roundBuildingBlock);
            }
        }

        public void StopBuildPhaseUI()
        {
            // TODO: Play some animations.
        }

    }

}
