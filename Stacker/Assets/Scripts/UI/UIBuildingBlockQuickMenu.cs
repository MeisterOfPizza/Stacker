using Stacker.Components;
using Stacker.Controllers;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649

namespace Stacker.UI
{

    class UIBuildingBlockQuickMenu : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private ConstructionBuildingBlock constructionBuildingBlock;

        [Header("UI References")]
        [SerializeField] private Button btnRotateX;
        [SerializeField] private Button btnRotateY;
        [SerializeField] private Button btnRotateZ;

        #endregion

        #region Private variables

        private BuildingBlock currentBuildingBlock;

        private bool isActive;

        #endregion

        #region Public properties

        public bool IsActive
        {
            set
            {
                isActive = value;

                gameObject.SetActive(value);
            }
        }

        #endregion

        public void Initialize(BuildingBlock buildingBlock)
        {
            this.currentBuildingBlock = buildingBlock;

            btnRotateX.enabled = buildingBlock.CanRotate;
            btnRotateY.enabled = buildingBlock.CanRotate;
            btnRotateZ.enabled = buildingBlock.CanRotate;
        }

        private void Update()
        {
            if (isActive)
            {
                transform.position = CameraController.MainCamera.WorldToScreenPoint(constructionBuildingBlock.transform.position); 
            }
        }

        #region Click events

        public void PlaceBlock()
        {
            BuildController.Singleton.PlaceBuildingBlock();
        }

        public void ResetBlock()
        {
            currentBuildingBlock.ResetBlock();
        }

        public void RotateX()
        {
            constructionBuildingBlock.TargetRotation *= Quaternion.Euler(90, 0, 0); // Rotate 90 degrees on the X-axis.
        }

        public void RotateY()
        {
            constructionBuildingBlock.TargetRotation *= Quaternion.Euler(0, 90, 0); // Rotate 90 degrees on the Y-axis.
        }

        public void RotateZ()
        {
            constructionBuildingBlock.TargetRotation *= Quaternion.Euler(0, 0, 90); // Rotate 90 degrees on the Z-axis.
        }

        #endregion

    }

}
