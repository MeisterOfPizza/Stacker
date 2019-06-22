using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Templates.RoundSurprises
{

    enum RoundSurpriseType
    {
        BeforeBuildPhase,
        AfterBuildPhase
    }

    [CreateAssetMenu(menuName ="Templates/Round Surprise")]
    class RoundSurpriseTemplate : ScriptableObject
    {

        #region Editor

        [SerializeField] private new string        name;
        [SerializeField] private     GameObject    prefab;
        [SerializeField] private RoundSurpriseType roundSurpriseType;

        #endregion

        #region Getters

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public GameObject Prefab
        {
            get
            {
                return prefab;
            }
        }

        public RoundSurpriseType Type
        {
            get
            {
                return roundSurpriseType;
            }
        }

        #endregion

    }

}
