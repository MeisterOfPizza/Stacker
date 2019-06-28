using Stacker.Controllers;
using TMPro;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.UIControllers
{

    class UITutorialController : Controller<UITutorialController>
    {

        #region Editor

        [SerializeField] private TMP_Text tutorialText;

        #endregion

        public override void OnAwake()
        {
#if UNITY_STANDALONE
            tutorialText.text += "\n\\u2022<indent=30px>Rotate the camera by holding down right mouse button and dragging. Zoom in and out by using the scroll wheel.</indent>";
            // Whenever adding unicode text to the TMP_Text, all unicode references gets reset, therefore, we need to manually replace all "fake" unicode character with the "real" ones.
            tutorialText.text = tutorialText.text.Replace("\\u2022", "\u2022");
#elif UNITY_IOS || UNITY_ANDROIOD
            tutorialText.text += "\n\\u2022<indent=30px>Rotate the camera by swiping your finger across the screen. Zoom in and out by using two fingers.</indent>";
            // Whenever adding unicode text to the TMP_Text, all unicode references gets reset, therefore, we need to manually replace all "fake" unicode character with the "real" ones.
            tutorialText.text = tutorialText.text.Replace("\\u2022", "\u2022");
#endif
        }

    }

}
