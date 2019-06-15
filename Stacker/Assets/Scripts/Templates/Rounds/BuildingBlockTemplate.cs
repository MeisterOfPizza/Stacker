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
        [SerializeField] private GameObject icon3DPrefab;
        [SerializeField] private Vector3    scale       = Vector3.one;
        [SerializeField] private float      icon3DScale = 1f;

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

        public GameObject Icon3DPrefab
        {
            get
            {
                return icon3DPrefab;
            }
        }

        public Vector3 Scale
        {
            get
            {
                return scale;
            }
        }

        public float Icon3DScale
        {
            get
            {
                return icon3DScale;
            }
        }

        #endregion

    }

}
