using UnityEngine;

namespace Stacker.Templates.Rounds
{

    [CreateAssetMenu(menuName = "Templates/Round Template")]
    class RoundTemplate : ScriptableObject
    {

        #region Editor

        [Header("Building")]
        [SerializeField] private RoundBuildingBlock[] roundBuildingBlocks;

        [Space]
        [SerializeField, Range(0.5f, 10f)] private float buildRadius      = 2f;
        [SerializeField]                   private float timeRestraint    = 15f;
        [SerializeField]                   private bool  useTimeRestraint = true;

        [Header("Challenges")]
        [SerializeField] private RoundChallengeTemplate[] roundChallengePool;

        #endregion

        #region Getters

        public RoundBuildingBlock[] RoundBuildingBlocks
        {
            get
            {
                return roundBuildingBlocks;
            }
        }

        public float BuildRadius
        {
            get
            {
                return buildRadius;
            }
        }

        public float TimeRestraint
        {
            get
            {
                return timeRestraint;
            }
        }

        public bool UseTimeRestraint
        {
            get
            {
                return useTimeRestraint;
            }
        }

        public RoundChallengeTemplate[] RoundChallengePool
        {
            get
            {
                return roundChallengePool;
            }
        }

        #endregion

    }

}
