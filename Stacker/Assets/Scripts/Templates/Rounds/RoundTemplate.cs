using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Templates.Rounds
{

    [CreateAssetMenu(menuName = "Templates/Round Template")]
    class RoundTemplate : ScriptableObject
    {

        #region Public constants

        public const float MAX_BUILD_RADIUS = 5f;

        #endregion

        #region Editor

        [Header("General")]
        [SerializeField, Tooltip("Minimum amount of stars needed to make this round appear.")]
        private int minStarsToAppear = 0;
        [SerializeField, Tooltip("Maximum amount of stars the player can have before this round cannot appear anymore.")]
        private int maxStarsToAppear = 10000;

        [Header("Building")]
        [SerializeField] private RoundBuildingBlockTemplate[] roundBuildingBlockTemplates;

        [Space]
        [SerializeField, Range(1f, MAX_BUILD_RADIUS)] private float buildRadius      = 2.5f;
        [SerializeField]                              private float timeRestraint    = 15f;
        [SerializeField]                              private bool  useTimeRestraint = true;

        [Header("Challenges")]
        [SerializeField] private RoundChallengeTemplate[] roundChallengePool;

        #endregion

        #region Getters

        public RoundBuildingBlockTemplate[] RoundBuildingBlockTemplates
        {
            get
            {
                return roundBuildingBlockTemplates;
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

        #region Helper methods

        public bool RoundCanAppear(int currentStars)
        {
            return currentStars >= minStarsToAppear && currentStars <= maxStarsToAppear;
        }

        #endregion

    }

}
