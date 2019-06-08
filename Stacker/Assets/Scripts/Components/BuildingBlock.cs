using Stacker.Controllers;
using Stacker.Templates.Rounds;
using Stacker.UI;
using UnityEngine;
using UnityEngine.EventSystems;

#pragma warning disable 0649

namespace Stacker.Components
{

    class BuildingBlock : MonoBehaviour
    {

        #region Constants

        private const float QUICK_MENU_SHOW_DURATION = 10f;

        #endregion

        #region Editor

        [Header("References")]
        [SerializeField] private MeshRenderer meshRenderer;

        [Header("Selection")]
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _selectedMaterial;

        #endregion

        #region Private variables

        private RoundBuildingBlock roundBuildingBlock;
        private UIBuildingBlock    uiBuildingBlock;

        private float lastClickCountdown;
        private bool  isSelected;

        private Mesh primaryMesh;

        #endregion

        #region Public properties

        public Mesh PrimaryMesh
        {
            get
            {
                return primaryMesh;
            }
        }

        public bool CanRotate
        {
            get
            {
                return roundBuildingBlock.CanRotate;
            }
        }

        #endregion

        public void Initialize(RoundBuildingBlock roundBuildingBlock, UIBuildingBlock uiBuildingBlock)
        {
            this.roundBuildingBlock = roundBuildingBlock;
            this.uiBuildingBlock    = uiBuildingBlock;
        }

        #region MonoBehaviour methods

        private void Awake()
        {
            primaryMesh = GetComponent<MeshFilter>().mesh;
        }

        private void Update()
        {
            if (isSelected)
            {
                lastClickCountdown += Time.deltaTime;

                if (lastClickCountdown <= 0)
                {
                    Deselect();
                }
            }
        }

        #endregion

        #region Selection

        public void ResetBlock()
        {
            uiBuildingBlock.ResetBlock(this);
        }

        public void Select()
        {
            if (!isSelected)
            {
                // Restart the countdown to deselection:
                lastClickCountdown = QUICK_MENU_SHOW_DURATION;
                isSelected         = true;

                BuildController.Singleton.SelectBuildingBlock(this);

                meshRenderer.material = _selectedMaterial;
            }
        }

        public void Deselect()
        {
            if (isSelected)
            {
                BuildController.Singleton.DeselectBuildingBlock();

                isSelected = false;

                meshRenderer.material = _defaultMaterial;
            }
        }

        #endregion

        #region Positioning

        public void PlaceBuildingBlock(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
        }

        #endregion

        #region Events systems

        private void OnMouseDown()
        {
            Select();
        }

        #endregion

    }

}
