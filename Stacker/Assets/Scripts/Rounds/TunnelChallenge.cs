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

        public TunnelChallenge(int starsReward, string description, int vehicles) : base(starsReward, description)
        {
            this.Vehicles = vehicles;
        }

        public override bool CheckCompleted()
        {
            if (!IsCompleted)
            {
                IsCompleted = !ChallengesController.VehicleHitStructure;
            }

            return IsCompleted;
        }

    }

}
