using Stacker.Controllers;
using System.Globalization;
using TMPro;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.UIControllers
{

    class UIStackHeightController : Controller<UIStackHeightController>
    {

        #region Private constants

        private string STACK_HEIGHT_METER_NEW_HEIGHT_ANIMATION_NAME = "Pulse";

        #endregion

        #region Editor

        [SerializeField] private GameObject stackHeightMeterContainer;
        [SerializeField] private TMP_Text   stackHeightMeterText;
        [SerializeField] private Animator   stackHeightMeterTextAnimator;

        #endregion

        #region Private variables

        private CultureInfo textCultureInfo = new CultureInfo("en-US");

        private float previousStackHeight = 0;
        private bool  updateStackHeightText;

        #endregion

        #region MonoBehaviour methods

        private void Update()
        {
            if (updateStackHeightText)
            {
                UpdateStackHeightMeter(StackHeightController.CalculateStackHeight());
            }
        }

        #endregion

        #region Helpers

        public void ActivateUIHeightMeter(bool activate, bool updateStackHeightText)
        {
            this.stackHeightMeterContainer.SetActive(activate);
            this.updateStackHeightText = updateStackHeightText;
        }

        private void UpdateStackHeightMeter(float height)
        {
            if (Mathf.Abs(previousStackHeight - height) > 0.05f)
            {
                stackHeightMeterTextAnimator.Play(STACK_HEIGHT_METER_NEW_HEIGHT_ANIMATION_NAME);
            }

            previousStackHeight = height;

            stackHeightMeterText.text = height.ToString("F1", textCultureInfo) + "m";
        }

        #endregion

    }

}
