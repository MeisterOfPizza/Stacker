﻿using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Templates.Rounds
{

    [CreateAssetMenu(menuName = "Templates/Round Template")]
    class RoundTemplate : ScriptableObject
    {

        #region Editor

        [Header("Building")]
        [SerializeField] private RoundBuildingBlockTemplate[] roundBuildingBlockTemplates;

        [Space]
        [SerializeField, Range(1f, 10f)] private float buildRadius      = 2f;
        [SerializeField]                 private float timeRestraint    = 15f;
        [SerializeField]                 private bool  useTimeRestraint = true;

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

    }

}
