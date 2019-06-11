using Stacker.Rounds;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649

namespace Stacker.UI.Challenges
{

    class UIChallenge : MonoBehaviour
    {

        #region Editor

        [SerializeField] private TMP_Text challengeName;
        [SerializeField] private TMP_Text challengeDescription;

        [Space]
        [SerializeField] private Image[] starRewardImages = new Image[3];

        #endregion

        #region Private variables

        private RoundChallenge roundChallenge;

        #endregion

        public void Initialize(RoundChallenge roundChallenge)
        {
            this.roundChallenge = roundChallenge;

            challengeName.text        = roundChallenge.RoundChallengeType.ToString();
            challengeDescription.text = roundChallenge.Description;

            challengeName.fontStyle        &= FontStyles.Strikethrough;
            challengeDescription.fontStyle &= FontStyles.Strikethrough;

            // Deactivate all star reward images beforehand:
            for (int i = 0; i < starRewardImages.Length; i++)
            {
                starRewardImages[i].gameObject.SetActive(false);
            }

            // Now, activate all star reward images that should be active:
            for (int i = 0; i < roundChallenge.StarsReward; i++)
            {
                starRewardImages[i].gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Update the UI.
        /// </summary>
        public void UpdateUIChallenge()
        {
            if (roundChallenge.IsCompleted)
            {
                challengeName.fontStyle        |= FontStyles.Strikethrough;
                challengeDescription.fontStyle |= FontStyles.Strikethrough;
            }
        }

    }

}
