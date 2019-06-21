using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Controllers
{

    class CameraController : Controller<CameraController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private Transform cameraContainer;
        [SerializeField] private Camera    mainCamera;
        [SerializeField] private Camera    ui3DOverlayCamera;

        [Header("Settings")]
        [SerializeField] private bool canReadInput = false;

        [Header("Rotate settings")]
        [SerializeField] private float cameraRotateSpeed = 5f;

        [Header("Zoom settings")]
        [SerializeField]                private float   zoomSpeed        = 5f;
        [SerializeField]                private Vector3 minZoomPosition  = new Vector3(5, 5, 5);
        [SerializeField]                private Vector3 maxZoomPosition  = new Vector3(25, 25, 25);
        [SerializeField, Range(0, 100)] private int     zoomStates       = 10;
        [SerializeField, Range(0, 100)] private int     defaultZoom      = 2;

        [Header("Leveling settings")]
        [SerializeField]                 private Transform levelAnchor;
        [SerializeField]                 private float     levelingSpeed = 5f;
        [SerializeField, Range(0f, 25f)] private float     maxLevel      = 10f;

        [Header("Misc")]
        [SerializeField] private LayerMask mainCameraEventMask;

        #endregion

        #region Private variables

        private int     currentZoom;
        private Vector3 targetZoomPosition;
        private Vector3 deltaZoom;

        private float currentLevel;

        #endregion

        #region Public properties

        public bool CanReadInput { get; set; } = true;
        public bool CanRotate    { get; set; } = true;
        public bool CanZoom      { get; set; } = true;
        public bool CanLevel     { get; set; } = true;

        #endregion

        #region Public static properties

        public static Camera MainCamera
        {
            get
            {
                return Singleton.mainCamera;
            }
        }

        public static Camera UI3DOverlayCamera
        {
            get
            {
                return Singleton.ui3DOverlayCamera;
            }
        }

        /// <summary>
        /// Left of <see cref="MainCamera"/>.
        /// </summary>
        public static Vector3 LeftOfCamera
        {
            get
            {
                return -MainCamera.transform.right;
            }
        }

        #endregion

        #region MonoBehaviour methods

        public override void OnAwake()
        {
            this.CanReadInput = canReadInput;

            this.deltaZoom          = maxZoomPosition - minZoomPosition;
            this.currentZoom        = defaultZoom;
            this.targetZoomPosition = GetZoomLevelPosition(currentZoom);
            
            this.levelAnchor.localPosition = new Vector3(this.levelAnchor.localPosition.x, currentLevel, this.levelAnchor.localPosition.z);

            this.mainCamera.eventMask = mainCameraEventMask;
            this.ui3DOverlayCamera.eventMask = 0;

            // Look at the world middle:
            mainCamera.transform.LookAt(Vector3.zero);
        }

        private void Update()
        {
            if (CanReadInput)
            {
                if (CanRotate)
                {
                    InputRotate();
                }

                if (CanZoom)
                {
                    InputZoom();
                }

                if (CanLevel)
                {
                    InputLevel();
                }

                // Look at the world middle:
                mainCamera.transform.LookAt(Vector3.zero);
            }
        }

        #endregion

        #region Rotating the camera

        /// <summary>
        /// Search for input to rotate by.
        /// </summary>
        private void InputRotate()
        {
            float direction = 0;

#if UNITY_STANDALONE
            direction = Input.GetAxis(KeybindingController.Standalone_Camera_Rotate_Axis);
            //TODO: Fix camera rotation on phone devices.
#elif UNITY_IOS || UNITY_ANDROID
#endif

            RotateCamera(direction);
        }

        private void RotateCamera(float direction)
        {
            cameraContainer.RotateAround(Vector3.zero, Vector3.up, direction * cameraRotateSpeed * 100 * Time.deltaTime);
        }

        #endregion

        #region Zooming

        private void InputZoom()
        {
            int zoomChange = 0;

#if UNITY_STANDALONE
            float zoomValue = Input.GetAxis(KeybindingController.Standalone_Camera_Zoom_Axis);

            if (zoomValue > 0)
            {
                zoomChange = -1;
            }
            else if (zoomValue < 0)
            {
                zoomChange = 1;
            }
            //TODO: Fix camera zoom on phone devices.
#elif UNITY_IOS || UNITY_ANDROIOD
#endif

            currentZoom = Mathf.Clamp(currentZoom + zoomChange, 0, zoomStates);

            if (zoomChange != 0)
            {
                targetZoomPosition = GetZoomLevelPosition(currentZoom);
            }

            ZoomCamera();
        }

        private void ZoomCamera()
        {
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, targetZoomPosition, zoomSpeed * Time.deltaTime);
        }

        private Vector3 GetZoomLevelPosition(int zoomLevel)
        {
            return minZoomPosition + deltaZoom * (zoomLevel / (float)zoomStates);
        }

        #endregion

        #region Leveling

        /// <summary>
        /// Level the camera as to move it closer or further to the ground (not the same as zoom).
        /// This only changes the Y-axis.
        /// </summary>
        private void InputLevel()
        {
            float levelDelta = 0;

#if UNITY_STANDALONE
            levelDelta = Input.GetAxis(KeybindingController.Standalone_Camera_Level_Axis);
            //TODO: Fix camera leveling on phone devices.
#elif UNITY_IOS || UNITY_ANDROID
#endif

            LevelCamera(levelDelta);
        }
        
        private void LevelCamera(float levelDelta)
        {
            currentLevel = Mathf.Clamp(currentLevel + levelDelta, 0f, maxLevel);

            float y = levelAnchor.localPosition.y;
            y = Mathf.Lerp(y, currentLevel, Time.deltaTime * levelingSpeed);

            levelAnchor.localPosition = new Vector3(levelAnchor.localPosition.x, y, levelAnchor.localPosition.z);
        }

        #endregion

    }

}
