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
        [SerializeField, Range(0, 100)] private int     zoomLevels       = 10;
        [SerializeField, Range(0, 100)] private int     defaultZoomLevel = 2;

        [Header("Misc")]
        [SerializeField] private LayerMask mainCameraEventMask;

        #endregion

        #region Private variables

        private int     currentZoomLevel;
        private Vector3 targetZoomPosition;
        private Vector3 deltaZoom;

        #endregion

        #region Public properties

        public bool CanReadInput { get; set; } = true;
        public bool CanRotate    { get; set; } = true;
        public bool CanZoom      { get; set; } = true;

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
            this.currentZoomLevel   = defaultZoomLevel;
            this.targetZoomPosition = GetZoomLevelPosition(currentZoomLevel);

            this.mainCamera.eventMask = mainCameraEventMask;
            this.ui3DOverlayCamera.eventMask = 0;
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

            currentZoomLevel = Mathf.Clamp(currentZoomLevel + zoomChange, 0, zoomLevels);

            if (zoomChange != 0)
            {
                targetZoomPosition = GetZoomLevelPosition(currentZoomLevel);
            }

            ZoomCamera();
        }

        private void ZoomCamera()
        {
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, targetZoomPosition, zoomSpeed * Time.deltaTime);
        }

        private Vector3 GetZoomLevelPosition(int zoomLevel)
        {
            return minZoomPosition + deltaZoom * (zoomLevel / (float)zoomLevels);
        }

#endregion

    }

}
