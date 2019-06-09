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
        [SerializeField] private Transform                 constructionBuildingBlockContainer;
        [SerializeField] private ConstructionBuildingBlock constructionBuildingBlock;
        [SerializeField] private LineRenderer              constructionBuildingBlockLandLine;
        [SerializeField] private SpriteRenderer            constructionBuildingBlockLandPoint;

        [Header("Building")]
        [SerializeField] private LayerMask buildLayerMask;
        [SerializeField] private float     constructionBlockBuildHeight = 5f;

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

        #region MonoBehaviour methods

        private void Update()
        {
            if (selectedBuildingBlock != null)
            {
                ChasePointer();
            }
        }

        #endregion

        #region Construction of building blocks

        private void ChasePointer()
        {
            int uiElementsUnderMouse = int.MaxValue;
            bool follow = false;
            Ray cameraRay;
            
#if UNITY_STANDALONE
            cameraRay = CameraController.MainCamera.ScreenPointToRay(Input.mousePosition);
            uiElementsUnderMouse = UtilExtensions.UIRaycastResults(Input.mousePosition).Count;
            follow = Input.GetMouseButton(0);
#elif UNITY_IOS || UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                cameraRay = CameraController.MainCamera.ScreenPointToRay(Input.GetTouch(0).position);
                uiElementsUnderMouse = UtilExtensions.UIRaycastResults(Input.GetTouch(0).position).Count;
                follow = Input.touchCount > 0;
            }
#endif

            if (follow && uiElementsUnderMouse == 0)
            {
                MoveConstructionBuildingBlock(cameraRay);
            }
        }

        public void InitializeBuildingBlockFromDrag(BuildingBlock buildingBlock, Ray cameraRay)
        {
            buildingBlock.Select();
            MoveConstructionBuildingBlock(cameraRay);
        }

        public void SelectBuildingBlock(BuildingBlock buildingBlock)
        {
            uiBuildingBlockQuickMenu.Initialize(buildingBlock);
            uiBuildingBlockQuickMenu.IsActive = true;

            selectedBuildingBlock = buildingBlock;

            constructionBuildingBlockContainer.gameObject.SetActive(true);
            constructionBuildingBlock.transform.rotation = Quaternion.identity;

            constructionBuildingBlock.Initialize(buildingBlock);
        }

        public void DeselectBuildingBlock()
        {
            uiBuildingBlockQuickMenu.IsActive = false;
            selectedBuildingBlock = null;

            constructionBuildingBlockContainer.gameObject.SetActive(false);
        }

        public void MoveConstructionBuildingBlock(Ray cameraRay)
        {
            Physics.Raycast(cameraRay, out RaycastHit cameraGroundHit, 100, buildLayerMask, QueryTriggerInteraction.Ignore);

            // Create the world position where the construction building block should be and
            // set the y coordinate to the construction build height to avoid hitting anything:
            Vector3 worldPosition = new Vector3(cameraGroundHit.point.x, constructionBlockBuildHeight, cameraGroundHit.point.z);

            // Trap the world position inside the build area:
            float buildRadius = RoundController.Singleton.CurrentRound.BuildRadius;
            worldPosition = worldPosition.TrapInBox(new Vector3(-buildRadius, -1000, -buildRadius), new Vector3(buildRadius, 1000, buildRadius));

            constructionBuildingBlock.transform.position = worldPosition;

            // Cast a ray down to determine where the block would land if the player dropped it:
            Ray groundRay = new Ray(worldPosition, Vector3.down);
            Physics.Raycast(groundRay, out RaycastHit groundHit, constructionBlockBuildHeight * 2, buildLayerMask, QueryTriggerInteraction.Ignore);

            // Update the land line and land point sprite:
            constructionBuildingBlockLandLine.SetPosition(0, worldPosition);
            constructionBuildingBlockLandLine.SetPosition(1, groundHit.point);
            constructionBuildingBlockLandPoint.transform.position = groundHit.point + Vector3.up * 0.025f;
        }

        public void PlaceBuildingBlock()
        {
            selectedBuildingBlock.PlaceBuildingBlock(constructionBuildingBlock.transform.position, constructionBuildingBlock.TargetRotation);
            selectedBuildingBlock.Deselect();
        }

        #endregion

    }

}
