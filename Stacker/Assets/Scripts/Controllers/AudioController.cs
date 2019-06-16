using UnityEngine;

namespace Stacker.Controllers
{

    class AudioController : Controller<AudioController>
    {

        #region Public variables

        public float musicVolume   = 1f;
        public float effectsVolume = 1f;
        public float uiVolume      = 1f;

        #endregion

        #region Public static properties

        public static float MusicVolume
        {
            get
            {
                return Singleton.musicVolume;
            }

            set
            {
                Singleton.musicVolume = Mathf.Clamp01(value);
            }
        }

        public static float EffectsVolume
        {
            get
            {
                return Singleton.effectsVolume;
            }

            set
            {
                Singleton.effectsVolume = Mathf.Clamp01(value);
            }
        }

        public static float UIVolume
        {
            get
            {
                return Singleton.uiVolume;
            }

            set
            {
                Singleton.uiVolume = Mathf.Clamp01(value);
            }
        }

        #endregion

    }

}
