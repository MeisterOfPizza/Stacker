using Stacker.Rounds;
using Stacker.UIControllers;
using System.Collections;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Controllers
{

    class RoundController : Controller<RoundController>
    {

        #region Private variables

        private Round currentRound;

        private bool roundHasEnded;

        #endregion

        #region Public properties

        public Round CurrentRound
        {
            get
            {
                return currentRound;
            }
        }

        #endregion

        public Templates.Rounds.RoundTemplate roundTemplate;
        public override void OnAwake()
        {
            BeginRound(new Round(roundTemplate));
        }

        #region Round cycle

        public void BeginRound(Round round)
        {
            StopCoroutine("RoundCycle");
            StopCoroutine("ActionPhase");
            roundHasEnded = false;

            currentRound = round;

            ReadyChallengeControllers();
            BeginBuildPhase();
            StartCoroutine("RoundCycle");
        }

        private void BeginBuildPhase()
        {
            BuildController.Singleton.BeginBuildPhase(currentRound.Template.RoundBuildingBlockTemplates);
        }

        private IEnumerator RoundCycle()
        {
            float time = currentRound.TimeRestraint;

            // Keep the round going as long as we're using a time restraint and time > 0 OR we're not using a time restraint (aka forever).
            while (!currentRound.UseTimeRestraint || time > 0)
            {
                time -= Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
            
            // In case anything updated roundHasEnded between framelag.
            if (!roundHasEnded)
            {
                EndBuildPhase();

                StartCoroutine("ActionPhase");
            }
        }

        private IEnumerator ActionPhase()
        {
            yield return StartCoroutine(ProjectilePhase());
            yield return StartCoroutine(VehiclePhase());

            EndRound();
        }

        private void EndBuildPhase()
        {

        }

        public void EndRound()
        {
            StopCoroutine("RoundCycle");

            roundHasEnded = true;
        }

        #endregion

        #region Challenge helpers

        private void ReadyChallengeControllers()
        {
            ProjectileController.Singleton.SetupProjectiles();
            VehicleController.Singleton.SetupVehicles(currentRound.ProminentTunnelChallenge);
        }

        private IEnumerator ProjectilePhase()
        {
            yield return StartCoroutine(ProjectileController.Singleton.FireProjectiles());
        }

        private IEnumerator VehiclePhase()
        {
            yield return StartCoroutine(VehicleController.Singleton.LaunchVehicles());
        }

        #endregion

    }

}