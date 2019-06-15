using Stacker.Building;
using Stacker.UI.Extensions;
using Stacker.UIControllers;
using TMPro;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.UI.Building
{

    class UIBuildingBlock : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private TMP_Text      quantityLeftText;
        [SerializeField] private DraggableRect draggableRect;

        #endregion

        #region Private variables

        private BuildingBlock buildingBlock;

        /// <summary>
        /// A GameObject in UI 3D overlay space.
        /// </summary>
        private GameObject icon3D;

        #endregion

        #region Public properties

        public BuildingBlock BuildingBlock
        {
            get
            {
                return buildingBlock;
            }
        }

        #endregion

        public void Initialize(BuildingBlock buildingBlock, GameObject icon3D)
        {
            this.buildingBlock = buildingBlock;
            this.icon3D        = icon3D;

            icon3D.GetComponent<Canvas3DFollow2D>().SetTarget(draggableRect.transform);

            draggableRect.DetachedParent = UIBuildController.Singleton.UIBuildMenuAnchor;

            UpdateUI();
        }

        public void UpdateUI()
        {
            quantityLeftText.text = "x" + buildingBlock.QuantityLeft;
        }

        #region Drag events

        public void PreviewCopy(Vector2 screenPosition)
        {
            buildingBlock.PreviewCopy(screenPosition);
        }

        #endregion

    }

}
