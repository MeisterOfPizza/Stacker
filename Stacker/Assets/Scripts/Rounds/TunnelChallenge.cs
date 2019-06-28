using Stacker.Controllers;
using Stacker.Templates.Rounds;

namespace Stacker.Rounds
{

    class TunnelChallenge : RoundChallenge
    {

        public int Vehicles       { get; private set; }

        public override RoundChallengeType RoundChallengeType
        {
            get
            {
                return RoundChallengeType.Tunnel;
            }
        }

        public TunnelChallenge(string name, int starsReward, string description, int vehicles) : base(name, starsReward, description)
        {
            this.Vehicles = vehicles;
        }

        public override bool CheckCompleted()
        {
            if (!IsCompleted)
            {
                IsCompleted = !ChallengesController.VehicleHitStructure;

                if (IsCompleted)
                {
                    ChallengesController.PlayChallengeCompleteSoundEffect();
                }
            }

            return IsCompleted;
        }

    }

}
