using Stacker.Templates.Rounds;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Stacker.Rounds
{

    class Round
    {

        #region Properties

        public RoundTemplate Template { get; private set; }

        public float BuildRadius      { get; private set; }
        public float TimeRestraint    { get; private set; }
        public bool  UseTimeRestraint { get; private set; }
        
        public int             MaxProjectiles           { get; private set; }
        public TunnelChallenge ProminentTunnelChallenge { get; private set; }

        public RoundChallenge[] RoundChallenges { get; private set; }

        #endregion

        public Round(RoundTemplate roundTemplate)
        {
            this.Template = roundTemplate;

            this.BuildRadius      = roundTemplate.BuildRadius;
            this.TimeRestraint    = roundTemplate.TimeRestraint;
            this.UseTimeRestraint = roundTemplate.UseTimeRestraint;

            CreateChallenges();
            SetupChallengeValues();
        }

        private void CreateChallenges()
        {
            int challengesToCreate = Mathf.Min(3, Template.RoundChallengePool.Length);

            RoundChallenges = new RoundChallenge[challengesToCreate];

            List<RoundChallengeTemplate> roundChallengeTemplates = Template.RoundChallengePool.ToList();

            for (int i = 0; i < challengesToCreate; i++)
            {
                int index = Random.Range(0, roundChallengeTemplates.Count);

                RoundChallenges[i] = roundChallengeTemplates[index].CreateChallenge();
                roundChallengeTemplates.RemoveAt(index);
            }
        }

        private void SetupChallengeValues()
        {
            var fortressChallenges = RoundChallenges.Where(rc => rc.RoundChallengeType == RoundChallengeType.Fortress);
            var tunnelChallenges   = RoundChallenges.Where(rc => rc.RoundChallengeType == RoundChallengeType.Tunnel);

            MaxProjectiles = fortressChallenges.Count() > 0 ? fortressChallenges.Max(rc => ((FortressChallenge)rc).Projectiles) : 0;
            ProminentTunnelChallenge = RoundChallenges.Where(rc => rc.RoundChallengeType == RoundChallengeType.Tunnel).Cast<TunnelChallenge>().OrderBy(tc => tc.Vehicles).FirstOrDefault();
        }

        #region Helper methods

        /// <summary>
        /// Run a update check to see which challenges are completed.
        /// </summary>
        public void CheckChallenges()
        {
            foreach (var challenge in RoundChallenges)
            {
                challenge.CheckCompleted();
            }
        }

        /// <summary>
        /// Return the total amount of stars the player has earned after the round is finished.
        /// </summary>
        public int RoundStarReward()
        {
            return RoundChallenges.Where(rc => rc.IsCompleted).Sum(rc => rc.StarsReward);
        }

        #endregion

    }

}
