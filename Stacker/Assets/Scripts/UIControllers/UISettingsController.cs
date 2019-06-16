using Stacker.Controllers;
using TMPro;
using UnityEngine;

namespace Stacker.UIControllers
{

    class UISettingsController : Controller<UISettingsController>
    {

        #region Constants

        private string MUSIC_ON_PLAYER_PREFS_KEY   = "Music On";
        private string EFFECTS_ON_PLAYER_PREFS_KEY = "Effects On";
        private string MISC_ON_PLAYER_PREFS_KEY    = "Misc On";

        #endregion

        #region Editor

        [Header("Settings texts")]
        [SerializeField] private TMP_Text musicToggleText;
        [SerializeField] private TMP_Text effectsToggleText;
        [SerializeField] private TMP_Text miscToggleText;

        #endregion

        #region Private variables

        // Sound On/Off //

        private bool musicOn   = true;
        private bool effectsOn = true;
        private bool miscOn    = true;

        #endregion

        public override void OnAwake()
        {
            // Preload settings:
            musicOn   = PlayerPrefs.GetInt(MUSIC_ON_PLAYER_PREFS_KEY, 1) == 1;
            effectsOn = PlayerPrefs.GetInt(EFFECTS_ON_PLAYER_PREFS_KEY, 1) == 1;
            miscOn    = PlayerPrefs.GetInt(MISC_ON_PLAYER_PREFS_KEY, 1) == 1;

            // Apply settings:
            ApplyMusic();
            ApplyEffects();
            ApplyMisc();
        }

        public void ToggleMusic()
        {
            musicOn = !musicOn;

            PlayerPrefs.SetInt(MUSIC_ON_PLAYER_PREFS_KEY, musicOn ? 1 : 0);
            PlayerPrefs.Save();

            ApplyMusic();
        }

        public void ToggleEffects()
        {
            effectsOn = !effectsOn;

            PlayerPrefs.SetInt(EFFECTS_ON_PLAYER_PREFS_KEY, effectsOn ? 1 : 0);
            PlayerPrefs.Save();

            ApplyEffects();
        }

        public void ToggleMisc()
        {
            miscOn = !miscOn;

            PlayerPrefs.SetInt(MISC_ON_PLAYER_PREFS_KEY, miscOn ? 1 : 0);
            PlayerPrefs.Save();

            ApplyMisc();
        }

        private void ApplyMusic()
        {
            musicToggleText.text = musicOn ? "Music On" : "Music Off";
            AudioController.MusicVolume = musicOn ? 1 : 0;
        }

        private void ApplyEffects()
        {
            effectsToggleText.text = effectsOn ? "Effects On" : "Effects Off";
            AudioController.EffectsVolume = effectsOn ? 1 : 0;
        }

        private void ApplyMisc()
        {
            miscToggleText.text = miscOn ? "Misc On" : "Misc Off";
            AudioController.MiscVolume = miscOn ? 1 : 0;
        }

    }

}
