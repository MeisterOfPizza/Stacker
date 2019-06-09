using Stacker.Building;
using Stacker.Controllers;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649

namespace Stacker.UI.Building
{

    class UIBuildingBlockQuickMenu : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private ConstructionBuildingBlock constructionBuildingBlock;

        [Header("UI References")]
        [SerializeField] private Button btnRemoveCopy;
        [SerializeField] private Button btnRotateX;
        [SerializeField] private Button btnRotateY;
        [SerializeField] private Button btnRotateZ;

        #endregion

        #region Private variables

        private BuildingBlock     currentBuildingBlock;
        private BuildingBlockCopy currentBuildingBlockCopy;
        private bool              isPreview;

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

        public void Initialize(BuildingBlockCopy buildingBlockCopy, bool isPreview)
        {
            this.currentBuildingBlockCopy = buildingBlockCopy;
            this.isPreview                = isPreview;

            btnRemoveCopy.gameObject.SetActive(!isPreview);

            btnRotateX.enabled = buildingBlockCopy.CanRotate;
            btnRotateY.enabled = buildingBlockCopy.CanRotate;
            btnRotateZ.enabled = buildingBlockCopy.CanRotate;
        }

        public void Initialize(BuildingBlock buildingBlock, bool isPreview)
        {
            this.currentBuildingBlock = buildingBlock;
            this.isPreview            = isPreview;

            btnRemoveCopy.gameObject.SetActive(!isPreview);

            btnRotateX.enabled = buildingBlock.RoundBuildingBlockTemplate.CanRotate;
            btnRotateY.enabled = buildingBlock.RoundBuildingBlockTemplate.CanRotate;
            btnRotateZ.enabled = buildingBlock.RoundBuildingBlockTemplate.CanRotate;
        }

        private void Update()
        {
            if (isActive)
            {
                transform.position = CameraController.MainCamera.WorldToScreenPoint(constructionBuildingBlock.transform.position); 
            }
        }

        #region Click events

        public void PlaceCopy()
        {
            if (isPreview)
            {
                BuildController.Singleton.AddCopy(currentBuildingBlock);
            }
            else
            {
                BuildController.Singleton.PlaceCopy();
            }
        }

        public void CancelCopy()
        {
            if (isPreview)
            {
                BuildController.Singleton.CancelPreviewCopy();
            }
            else
            {
                BuildController.Singleton.DeselectCopy();
            }
        }

        public void RemoveCopy()
        {
            if (!isPreview)
            {
                BuildController.Singleton.RemoveCopy();
            }
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
