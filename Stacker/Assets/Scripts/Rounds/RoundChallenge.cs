using Stacker.Templates.Rounds;

namespace Stacker.Rounds
{

    abstract class RoundChallenge
    {

        public string Name        { get; private set; }
        public int    StarsReward { get; private set; }
        public string Description { get; private set; }

        public bool IsCompleted { get; protected set; }

        public abstract RoundChallengeType RoundChallengeType { get; }

        public RoundChallenge(string name, int starsReward, string description)
        {
            this.Name        = name;
            this.StarsReward = starsReward;
            this.Description = description;
        }

        public abstract bool CheckCompleted();

    }

}
