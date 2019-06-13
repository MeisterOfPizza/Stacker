using Stacker.Rounds;
using Stacker.Templates.Rounds;
using Stacker.UIControllers;
using System.Collections;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Controllers
{

    class RoundController : Controller<RoundController>
    {

        #region Editor

        [SerializeField] private RoundTemplate[] roundTemplates;

        #endregion

        #region Private variables

        private Round currentRound;

        private bool roundHasEnded;

        private int roundsPassedWithoutLoss;

        #endregion

        #region Public properties

        public Round CurrentRound
        {
            get
            {
                return currentRound;
            }
        }

        /// <summary>
        /// A percentage value (0...1) that indicates how far the build phase has come.
        /// </summary>
        public float BuildPhaseProgress { get; private set; }

        #endregion

        private void Start()
        {
            CreateNewRound();
        }

        #region Round cycle

        /// <summary>
        /// Creates a new round from a random round template.
        /// </summary>
        public void CreateNewRound()
        {
            StopCoroutine("RoundCycle");
            StopCoroutine("ActionPhase");
            roundHasEnded = false;

            currentRound = new Round(roundTemplates[Random.Range(0, roundTemplates.Length)]);

            ProjectileController.Singleton.ClearProjectiles();
            VehicleController.Singleton.ClearVehicles();
            ChallengesController.ResetChallengeValues();
            RoundCleanController.Singleton.ResetRound();

            UIChallengesController.Singleton.InitializeUIChallenges(currentRound.RoundChallenges);
        }

        /// <summary>
        /// Starts the round, allowing the player to build etc.
        /// </summary>
        public void BeginRound()
        {
            ChallengesController.ResetChallengeValues();

            ReadyChallengeControllers();
            BeginBuildPhase();
            StartCoroutine("RoundCycle");
        }

        private void BeginBuildPhase()
        {
            UIPhaseController.Singleton.BeginPhases(); // Set the current UI phase to build.

            CameraController.Singleton.CanReadInput = true;
            BorderController.SetupBorder();
            BuildController.Singleton.BeginBuildPhase(currentRound.Template.RoundBuildingBlockTemplates);
        }

        private IEnumerator RoundCycle()
        {
            float time = currentRound.TimeRestraint;

            // Keep the round going as long as we're using a time restraint and time > 0 OR we're not using a time restraint (aka forever).
            while (!currentRound.UseTimeRestraint || time > 0)
            {
                time -= Time.deltaTime;

                BuildPhaseProgress = 1 - time / currentRound.TimeRestraint;

                yield return new WaitForEndOfFrame();
            }

            // In case anything updated roundHasEnded between frames because of framelag.
            if (!roundHasEnded)
            {
                EndBuildPhase();

                StartCoroutine("ActionPhase");
            }
        }

        private IEnumerator ActionPhase()
        {
            UIPhaseController.Singleton.NextPhase(); // Set the current UI phase to fortress.
            yield return StartCoroutine(FortressPhase());
            ChallengesController.CheckFortressChallenges();

            UIPhaseController.Singleton.NextPhase(); // Set the current UI phase to tunnel.
            yield return StartCoroutine(TunnelPhase());
            ChallengesController.CheckTunnelChallenges();

            EndRound();
        }

        private void EndBuildPhase()
        {
            ChallengesController.CheckSkyscraperChallenges();

            BorderController.HideBorder();
            BuildController.Singleton.EndBuildPhase();
        }

        public void EndRound()
        {
            StopCoroutine("RoundCycle");

            UIPhaseController.Singleton.EndPhases();

            CameraController.Singleton.CanReadInput = false;
            
            roundHasEnded = true;

            CheckRoundWinLossState();
        }

        /// <summary>
        /// Check if the round was a win or a loss.
        /// </summary>
        private void CheckRoundWinLossState()
        {
            RoundCleanController.Singleton.CleanRound();

            // Count stars:
            int starsReceived = currentRound.RoundStarsReward();

            // Don't give any stars unless the player has placed at least one block.
            if (BuildController.PlacedBuildingBlockCopies > 0)
            {
                GameController.GivePlayerStars(starsReceived);
            }

            // The round was lost because the player did not gain any stars:
            if (starsReceived > 0 && BuildController.PlacedBuildingBlockCopies > 0)
            {
                roundsPassedWithoutLoss++;

                UIRoundController.Singleton.WonRoundWindow();
            }
            else
            {
                roundsPassedWithoutLoss = 0;

                UIRoundController.Singleton.LostRoundWindow();
            }

            UIRoundController.Singleton.UpdateStarCount();
            UIRoundController.Singleton.UpdateCurrentRound(roundsPassedWithoutLoss);
        }

        #endregion

        #region Challenge helpers

        private void ReadyChallengeControllers()
        {
            ProjectileController.Singleton.SetupProjectiles();
            VehicleController.Singleton.SetupVehicles(currentRound.ProminentTunnelChallenge);
        }

        private IEnumerator FortressPhase()
        {
            yield return StartCoroutine(ProjectileController.Singleton.FireProjectiles());
        }

        private IEnumerator TunnelPhase()
        {
            yield return StartCoroutine(VehicleController.Singleton.LaunchVehicles());
        }

        #endregion

    }

}