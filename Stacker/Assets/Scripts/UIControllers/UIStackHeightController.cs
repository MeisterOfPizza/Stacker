using Stacker.Controllers;
using TMPro;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.UIControllers
{

    class UIStackHeightController : Controller<UIStackHeightController>
    {

        #region Editor

        [SerializeField] private RectTransform uiStackHeightMeter;
        [SerializeField] private TMP_Text      uiStackHeightMeterText;

        #endregion

        #region Private variables

        private bool isActivated;

        #endregion

        #region MonoBehaviour methods

        private void Update()
        {
            if (isActivated)
            {
                PositionMeter(StackHeightController.CalculateStackHeight());
            }
        }

        #endregion

        #region Helpers

        public void ActivateUIHeightMeter(bool activate)
        {
            uiStackHeightMeter.gameObject.SetActive(activate);
            isActivated = activate;
        }

        private void PositionMeter(Vector3 tallestPoint)
        {
            uiStackHeightMeter.position = CameraController.MainCamera.WorldToScreenPoint(tallestPoint);
            uiStackHeightMeterText.text = tallestPoint.y.ToString("F1") + "m";
        }

        #endregion

    }

}
