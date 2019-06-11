using UnityEngine;

namespace Stacker.Controllers
{

    class BorderController : Controller<BorderController>
    {

        #region Editor

        [SerializeField] private Transform borderContainer;

        #endregion

        public static void SetupBorder()
        {
            Singleton.borderContainer.gameObject.SetActive(true);
            Singleton.borderContainer.transform.localScale = new Vector3(RoundController.Singleton.CurrentRound.BuildRadius, 1, RoundController.Singleton.CurrentRound.BuildRadius);
        }

        public static void HideBorder()
        {
            Singleton.borderContainer.gameObject.SetActive(false);
        }

    }

}
