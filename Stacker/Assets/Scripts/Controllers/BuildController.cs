using Stacker.Components;
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
        [SerializeField] private MeshFilter     constructionBlock;
        [SerializeField] private LineRenderer   constructionBlockLandLine;
        [SerializeField] private SpriteRenderer constructionBlockLandPoint;

        [Header("Building")]
        [SerializeField] private float     constructionBlockBuildHeight  = 10f;
        [SerializeField] private float     constructionBlockRotateSpeed  = 5f;
        [SerializeField] private LayerMask constructionBlockLayerMask;

        #endregion

        #region Private variables

        private BuildingBlock selectedBuildingBlock;

        #endregion

        #region Public properties

        /// <summary>
        /// Used by <see cref="BuildingBlock"/> to position itself when the user is finished editing.
        /// </summary>
        public Vector3 ConstructionBlockPosition
        {
            get
            {
                return constructionBlock.transform.position;
            }
        }

        #endregion

        public void SelectBuildingBlock(BuildingBlock buildingBlock)
        {
            uiBuildingBlockQuickMenu.Initialize(buildingBlock);
            uiBuildingBlockQuickMenu.IsActive = true;

            selectedBuildingBlock = buildingBlock;

            constructionBlock.gameObject.SetActive(true);
            constructionBlock.transform.rotation = Quaternion.identity;
            constructionBlock.mesh = buildingBlock.ConstructionMesh;
        }

        public void DeselectBuildingBlock()
        {
            uiBuildingBlockQuickMenu.IsActive = false;
            selectedBuildingBlock = null;

            constructionBlock.gameObject.SetActive(false);
        }

        public void MoveConstructionBlock(Vector3 worldPosition)
        {
            // Set the y coordinate to the construction build height to avoid hitting anything.
            worldPosition.y = constructionBlockBuildHeight;
            constructionBlock.transform.position = worldPosition;

            // Cast a ray down to determine where the block would land if the player dropped it:
            Ray ray = new Ray(worldPosition, Vector3.down);
            Physics.Raycast(ray, out RaycastHit hit, constructionBlockBuildHeight * 2, constructionBlockLayerMask);

            // Update the land line and land point sprite:
            constructionBlockLandLine.SetPosition(0, worldPosition);
            constructionBlockLandLine.SetPosition(0, hit.point);
            constructionBlockLandPoint.transform.position = hit.point + Vector3.up * 0.025f;
        }

        public void RotateConstructionBlock(Quaternion target)
        {
            constructionBlock.transform.rotation = Quaternion.Slerp(constructionBlock.transform.rotation, target, Time.fixedDeltaTime * constructionBlockRotateSpeed);
        }

    }

}
