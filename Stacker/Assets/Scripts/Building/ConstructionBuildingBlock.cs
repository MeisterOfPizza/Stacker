using Stacker.Controllers;
using Stacker.Extensions.Utils;
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
        [SerializeField] private LineRenderer   landLine;
        [SerializeField] private SpriteRenderer landPoint;

        [Space]
        [SerializeField] private float constructionBlockMoveSpeed   = 10f;
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

        private void Awake()
        {
            _hologramMaterial.SetFloat("_Flicker", 1f); // Disable hologram flicker.
        }

        public void Initialize(BuildingBlockTemplate buildingBlockTemplate)
        {
            transform.localScale    = buildingBlockTemplate.Scale;
            meshFilter.mesh         = buildingBlockTemplate.Mesh;
            meshCollider.sharedMesh = buildingBlockTemplate.Mesh;

            SetupMaterials();

            transform.position = new Vector3(0, 5, 0);
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
            
            UpdateFX();
        }

        private void UpdateFX()
        {
            // Cast a ray down to determine where the block would land if the player dropped it:
            Ray groundRay = new Ray(transform.position, Vector3.down);
            Physics.Raycast(groundRay, out RaycastHit groundHit, 100f, BuildController.BuildLayerMask, QueryTriggerInteraction.Ignore);

            // Update the land line and land point sprite:
            landLine.SetPosition(0, transform.position);
            landLine.SetPosition(1, groundHit.point);
            landPoint.transform.position = groundHit.point + Vector3.up * 0.025f;
        }

        public void Teleport(Vector3 position)
        {
            transform.position = position;

            UpdateFX();
        }

    }

}
