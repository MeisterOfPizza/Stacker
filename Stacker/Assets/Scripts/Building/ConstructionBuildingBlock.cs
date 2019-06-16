using Stacker.Templates.Rounds;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Building
{

    class ConstructionBuildingBlock : MonoBehaviour
    {

        #region Editor

        [SerializeField] private MeshFilter   meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private MeshCollider meshCollider;

        [Space]
        [SerializeField] private float constructionBlockMoveSpeed   = 5f;
        [SerializeField] private float constructionBlockRotateSpeed = 5f;

        [Header("Predefined editor values")]
        [SerializeField] private Material _hologramMaterial;

        #endregion

        #region Public properties
        
        public Vector3 Position
        {
            get
            {
                return transform.position;
            }
        }

        public Vector3    TargetPosition { get; set; }
        public Quaternion TargetRotation { get; set; }

        #endregion

        public void Initialize(BuildingBlockTemplate buildingBlockTemplate)
        {
            transform.localScale    = buildingBlockTemplate.Scale;
            meshFilter.mesh         = buildingBlockTemplate.Mesh;
            meshCollider.sharedMesh = buildingBlockTemplate.Mesh;

            SetupMaterials();

            transform.rotation = Quaternion.identity;

            TargetPosition = transform.position;
            TargetRotation = Quaternion.identity;
        }

        private void SetupMaterials()
        {
            Material[] materials = new Material[meshFilter.mesh.subMeshCount];

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = _hologramMaterial;
            }

            meshRenderer.materials = materials;
        }

        private void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.fixedDeltaTime * constructionBlockMoveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, Time.fixedDeltaTime * constructionBlockRotateSpeed);
        }

        public void SetConstructionBuildingBlockActive(bool active)
        {
            transform.parent.gameObject.SetActive(active);
        }

    }

}
