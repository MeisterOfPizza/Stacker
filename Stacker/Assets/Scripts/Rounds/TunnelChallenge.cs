using Stacker.Templates.Rounds;

namespace Stacker.Rounds
{

    class TunnelChallenge : RoundChallenge
    {

        public int                           Vehicles       { get; private set; }
        public TunnelChallengeVehiclePattern VehiclePattern { get; private set; }

        public override RoundChallengeType RoundChallengeType
        {
            get
            {
                return RoundChallengeType.Tunnel;
            }
        }

        public TunnelChallenge(int starsReward, string description, int vehicles, TunnelChallengeVehiclePattern vehiclePattern) : base(starsReward, description)
        {
            this.Vehicles       = vehicles;
            this.VehiclePattern = vehiclePattern;
        }

        public override bool CheckCompleted()
        {
            throw new System.NotImplementedException();
        }

    }

}
