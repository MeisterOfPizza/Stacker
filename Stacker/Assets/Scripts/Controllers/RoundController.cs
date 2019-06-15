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

        #region Round cycle

        /// <summary>
        /// Creates a new round from a random round template.
        /// </summary>
        public void CreateNewRound()
        {
            BorderController.HideBorder();
            RoundCleanController.Singleton.CleanRound();

            StopCoroutine("RoundCycle");
            StopCoroutine("ActionPhase");
            roundHasEnded = false;

            currentRound = new Round(roundTemplates[Random.Range(0, roundTemplates.Length)]);
            
            ChallengesController.ResetChallengeValues();

            UIChallengesController.Singleton.InitializeUIChallenges(currentRound.RoundChallenges);
        }

        /// <summary>
        /// Starts the round, allowing the player to build etc.
        /// </summary>
        public void BeginRound()
        {
            ProjectileController.Singleton.ClearProjectiles();
            VehicleController.Singleton.ClearVehicles();
            RoundCleanController.Singleton.ResetRound();

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

        private void EndBuildPhase()
        {
            ChallengesController.CheckSkyscraperChallenges();

            BuildController.Singleton.EndBuildPhase();

            // Check if the player even has placed blocks:
            // Otherwise, end the round prematurely.
            if (BuildController.PlacedBuildingBlockCopies > 0)
            {
                StartCoroutine("ActionPhase");
            }
            else
            {
                EndRound();
            }
        }

        private IEnumerator RoundCycle()
        {
            float time = currentRound.TimeRestraint;

            // Keep the round going as long as we're using a time restraint and time > 0 OR we're not using a time restraint (aka forever).
            while (time > 0)
            {
                if (currentRound.UseTimeRestraint)
                {
                    time -= Time.deltaTime;
                }

                BuildPhaseProgress = 1 - time / currentRound.TimeRestraint;

                yield return new WaitForEndOfFrame();
            }

            // In case anything updated roundHasEnded between frames because of framelag.
            if (!roundHasEnded)
            {
                EndBuildPhase();
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

                GameController.ResetStars();
            }

            UIRoundController.Singleton.UpdateStarCount();
            UIRoundController.Singleton.UpdateCurrentRound(roundsPassedWithoutLoss);
        }

        #endregion

        #region Challenge helpers

        private void ReadyChallengeControllers()
        {
            ProjectileController.Singleton.SetupProjectiles();
            VehicleController.Singleton.SetupVehicles();
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