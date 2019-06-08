using Stacker.Controllers;
using Stacker.Templates.Rounds;
using Stacker.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Stacker.Components
{

    class BuildingBlock : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        #region Constants

        private const float QUICK_MENU_SHOW_DURATION = 10f;

        #endregion

        #region Editor

        [Header("References")]
        [SerializeField] private MeshRenderer meshRenderer;

        [Header("Selection")]
        [SerializeField] private Mesh constructionMesh;

        [SerializeField] private Shader _defaultBuildingBlockShader;

        /// <summary>
        /// Shader used on the actual building block (which is still in the same spot) while the user is moving the construction block.
        /// </summary>
        [SerializeField] private Shader _hologramShader;

        #endregion

        #region Private variables

        private RoundBuildingBlock roundBuildingBlock;
        private UIBuildingBlock    uiBuildingBlock;

        private float lastClickCountdown;
        private bool  isSelected;

        #endregion

        #region Public properties

        public Quaternion TargetRotation { get; set; }

        public Mesh ConstructionMesh
        {
            get
            {
                return constructionMesh;
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

        #region MonoBehaviour methods

        private void Awake()
        {
            TargetRotation = transform.rotation;
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

        private void FixedUpdate()
        {
            if (isSelected && roundBuildingBlock.CanRotate)
            {
                BuildController.Singleton.RotateConstructionBlock(TargetRotation);
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
            // Restart the countdown to deselection:
            lastClickCountdown = QUICK_MENU_SHOW_DURATION;
            isSelected         = true;

            BuildController.Singleton.SelectBuildingBlock(this);

            meshRenderer.material.shader = _hologramShader;
        }

        public void Deselect()
        {
            if (isSelected)
            {
                BuildController.Singleton.DeselectBuildingBlock();

                isSelected = false;

                meshRenderer.material.shader = _defaultBuildingBlockShader;
            }
        }

        #endregion

        #region Positioning

        private void FollowPointer()
        {
            Vector3 worldPosition;

#if UNITY_STANDALONE
            worldPosition = CameraController.MainCamera.ScreenToWorldPoint(Input.mousePosition);
#elif UNITY_IOS || UNITY_ANDROID
            worldPosition = CameraController.MainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
#endif

            BuildController.Singleton.MoveConstructionBlock(worldPosition);
        }

        private void PlaceBuildingBlock()
        {
            transform.position = BuildController.Singleton.ConstructionBlockPosition;
        }

        #endregion

        #region Events systems

        public void OnPointerClick(PointerEventData eventData)
        {
            Select();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            FollowPointer();
        }

        public void OnDrag(PointerEventData eventData)
        {
            FollowPointer();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            FollowPointer();
            PlaceBuildingBlock();
            Deselect();
        }

        #endregion

    }

}
