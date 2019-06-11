using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Templates.Rounds
{

    [CreateAssetMenu(menuName = "Templates/Building Block Template")]
    class BuildingBlockTemplate : ScriptableObject
    {

        #region Editor

        [SerializeField] private GameObject prefab;
        [SerializeField] private Mesh       mesh;

        #endregion

        #region Getters

        public GameObject Prefab
        {
            get
            {
                return prefab;
            }
        }

        public Mesh Mesh
        {
            get
            {
                return mesh;
            }
        }

        #endregion

    }

}
