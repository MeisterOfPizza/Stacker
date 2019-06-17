using Stacker.Controllers;
using Stacker.UIControllers;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649

namespace Stacker.UI.Challenges
{

    class UIChallengeStar : MonoBehaviour
    {

        #region Private constants

        private const float FLY_SPEED = 5f;

        #endregion

        #region Editor

        [SerializeField] private Image image;

        #endregion

        /// <summary>
        /// Begin flying towards the target.
        /// </summary>
        public void StartPath()
        {
            StopCoroutine("SeekPath");
            StartCoroutine("SeekPath");
        }

        private IEnumerator SeekPath()
        {
            image.enabled = false;

            float randomDelay = Random.Range(0.5f, 1.5f);
            float distance    = Vector2.Distance(transform.position, UIChallengesController.UIChallengeStarTarget.position);

            yield return new WaitForSeconds(randomDelay);
            image.enabled = true;

            while (distance > 1f)
            {
                transform.position = Vector2.Lerp(transform.position, UIChallengesController.UIChallengeStarTarget.position, FLY_SPEED * Time.deltaTime);
                distance           = Vector2.Distance(transform.position, UIChallengesController.UIChallengeStarTarget.position);

                yield return new WaitForEndOfFrame();
            }

            GameController.GivePlayerStars(1);
            UIRoundController.Singleton.UpdateStarCount();
            UIChallengesController.Singleton.DespawnStar(this);
        }

    }

}
