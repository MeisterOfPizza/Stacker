using UnityEngine;

namespace Stacker.Controllers
{

    class GameController : Controller<GameController>
    {

        #region Editor

        [SerializeField] private GameObject quitButton;

        #endregion

        #region Static properties

        public static int TotalStars { get; private set; }

        #endregion

        public override void OnAwake()
        {
#if UNITY_IOS || UNITY_ANDROIOD
            quitButton.SetActive(false);
#endif
        }

        public static void GivePlayerStars(int stars)
        {
            TotalStars += stars;
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
