using Stacker.Extensions.Attributes;
using Stacker.Rounds;
using System;
using UnityEngine;

namespace Stacker.Templates.Rounds
{
    
    [Flags]
    public enum RoundChallengeType
    {
        Skyscraper = 1,
        Fortress   = 2,
        Tunnel     = 4
    }

    enum TunnelChallengeVehiclePattern
    {
        Cross,
        TiltedCross,
        Highway,
        Oneway
    }

    [Serializable]
    public class RoundChallengeTemplate
    {

        #region Constants

        public const int ROUND_CHALLENGE_FORTRESS_MAX_PROJECTILES = 10;
        public const int ROUND_CHALLENGE_TUNNEL_MAX_VEHICLES      = 4;

        #endregion

        #region Editor

        [SerializeField]              private RoundChallengeType type        = RoundChallengeType.Skyscraper;
        [SerializeField, Range(1, 3)] private int                starsReward = 1;

        // Conditional editor variables //

        // Skyscraper //

        [Space]
        [SerializeField]
        [EnumShowField("type", (int)RoundChallengeType.Skyscraper)]
        private float buildHeight = 2.5f;

        // Fortress //

        [SerializeField]
        [EnumShowField("type", (int)RoundChallengeType.Fortress)]
        [Range(1, ROUND_CHALLENGE_FORTRESS_MAX_PROJECTILES)]
        private int projectiles = 1;

        [SerializeField]
        [EnumShowField("type", (int)RoundChallengeType.Fortress)]
        [Range(0.0f, 1.0f)]
        [Tooltip("How much of the structure should still be standing after the projectiles have been fired?")]
        private float structuralIntegrity = 0.5f;

        // Tunnel type //

        [SerializeField]
        [EnumShowField("type", (int)RoundChallengeType.Tunnel)]
        [Range(1, ROUND_CHALLENGE_TUNNEL_MAX_VEHICLES)]
        private int vehicles = 1;

        [SerializeField]
        [EnumShowField("type", (int)RoundChallengeType.Tunnel)]
        private TunnelChallengeVehiclePattern vehiclePattern = TunnelChallengeVehiclePattern.Cross;

        #endregion

        #region Helper methods

        internal RoundChallenge CreateChallenge()
        {
            switch (type)
            {
                case RoundChallengeType.Skyscraper:
                    return new SkyscraperChallenge(starsReward, string.Format("Build at least {0} meters high.", buildHeight), buildHeight);
                case RoundChallengeType.Fortress:
                    return new FortressChallenge(starsReward, string.Format("{0} of the structure should remain after {1} projectiles have been fired.", structuralIntegrity.ToString("P0"), projectiles), projectiles, structuralIntegrity);
                case RoundChallengeType.Tunnel:
                    return new TunnelChallenge(starsReward, string.Format("Do not let the {0} vehicles crash into the structure", vehicles), vehicles, vehiclePattern);
                default:
                    return null;
            }
        }

        #endregion

    }

}