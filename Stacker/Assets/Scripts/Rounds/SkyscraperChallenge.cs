using Stacker.Templates.Rounds;

namespace Stacker.Rounds
{

    class SkyscraperChallenge : RoundChallenge
    {

        public float BuildHeight { get; private set; }

        public override RoundChallengeType RoundChallengeType
        {
            get
            {
                return RoundChallengeType.Skyscraper;
            }
        }

        public SkyscraperChallenge(int starsReward, string description, float buildHeight) : base(starsReward, description)
        {
            this.BuildHeight = buildHeight;
        }

        public override bool CheckCompleted()
        {
            throw new System.NotImplementedException();
        }

    }

}
