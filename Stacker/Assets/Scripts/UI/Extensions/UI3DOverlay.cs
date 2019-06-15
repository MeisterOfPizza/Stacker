using Stacker.Extensions.Utils;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649

namespace Stacker.UI.Extensions
{

    [RequireComponent(typeof(RawImage))]
    class UI3DOverlay : MonoBehaviour, IScreenResizeListener
    {

        #region Editor

        [SerializeField] private Camera   hostCamera;
        [SerializeField] private RawImage rawImage;

        #endregion

        #region Private variables

        private RenderTexture renderTexture;

        #endregion

        private void Awake()
        {
            CreateRenderTexture(ScreenResizer.ScreenSize);

            rawImage.enabled = true;

            ScreenResizer.AddListener(this);
        }

        #region Helper methods

        private void CreateRenderTexture(Vector2 size)
        {
            renderTexture?.Release(); // Relase resources used by the render texture.

            renderTexture = new RenderTexture((int)size.x, (int)size.y, 24);

            hostCamera.targetTexture = renderTexture;
            rawImage.texture         = renderTexture;
        }

        #endregion

        private void Reset()
        {
            rawImage = GetComponent<RawImage>();
            rawImage.enabled = false;
        }

        #region IScreenResizeListener

        public void ScreenResized(Vector2 size)
        {
            CreateRenderTexture(size);
        }

        #endregion

    }

}
