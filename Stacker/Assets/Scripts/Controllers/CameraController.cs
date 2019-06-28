using UnityEngine;

#pragma warning disable 0649
#pragma warning disable 0414

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
        [SerializeField]                private Transform rotateAnchor;
        [SerializeField]                private float     cameraRotateSpeed = 5f;
        [SerializeField, Range(0, 90f)] private float     minAngleX         = 20f;
        [SerializeField, Range(0, 90f)] private float     maxAngleX         = 80f;

        [Header("Zoom settings")]
        [SerializeField]                private float     zoomSpeed        = 5f;
        [SerializeField]                private Vector3   minZoomPosition  = new Vector3(5, 5, 5);
        [SerializeField]                private Vector3   maxZoomPosition  = new Vector3(25, 25, 25);
        [SerializeField, Range(0, 100)] private int       zoomStates       = 10;
        [SerializeField, Range(0, 100)] private int       defaultZoom      = 2;

        [Header("Misc")]
        [SerializeField] private LayerMask mainCameraEventMask;

        #endregion

        #region Private variables

        private int     currentZoom;
        private Vector3 targetZoomPosition;
        private Vector3 deltaZoom;

        private float angleX = 45f;
        private float angleY = 45f;

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
            this.currentZoom        = defaultZoom;
            this.targetZoomPosition = GetZoomLevelPosition(currentZoom);

            this.angleX = rotateAnchor.rotation.eulerAngles.x;
            this.angleY = rotateAnchor.rotation.eulerAngles.y;

            this.mainCamera.eventMask = mainCameraEventMask;
            this.ui3DOverlayCamera.eventMask = 0;
        }

        private void Update()
        {
            if (CanReadInput)
            {
                if (CanZoom)
                {
                    InputZoom();
                }

                if (CanRotate)
                {
                    InputRotate();
                }
            }
        }

        #endregion

        #region Rotating

        private void InputRotate()
        {
            Vector2 deltaAxes = Vector2.zero;

#if UNITY_STANDALONE
            if (Input.GetMouseButton(1))
            {
                deltaAxes = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            }
#elif UNITY_IOS || UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                deltaAxes = new Vector2(Input.touches[0].deltaPosition.x, Input.touches[0].deltaPosition.y);
            }
#endif

            angleX = Mathf.Clamp(angleX + deltaAxes.y * cameraRotateSpeed * -1, 20f, 80f);
            angleY += deltaAxes.x * cameraRotateSpeed;
            
            rotateAnchor.rotation = Quaternion.Euler(angleX, angleY, 0);
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
#elif UNITY_IOS || UNITY_ANDROID
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

    }

}
