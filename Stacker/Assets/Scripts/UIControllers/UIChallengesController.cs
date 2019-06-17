using Stacker.Controllers;
using Stacker.Extensions.Components;
using Stacker.Rounds;
using Stacker.UI.Challenges;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649

namespace Stacker.UIControllers
{

    class UIChallengesController : Controller<UIChallengesController>
    {

        #region Editor

        [Header("UI Challenges")]
        [SerializeField] private RectTransform challengesRectTransform;
        [SerializeField] private UIChallenge[] uiChallenges = new UIChallenge[3];

        [Header("UI Challenge Stars")]
        [SerializeField] private RectTransform uiChallengeStarAnchor;
        [SerializeField] private RectTransform uiChallengeStarTarget;
        [SerializeField] private GameObject    uiChallengeStarPrefab;

        #endregion

        #region Private variables

        private GameObjectPool<UIChallengeStar> challengeStarPool;

        #endregion

        #region Public properties

        public static RectTransform UIChallengeStarTarget
        {
            get
            {
                return Singleton.uiChallengeStarTarget;
            }
        }

        #endregion

        public override void OnAwake()
        {
            challengeStarPool = new GameObjectPool<UIChallengeStar>(uiChallengeStarAnchor, uiChallengeStarPrefab, 9, Vector3.one);
        }

        public override void LateStart()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(challengesRectTransform);
        }

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

            Invoke("FixUIChallengesLayout", 0.25f);
        }

        /// <summary>
        /// Fix a small bug with the TMP_Pro and Unity layout system.
        /// </summary>
        private void FixUIChallengesLayout()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(challengesRectTransform);
        }

        public void UpdateUIChallenges()
        {
            foreach (var uiChallenge in uiChallenges)
            {
                uiChallenge.UpdateUIChallenge();
            }
        }

        public void SpawnStars(Vector3 position, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                challengeStarPool.Spawn(position, Quaternion.identity).StartPath();
            }
        }

        public void DespawnStar(UIChallengeStar star)
        {
            challengeStarPool.Despawn(star);
        }

    }

}
