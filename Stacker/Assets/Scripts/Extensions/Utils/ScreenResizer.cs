using System.Collections.Generic;
using UnityEngine;

namespace Stacker.Extensions.Utils
{

    static class ScreenResizer
    {

        #region Private variables

        private static HashSet<IScreenResizeListener> screenResizeListeners = new HashSet<IScreenResizeListener>();

        private static Vector2Int resolution = new Vector2Int(Screen.width, Screen.height);

        #endregion

        #region Public properties

        public static Vector2 ScreenSize
        {
            get
            {
                return resolution;
            }
        }

        #endregion

        #region Public methods

        public static void SetResolution(int width, int height, FullScreenMode fullScreenMode, int preferredRefreshRate)
        {
            Screen.SetResolution(width, height, fullScreenMode, preferredRefreshRate);

            resolution = new Vector2Int(width, height);

            UpdateScreenSize(resolution);
        }

        public static void UpdateScreenSize(Vector2 size)
        {
            foreach (var listener in screenResizeListeners)
            {
                listener.ScreenResized(size);
            }
        }

        public static bool AddListener(IScreenResizeListener screenResizeListener)
        {
            return screenResizeListeners.Add(screenResizeListener);
        }

        public static bool RemoveListener(IScreenResizeListener screenResizeListener)
        {
            return screenResizeListeners.Remove(screenResizeListener);
        }

        #endregion

    }

}
