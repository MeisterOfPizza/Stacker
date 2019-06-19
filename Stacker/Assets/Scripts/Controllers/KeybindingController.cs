using UnityEngine;

namespace Stacker.Controllers
{

    class KeybindingController : Controller<KeybindingController>
    {

        #region Editor

        [Header("Standalone Axes")]
        [SerializeField] private string camera_Rotate = "Rotate";
        [SerializeField] private string camera_Zoom   = "Mouse ScrollWheel";

        [Header("Standalone Keys")]
        [SerializeField] private KeyCode build_Place   = KeyCode.Space;
        [SerializeField] private KeyCode build_Cancel  = KeyCode.Escape;
        [SerializeField] private KeyCode build_RotateX = KeyCode.A;
        [SerializeField] private KeyCode build_RotateY = KeyCode.S;
        [SerializeField] private KeyCode build_RotateZ = KeyCode.D;

        #endregion

        #region Public static properties

        // Standalone Axes //

        public static string Standalone_Camera_Rotate_Axis
        {
            get
            {
                return Singleton.camera_Rotate;
            }
        }

        public static string Standalone_Camera_Zoom_Axis
        {
            get
            {
                return Singleton.camera_Zoom;
            }
        }

        // Standalone Keys //

        public static KeyCode Standalone_Build_Place_KeyCode
        {
            get
            {
                return Singleton.build_Place;
            }
        }

        public static KeyCode Standalone_Build_Cancel_KeyCode
        {
            get
            {
                return Singleton.build_Cancel;
            }
        }

        public static KeyCode Standalone_Build_RotateX
        {
            get
            {
                return Singleton.build_RotateX;
            }
        }

        public static KeyCode Standalone_Build_RotateY
        {
            get
            {
                return Singleton.build_RotateY;
            }
        }

        public static KeyCode Standalone_Build_RotateZ
        {
            get
            {
                return Singleton.build_RotateZ;
            }
        }

        #endregion

    }

}
