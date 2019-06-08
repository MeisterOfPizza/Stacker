using Stacker.Components;
using Stacker.Extensions.Utils;
using Stacker.UI;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Controllers
{

    class BuildController : Controller<BuildController>
    {

        #region Editor

        [Header("UI Elements")]
        [SerializeField] private UIBuildingBlockQuickMenu uiBuildingBlockQuickMenu;

        [Header("References")]
        [SerializeField] private ConstructionBuildingBlock constructionBuildingBlock;
        [SerializeField] private LineRenderer              constructionBlockLandLine;
        [SerializeField] private SpriteRenderer            constructionBlockLandPoint;

        [Header("Building")]
        [SerializeField] private float     constructionBlockBuildHeight = 5f;
        [SerializeField] private LayerMask constructionBuildingBlockLayerMask;

        [Header("Storing building blocks")]
        [SerializeField] private Vector3 temporaryBuildingBlockPosition;

        #endregion

        #region Private variables

        private BuildingBlock selectedBuildingBlock;

        #endregion

        #region Public properties

        public Vector3 TemporaryBuildingBlockPosition
        {
            get
            {
                return temporaryBuildingBlockPosition;
            }
        }

        #endregion

        #region Construction of building blocks

        public void InitializeBuildingBlockFromDrag(BuildingBlock buildingBlock, Ray ray)
        {
            Physics.Raycast(ray, out RaycastHit hit, 100);

            float buildRadius = RoundController.Singleton.CurrentRound.BuildRadius;
            Vector3 hitPoint = hit.point.TrapInBox(new Vector3(-buildRadius, -1000, -buildRadius), new Vector3(buildRadius, 1000, buildRadius));

            buildingBlock.Select();
            MoveConstructionBuildingBlock(hitPoint);
        }

        public void SelectBuildingBlock(BuildingBlock buildingBlock)
        {
            uiBuildingBlockQuickMenu.Initialize(buildingBlock);
            uiBuildingBlockQuickMenu.IsActive = true;

            selectedBuildingBlock = buildingBlock;

            constructionBuildingBlock.gameObject.SetActive(true);
            constructionBuildingBlock.transform.rotation = Quaternion.identity;

            constructionBuildingBlock.Initialize(buildingBlock);
        }

        public void DeselectBuildingBlock()
        {
            uiBuildingBlockQuickMenu.IsActive = false;
            selectedBuildingBlock = null;

            constructionBuildingBlock.gameObject.SetActive(false);
        }

        public void MoveConstructionBuildingBlock(Vector3 worldPosition)
        {
            // Set the y coordinate to the construction build height to avoid hitting anything.
            worldPosition.y = constructionBlockBuildHeight;
            constructionBuildingBlock.transform.position = worldPosition;

            // Cast a ray down to determine where the block would land if the player dropped it:
            Ray ray = new Ray(worldPosition, Vector3.down);
            Physics.Raycast(ray, out RaycastHit hit, constructionBlockBuildHeight * 2, ~constructionBuildingBlockLayerMask);

            // Update the land line and land point sprite:
            constructionBlockLandLine.SetPosition(0, worldPosition);
            constructionBlockLandLine.SetPosition(0, hit.point);
            constructionBlockLandPoint.transform.position = hit.point + Vector3.up * 0.025f;
        }

        public void PlaceBuildingBlock()
        {
            selectedBuildingBlock.PlaceBuildingBlock(constructionBuildingBlock.transform.position, constructionBuildingBlock.TargetRotation);
            selectedBuildingBlock.Deselect();
        }

        #endregion

    }

}
