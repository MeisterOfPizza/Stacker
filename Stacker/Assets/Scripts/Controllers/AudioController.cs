using UnityEngine;

namespace Stacker.Controllers
{

    class AudioController : Controller<AudioController>
    {

        #region Public variables

        [HideInInspector] public float musicVolume   = 1f;
        [HideInInspector] public float effectsVolume = 1f;
        [HideInInspector] public float miscVolume    = 1f;

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

        public static float MiscVolume
        {
            get
            {
                return Singleton.miscVolume;
            }

            set
            {
                Singleton.miscVolume = Mathf.Clamp01(value);
            }
        }

        #endregion

    }

}
