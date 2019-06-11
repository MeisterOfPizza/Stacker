using Stacker.Building;
using Stacker.Extensions.Utils;
using Stacker.Templates.Rounds;
using Stacker.UI.Building;
using Stacker.UIControllers;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Controllers
{

    class BuildController : Controller<BuildController>
    {

        #region Editor

        [Header("UI Elements")]
        [SerializeField] private UIBuildingBlockQuickMenu uiBuildingBlockQuickMenu;
        [SerializeField] private string                   uiBuildingBlockQuickMenuTag = "UI Building Block Quick Menu";

        [Header("References")]
        [SerializeField] private Transform buildContainer;

        [Space]
        [SerializeField] private Transform                 constructionBuildingBlockContainer;
        [SerializeField] private ConstructionBuildingBlock constructionBuildingBlock;
        [SerializeField] private LineRenderer              constructionBuildingBlockLandLine;
        [SerializeField] private SpriteRenderer            constructionBuildingBlockLandPoint;

        [Header("Building")]
        [SerializeField] private LayerMask buildLayerMask;
        [SerializeField] private float     constructionBuildHeight = 5f;

        #endregion

        #region Private variables

        private List<BuildingBlock> buildingBlocks = new List<BuildingBlock>(25);

        private BuildingBlockCopy selectedBuildingBlock;

        private List<BuildingBlockCopy> placedBuildingBlockCopies = new List<BuildingBlockCopy>(100);

        private bool canBuild;

        #endregion

        #region Static properties

        public static Transform BuildContainer
        {
            get
            {
                return Singleton.buildContainer;
            }
        }

        public static bool CanBuild
        {
            get
            {
                return Singleton.canBuild;
            }
        }

        public static int PlacedBuildingBlockCopies
        {
            get
            {
                return Singleton.placedBuildingBlockCopies.Count;
            }
        }

        #endregion

        #region MonoBehaviour methods

        private void Update()
        {
            if (canBuild)
            {
                ChasePointer();
                BuildHeightController.CalculateCurrentBuildHeight();
            }
        }

        #endregion

        #region Build phases

        public void BeginBuildPhase(RoundBuildingBlockTemplate[] roundBuildingBlockTemplates)
        {
            this.canBuild = true;

            buildingBlocks.Clear();

            foreach (var template in roundBuildingBlockTemplates)
            {
                buildingBlocks.Add(new BuildingBlock(template));
            }

            UIBuildController.Singleton.BeginBuildPhaseUI(buildingBlocks);
        }

        public void EndBuildPhase()
        {
            canBuild = false;

            CancelPreviewCopy();
            DeselectCopy();
        }

        #endregion

        #region Building helpers

        private void ChasePointer()
        {
            int uiElementsUnderMouse = int.MaxValue;
            bool follow = false;
            Ray cameraRay;
            
#if UNITY_STANDALONE
            cameraRay = CameraController.MainCamera.ScreenPointToRay(Input.mousePosition);
            uiElementsUnderMouse = UtilExtensions.UIRaycastResults(Input.mousePosition, uiBuildingBlockQuickMenuTag).Count;
            follow = Input.GetMouseButton(0);
#elif UNITY_IOS || UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                cameraRay = CameraController.MainCamera.ScreenPointToRay(Input.GetTouch(0).position);
                uiElementsUnderMouse = UtilExtensions.UIRaycastResults(Input.mousePosition, uiBuildingBlockQuickMenuTag).Count;
                follow = Input.touchCount > 0;
            }
#endif

            if (follow && uiElementsUnderMouse == 0)
            {
                MoveConstructionBuildingBlock(cameraRay);
            }
        }

        #endregion

        #region Previewing building block copies

        /// <summary>
        /// Activates a hologram copy of the building block copy but does not activate the actual building block.
        /// </summary>
        public void PreviewCopy(BuildingBlock buildingBlock)
        {
            if (canBuild)
            {
                DeselectCopy();

                constructionBuildingBlock.SetConstructionBuildingBlockActive(true);
                constructionBuildingBlock.Initialize(buildingBlock.RoundBuildingBlockTemplate.Mesh);

                uiBuildingBlockQuickMenu.Initialize(buildingBlock, true);
                uiBuildingBlockQuickMenu.IsActive = true;
            }
        }

        /// <summary>
        /// Deactives the hologram copy of the current building block copy, preventing the actual building block from being activated.
        /// </summary>
        public void CancelPreviewCopy()
        {
            constructionBuildingBlock.SetConstructionBuildingBlockActive(false);
            uiBuildingBlockQuickMenu.IsActive = false;
        }

        #endregion

        #region Adding, removing and clearing building blocks

        /// <summary>
        /// Adds and places the currently selected building block, deactivating the hologram copy.
        /// </summary>
        public void AddCopy(BuildingBlock buildingBlock)
        {
            CancelPreviewCopy(); // The block still hasn't been placed yet, which means that we're technically still in preview mode.

            BuildingBlockCopy copy = buildingBlock.AddCopy();

            selectedBuildingBlock = copy;
            placedBuildingBlockCopies.Add(copy);

            PlaceCopy();
        }

        /// <summary>
        /// Removes the currently selected building block copy and places it back into "memory" (deactivating it).
        /// </summary>
        public void RemoveCopy()
        {
            placedBuildingBlockCopies.Remove(selectedBuildingBlock);
            selectedBuildingBlock.BuildingBlock.RemoveCopy(selectedBuildingBlock);

            DeselectCopy();
        }

        /// <summary>
        /// Removes all building blocks copies.
        /// </summary>
        public void ClearCopies()
        {
            CancelPreviewCopy();
            DeselectCopy();

            foreach (var buildingBlock in buildingBlocks)
            {
                buildingBlock.ClearCopies();
            }

            placedBuildingBlockCopies.Clear();
        }

        #endregion

        #region Selecting, deselecting and placing building blocks

        /// <summary>
        /// Selects a new copy and deactivates the old one.
        /// </summary>
        public void SelectCopy(BuildingBlockCopy buildingBlockCopy)
        {
            if (canBuild)
            {
                DeselectCopy();

                selectedBuildingBlock = buildingBlockCopy;
                selectedBuildingBlock.Select();

                constructionBuildingBlock.SetConstructionBuildingBlockActive(true);
                constructionBuildingBlock.Initialize(buildingBlockCopy.BuildingBlock.RoundBuildingBlockTemplate.Mesh);

                uiBuildingBlockQuickMenu.Initialize(buildingBlockCopy, false);
                uiBuildingBlockQuickMenu.IsActive = true;
            }
        }

        public void DeselectCopy()
        {
            if (selectedBuildingBlock != null)
            {
                selectedBuildingBlock.Deselect();
                selectedBuildingBlock = null;
            }

            constructionBuildingBlock.SetConstructionBuildingBlockActive(false);
            uiBuildingBlockQuickMenu.IsActive = false;
        }

        public void PlaceCopy()
        {
            selectedBuildingBlock.PlaceBuildingBlock(constructionBuildingBlock.TargetPosition, constructionBuildingBlock.TargetRotation);
            DeselectCopy();
        }

        #endregion

        #region Construction building block

        public void MoveConstructionBuildingBlock(Ray cameraRay)
        {
            Physics.Raycast(cameraRay, out RaycastHit cameraGroundHit, 100, buildLayerMask, QueryTriggerInteraction.Ignore);

            float buildHeight = BuildHeightController.CurrentBuildHeight + constructionBuildHeight;

            // Create the world position where the construction building block should be and
            // set the y coordinate to the construction build height to avoid hitting anything:
            Vector3 worldPosition = new Vector3(cameraGroundHit.point.x, buildHeight, cameraGroundHit.point.z);

            // Trap the world position inside the build area:
            float buildRadius = RoundController.Singleton.CurrentRound.BuildRadius;
            worldPosition = worldPosition.TrapInBox(new Vector3(-buildRadius, -100, -buildRadius), new Vector3(buildRadius, 100, buildRadius));

            constructionBuildingBlock.TargetPosition = worldPosition;

            // Cast a ray down to determine where the block would land if the player dropped it:
            Ray groundRay = new Ray(worldPosition, Vector3.down);
            Physics.Raycast(groundRay, out RaycastHit groundHit, buildHeight * 2, buildLayerMask, QueryTriggerInteraction.Ignore);

            // Update the land line and land point sprite:
            constructionBuildingBlockLandLine.SetPosition(0, worldPosition);
            constructionBuildingBlockLandLine.SetPosition(1, groundHit.point);
            constructionBuildingBlockLandPoint.transform.position = groundHit.point + Vector3.up * 0.025f;
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Calculates the height of the stack (all building blocks).
        /// </summary>
        public float CalculateStackHeight()
        {
            float highest = 0f;

            foreach (BuildingBlockCopy copy in placedBuildingBlockCopies)
            {
                if (copy.transform.position.y > highest)
                {
                    highest = copy.transform.position.y;
                }
            }

            return highest;
        }

        #endregion

    }

}
