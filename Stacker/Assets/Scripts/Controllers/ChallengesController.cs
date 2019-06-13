using Stacker.Templates.Rounds;
using Stacker.UIControllers;
using System.Linq;

namespace Stacker.Controllers
{

    class ChallengesController : Controller<ChallengesController>
    {

        #region Public static properties
        
        public static bool  VehicleHitStructure    { get; set; }
        public static float BuildHeight            { get; set; }
        public static int   ProjectilesFired       { get; set; }
        public static int   BlocksHitByProjectiles { get; set; }

        /// <summary>
        /// What percentage of building block copies has NOT been hit?
        /// </summary>
        public static float StructuralIntegrity { get; private set; }

        #endregion

        public static void ResetChallengeValues()
        {
            VehicleHitStructure    = false;
            BuildHeight            = 0;
            ProjectilesFired       = 0;
            BlocksHitByProjectiles = 0;
        }

        private void FixedUpdate()
        {
            if (BuildController.CanBuild)
            {
                BuildHeight = BuildController.Singleton.CalculateStackHeight();
            }
        }

        public static void CheckSkyscraperChallenges()
        {
            var skyscraperChallenges = RoundController.Singleton.CurrentRound.RoundChallenges.Where(rc => rc.RoundChallengeType == RoundChallengeType.Skyscraper);

            foreach (var challenge in skyscraperChallenges)
            {
                challenge.CheckCompleted();
            }

            UIChallengesController.Singleton.UpdateUIChallenges();
        }

        public static void CheckFortressChallenges()
        {
            StructuralIntegrity = 1 - BlocksHitByProjectiles / (float)BuildController.PlacedBuildingBlockCopies;

            var fortressChallenges = RoundController.Singleton.CurrentRound.RoundChallenges.Where(rc => rc.RoundChallengeType == RoundChallengeType.Fortress);

            foreach (var challenge in fortressChallenges)
            {
                challenge.CheckCompleted();
            }

            UIChallengesController.Singleton.UpdateUIChallenges();
        }

        public static void CheckTunnelChallenges()
        {
            var tunnelChallenges = RoundController.Singleton.CurrentRound.RoundChallenges.Where(rc => rc.RoundChallengeType == RoundChallengeType.Tunnel);

            foreach (var challenge in tunnelChallenges)
            {
                challenge.CheckCompleted();
            }

            UIChallengesController.Singleton.UpdateUIChallenges();
        }

    }

}
