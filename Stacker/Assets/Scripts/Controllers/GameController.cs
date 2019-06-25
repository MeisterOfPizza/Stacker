using Stacker.UIControllers;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Controllers
{

    class GameController : Controller<GameController>
    {

        #region Private constants

        private const string PLAYER_PREFS_HIGHSCORE_KEY = "Highscore";

        #endregion

        #region Editor

        [Header("References")]
        [SerializeField] private GameObject quitButton;

        [Header("UI References")]
        [SerializeField] private Animator newHighscoreTextPopupAnimator;

        [Header("Audio")]
        [SerializeField] private AudioSource newHighscoreAudioSource;
        [SerializeField] private AudioClip   newHighscoreSoundEffect;

        #endregion

        #region Static properties

        public static int TotalStars { get; private set; }

        public static int Highscore
        {
            get
            {
                return PlayerPrefs.GetInt(PLAYER_PREFS_HIGHSCORE_KEY, 0);
            }
        }

        #endregion

        public override void OnAwake()
        {
#if UNITY_IOS || UNITY_ANDROID
            quitButton.SetActive(false);
#endif
        }

        public static void GivePlayerStars(int stars)
        {
            TotalStars += stars;

            if (PlayerPrefs.GetInt(PLAYER_PREFS_HIGHSCORE_KEY, 0) < TotalStars)
            {
                PlayerPrefs.SetInt(PLAYER_PREFS_HIGHSCORE_KEY, TotalStars);
                PlayerPrefs.Save();

                UIRoundController.Singleton.UpdateHighscoreCount();

                Singleton.newHighscoreTextPopupAnimator.Play("Popup");

                Singleton.newHighscoreAudioSource.PlayOneShot(Singleton.newHighscoreSoundEffect, AudioController.MiscVolume * 0.5f);
            }
        }

        public static void ResetStars()
        {
            TotalStars = 0;
        }

        public void QuitGame()
        {
            PlayerPrefs.Save();
            Application.Quit();
        }

    }

}
