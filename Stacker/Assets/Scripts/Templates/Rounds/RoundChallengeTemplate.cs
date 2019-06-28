using Stacker.Extensions.Attributes;
using Stacker.Rounds;
using System;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Templates.Rounds
{

    [Flags]
    public enum RoundChallengeType
    {
        Skyscraper = 1,
        Fortress   = 2,
        Tunnel     = 4
    }

    [Serializable]
    public class RoundChallengeTemplate
    {

        #region Constants

        public const int ROUND_CHALLENGE_FORTRESS_MAX_PROJECTILES = 10;
        public const int ROUND_CHALLENGE_TUNNEL_MAX_VEHICLES      = 4;

        #endregion

        #region Editor

        [SerializeField] private string customName;

        [Space]
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
        [Tooltip("How much of the structure should still be standing after the projectiles have been fired?")]
        [Range(0.0f, 1.0f)]
        private float structuralIntegrity = 0.5f;

        // Tunnel type //

        [SerializeField]
        [EnumShowField("type", (int)RoundChallengeType.Tunnel)]
        [Range(1, ROUND_CHALLENGE_TUNNEL_MAX_VEHICLES)]
        private int vehicles = 1;

        #endregion

        #region Helper methods

        internal RoundChallenge CreateChallenge()
        {
            switch (type)
            {
                case RoundChallengeType.Skyscraper:
                    return new SkyscraperChallenge(customName.Length > 0 ? customName : type.ToString(), starsReward, string.Format("Build at least {0} meter{1} high.", buildHeight, buildHeight != 1 ? "s" : ""), buildHeight);
                case RoundChallengeType.Fortress:
                    return new FortressChallenge(customName.Length > 0 ? customName : type.ToString(), starsReward, string.Format("{0} of the structure should remain after {1} projectile{2} has been fired.", structuralIntegrity.ToString("P0"), projectiles, projectiles > 1 ? "s" : ""), projectiles, structuralIntegrity);
                case RoundChallengeType.Tunnel:
                    return new TunnelChallenge(customName.Length > 0 ? customName : type.ToString(), starsReward, string.Format("Do not let the {0} vehicle{1} crash into the structure.", vehicles, vehicles > 1 ? "s" : ""), vehicles);
                default:
                    return null;
            }
        }

        #endregion

    }

}