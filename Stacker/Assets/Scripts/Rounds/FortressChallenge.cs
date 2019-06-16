using Stacker.Controllers;
using Stacker.Templates.Rounds;

namespace Stacker.Rounds
{

    class FortressChallenge : RoundChallenge
    {

        public int   Projectiles         { get; private set; }
        public float StructuralIntegrity { get; private set; }

        public override RoundChallengeType RoundChallengeType
        {
            get
            {
                return RoundChallengeType.Fortress;
            }
        }

        public FortressChallenge(int starsReward, string description, int projectiles, float structuralIntegrity) : base(starsReward, description)
        {
            this.Projectiles         = projectiles;
            this.StructuralIntegrity = structuralIntegrity;
        }

        public override bool CheckCompleted()
        {
            if (!IsCompleted)
            {
                IsCompleted = ChallengesController.ProjectilesFired >= Projectiles && StructuralIntegrity <= ChallengesController.StructuralIntegrity;

                if (IsCompleted)
                {
                    ChallengesController.PlayChallengeCompleteSoundEffect();
                }
            }

            return IsCompleted;
        }

    }

}
