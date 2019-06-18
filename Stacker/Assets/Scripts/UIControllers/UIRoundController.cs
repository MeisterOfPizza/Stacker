using Stacker.Controllers;
using TMPro;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.UIControllers
{

    class UIRoundController : Controller<UIRoundController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private TMP_Text currentRoundText;
        [SerializeField] private TMP_Text starCountText;
        [SerializeField] private TMP_Text highscoreStarCountText;

        [Header("Windows")]
        [SerializeField] private GameObject roundWonWindow;
        [SerializeField] private GameObject roundLostWindow;
        [SerializeField] private GameObject roundReadyWindow;

        #endregion

        public override void OnAwake()
        {
            UpdateHighscoreCount();
        }

        public void UpdateStarCount()
        {
            starCountText.text = GameController.TotalStars.ToString();
        }

        public void UpdateHighscoreCount()
        {
            highscoreStarCountText.text = GameController.Highscore.ToString();
        }

        public void UpdateCurrentRound(int round)
        {
            currentRoundText.text = "Round " + (round + 1);
        }

        public void WonRoundWindow()
        {
            roundWonWindow.SetActive(true);
        }

        public void LostRoundWindow()
        {
            roundLostWindow.SetActive(true);
        }

        #region Click events

        /// <summary>
        /// Ready a new game.
        /// </summary>
        public void NewGame()
        {
            RoundController.Singleton.CreateNewRound();

            roundReadyWindow.SetActive(true);
        }

        /// <summary>
        /// Ready a new round.
        /// </summary>
        public void NextRound()
        {
            RoundController.Singleton.CreateNewRound();

            roundReadyWindow.SetActive(true);
        }

        public void StartRound()
        {
            RoundController.Singleton.BeginRound();
        }

        #endregion

    }

}
