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
        [SerializeField] private GameObject                constructionBuildingBlockContainer;
        [SerializeField] private ConstructionBuildingBlock constructionBuildingBlock;

        [Header("Building")]
        [SerializeField, Tooltip("What layers can building blocks be placed on?")] private LayerMask buildLayerMask;
        [SerializeField, Tooltip("How high up should the construction block be?")] private float     constructionBuildHeight = 5f;

        #endregion

        #region Private variables

        private List<BuildingBlock>     buildingBlocks            = new List<BuildingBlock>(25);
        private List<BuildingBlockCopy> placedBuildingBlockCopies = new List<BuildingBlockCopy>(100);
        
        private BuildingBlockCopy selectedBuildingBlockCopy;
        
        private bool hasSelectedBlock;
        private bool canBuild;

        #endregion

        #region Public static properties

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

        /// <summary>
        /// What layers can building blocks be placed on?
        /// </summary>
        public static LayerMask BuildLayerMask
        {
            get
            {
                return Singleton.buildLayerMask;
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

                if (hasSelectedBlock)
                {
                    ListenForBuildHotkeys();
                }
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

            UIBuildController.Singleton.StopBuildPhaseUI();
        }

        #endregion

        #region Building helpers

        private Ray GetPointerRay()
        {
#if UNITY_STANDALONE
            return CameraController.MainCamera.ScreenPointToRay(Input.mousePosition);
#elif UNITY_IOS || UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                return CameraController.MainCamera.ScreenPointToRay(Input.GetTouch(0).position);
            }
            else
            {
                return new Ray(CameraController.MainCamera.transform.position, CameraController.MainCamera.transform.forward);
            }
#endif
        }

        private void ChasePointer()
        {
            int uiElementsUnderMouse = int.MaxValue;
            bool follow = false;

#if UNITY_STANDALONE
            uiElementsUnderMouse = UtilExtensions.UIRaycastResults(Input.mousePosition, uiBuildingBlockQuickMenuTag).Count;
            follow = Input.GetMouseButton(0);
#elif UNITY_IOS || UNITY_ANDROID
            uiElementsUnderMouse = UtilExtensions.UIRaycastResults(Input.mousePosition, uiBuildingBlockQuickMenuTag).Count;
            follow = Input.touchCount > 0;
#endif

            if (follow && uiElementsUnderMouse == 0)
            {
                MoveConstructionBuildingBlock(GetPointerRay());
            }
        }

        /// <summary>
        /// Listen for any hotkey presses during this frame that will aid the player with building.
        /// </summary>
        private void ListenForBuildHotkeys()
        {
            if (Input.GetKeyDown(KeybindingController.Standalone_Build_Place_KeyCode))
            {
                uiBuildingBlockQuickMenu.PlaceCopy();
            }
            else if (Input.GetKeyDown(KeybindingController.Standalone_Build_Cancel_KeyCode))
            {
                uiBuildingBlockQuickMenu.CancelCopy();
            }
            else if (Input.GetKeyDown(KeybindingController.Standalone_Build_RotateX))
            {
                uiBuildingBlockQuickMenu.RotateX();
            }
            else if (Input.GetKeyDown(KeybindingController.Standalone_Build_RotateY))
            {
                uiBuildingBlockQuickMenu.RotateY();
            }
            else if (Input.GetKeyDown(KeybindingController.Standalone_Build_RotateZ))
            {
                uiBuildingBlockQuickMenu.RotateZ();
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
                
                hasSelectedBlock = true;

                constructionBuildingBlockContainer.SetActive(true);
                constructionBuildingBlock.Initialize(buildingBlock.RoundBuildingBlockTemplate.Template);

                MoveConstructionBuildingBlock(GetPointerRay(), true); // Set the block where it needs to be before moving on to the next frame.

                uiBuildingBlockQuickMenu.Initialize(buildingBlock, true);
                uiBuildingBlockQuickMenu.IsActive = true;
            }
        }

        /// <summary>
        /// Deactives the hologram copy of the current building block copy, preventing the actual building block from being activated.
        /// </summary>
        public void CancelPreviewCopy()
        {
            hasSelectedBlock = false;

            constructionBuildingBlockContainer.SetActive(false);
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

            selectedBuildingBlockCopy = copy;
            placedBuildingBlockCopies.Add(copy);

            PlaceCopy();
        }

        /// <summary>
        /// Removes the currently selected building block copy and places it back into "memory" (deactivating it).
        /// </summary>
        public void RemoveCopy()
        {
            placedBuildingBlockCopies.Remove(selectedBuildingBlockCopy);
            selectedBuildingBlockCopy.BuildingBlock.RemoveCopy(selectedBuildingBlockCopy);

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
            if (canBuild && !hasSelectedBlock)
            {
                DeselectCopy();

                hasSelectedBlock = true;

                selectedBuildingBlockCopy = buildingBlockCopy;
                selectedBuildingBlockCopy.Select();

                constructionBuildingBlockContainer.SetActive(true);
                constructionBuildingBlock.Initialize(buildingBlockCopy.BuildingBlock.RoundBuildingBlockTemplate.Template);

                MoveConstructionBuildingBlock(GetPointerRay(), true); // Set the block where it needs to be before moving on to the next frame.

                uiBuildingBlockQuickMenu.Initialize(buildingBlockCopy, false);
                uiBuildingBlockQuickMenu.IsActive = true;
            }
        }

        public void DeselectCopy()
        {
            if (selectedBuildingBlockCopy != null)
            {
                selectedBuildingBlockCopy.Deselect();
                selectedBuildingBlockCopy = null;
            }

            hasSelectedBlock = false;

            constructionBuildingBlockContainer.SetActive(false);
            uiBuildingBlockQuickMenu.IsActive = false;
        }

        public void PlaceCopy()
        {
            selectedBuildingBlockCopy.PlaceBuildingBlock(constructionBuildingBlock.Position, constructionBuildingBlock.TargetRotation);
            DeselectCopy();
        }

        #endregion

        #region Construction building block

        public void MoveConstructionBuildingBlock(Ray cameraRay, bool teleport = false)
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

            if (teleport)
            {
                constructionBuildingBlock.Teleport(worldPosition);
            }
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
