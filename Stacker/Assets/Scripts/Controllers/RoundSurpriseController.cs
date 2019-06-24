using Stacker.RoundSurprises;
using Stacker.Templates.RoundSurprises;
using System.Collections;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Controllers
{

    class RoundSurpriseController : Controller<RoundSurpriseController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private Transform roundSurpriseContainer;

        [Space]
        [SerializeField] private Transform ufoOffset;

        [Header("Prefabs")]
        [SerializeField] private RoundSurpriseTemplate[] roundSurpriseTemplates;

        #endregion

        #region Private variables

        private RoundSurprise[] roundSurprises;

        private RoundSurprise chosenRoundSurpriseTemplate;
        private bool          hasRoundSurpriseThisRound;

        #endregion

        #region Public properties

        public Transform UFOOffset
        {
            get
            {
                return ufoOffset;
            }
        }

        #endregion

        public override void OnAwake()
        {
            CreateRoundSurprises();
        }

        #region Phase cycles

        public void ResetRoundSurprise()
        {
            ChoseRandomRoundSurprise();
        }

        public IEnumerator AwaitBeforeBuildPhase()
        {
            if (hasRoundSurpriseThisRound && chosenRoundSurpriseTemplate.Template.Type == RoundSurpriseType.BeforeBuildPhase)
            {
                chosenRoundSurpriseTemplate.SpawnRoundSurprise();

                yield return StartCoroutine(chosenRoundSurpriseTemplate.StartRoundSurprise());
            }
        }

        public IEnumerator AwaitAfterBuildPhase()
        {
            if (hasRoundSurpriseThisRound)
            {
                if (chosenRoundSurpriseTemplate.Template.Type == RoundSurpriseType.BeforeBuildPhase) // Update the round surprise and remove it.
                {
                    chosenRoundSurpriseTemplate.RemoveRoundSurprise();
                }
                else
                {
                    chosenRoundSurpriseTemplate.SpawnRoundSurprise();

                    yield return StartCoroutine(chosenRoundSurpriseTemplate.StartRoundSurprise());

                    chosenRoundSurpriseTemplate.RemoveRoundSurprise();
                }
            }
        }

        #endregion

        #region Helpers

        private void CreateRoundSurprises()
        {
            roundSurprises = new RoundSurprise[roundSurpriseTemplates.Length];

            for (int i = 0; i < roundSurprises.Length; i++)
            {
                roundSurprises[i] = Instantiate(roundSurpriseTemplates[i].Prefab, roundSurpriseContainer).GetComponent<RoundSurprise>();
            }
        }

        private void ChoseRandomRoundSurprise()
        {
            if (Random.value >= 0.5f || true) //TEST: Remove test
            {
                hasRoundSurpriseThisRound = true;

                chosenRoundSurpriseTemplate = roundSurprises[Random.Range(0, roundSurprises.Length)];
            }
            else
            {
                hasRoundSurpriseThisRound = false;
            }
        }

        #endregion

    }

}
