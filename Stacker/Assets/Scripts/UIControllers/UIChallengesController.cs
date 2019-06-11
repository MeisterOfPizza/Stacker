using Stacker.Controllers;
using Stacker.Rounds;
using Stacker.UI.Challenges;
using System.Linq;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.UIControllers
{

    class UIChallengesController : Controller<UIChallengesController>
    {

        #region Editor

        [SerializeField] private UIChallenge[] uiChallenges = new UIChallenge[3];

        #endregion

        /// <summary>
        /// Initialize the three (or less) round challenges.
        /// </summary>
        public void InitializeUIChallenges(RoundChallenge[] roundChallenges)
        {
            // Order the challenges by the amount of stars they give on completion.
            // This to put the most rewarding challenge at the top.
            roundChallenges = roundChallenges.OrderByDescending(rc => rc.StarsReward).ToArray();

            foreach (UIChallenge challenge in uiChallenges)
            {
                challenge.gameObject.SetActive(false);
            }

            for (int i = 0; i < roundChallenges.Length; i++)
            {
                uiChallenges[i].gameObject.SetActive(true);
                uiChallenges[i].Initialize(roundChallenges[i]);
            }
        }

        public void UpdateUIChallenges()
        {
            foreach (var uiChallenge in uiChallenges)
            {
                uiChallenge.UpdateUIChallenge();
            }
        }

    }

}
