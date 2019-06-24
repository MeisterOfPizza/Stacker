using Stacker.Controllers;
using Stacker.Extensions.Utils;
using UnityEngine;

#pragma warning disable 0649
#pragma warning disable 0109

namespace Stacker.Building
{

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    class BuildingBlockCopy : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private     MeshRenderer meshRenderer;
        [SerializeField] private     Collider[]   colliders;
        [SerializeField] private new Rigidbody    rigidbody;

        [Header("Selection")]
        [SerializeField] private Material _selectedMaterial;

        #endregion

        #region Private variables

        private BuildingBlock buildingBlock;

        private BuildingBlockState state = BuildingBlockState.Inactive;

        private Material[] defaultMaterials;

        private bool isGrounded;

        #endregion

        #region Public properties

        public BuildingBlock BuildingBlock
        {
            get
            {
                return buildingBlock;
            }
        }

        public BuildingBlockState State
        {
            get
            {
                return state;
            }
        }

        public bool CanRotate
        {
            get
            {
                return buildingBlock.RoundBuildingBlockTemplate.CanRotate;
            }
        }

        public bool IsGrounded
        {
            get
            {
                return isGrounded;
            }
        }

        #endregion

        #region Enums

        internal enum BuildingBlockState
        {
            Inactive,
            Active,
            Selected
        }

        #endregion

        public void Initialize(BuildingBlock buildingBlock)
        {
            this.buildingBlock = buildingBlock;
        }

        public void BeamBuildingBlock()
        {
            BuildController.PlacedBuildingBlockCopies.Remove(this);

            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rigidbody.useGravity             = false;
            rigidbody.isKinematic            = true;

            foreach (var collider in colliders)
            {
                collider.enabled = false;
            }
        }

        #region MonoBehaviour methods

        private void Awake()
        {
            defaultMaterials = meshRenderer.materials;

            _selectedMaterial.SetFloat("_Flicker", 1f); // Disable hologram flicker.
        }

        private void OnEnable()
        {
            state = BuildingBlockState.Active;
        }

        private void OnDisable()
        {
            state = BuildingBlockState.Inactive;

            ChangeToDefaultMaterials();
        }

        #endregion

        #region Selection
        
        public void Select()
        {
            if (state != BuildingBlockState.Selected)
            {
                state = BuildingBlockState.Selected;

                ChangeToHologramMaterials();
            }
        }

        public void Deselect()
        {
            if (state == BuildingBlockState.Selected)
            {
                state = BuildingBlockState.Active;

                ChangeToDefaultMaterials();
            }
        }

        #endregion

        #region Positioning

        public void PlaceBuildingBlock(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
        }

        #endregion

        #region Physics

        private void OnCollisionEnter(Collision collision)
        {
            if (UtilExtensions.IsLayerInLayerMask(BuildController.BuildLayerMask, collision.gameObject.layer))
            {
                isGrounded = true;
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (!isGrounded && UtilExtensions.IsLayerInLayerMask(BuildController.BuildLayerMask, collision.gameObject.layer))
            {
                isGrounded = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (UtilExtensions.IsLayerInLayerMask(BuildController.BuildLayerMask, collision.gameObject.layer))
            {
                isGrounded = false;
            }
        }

        #endregion

        #region FX

        private void ChangeToDefaultMaterials()
        {
            meshRenderer.materials = defaultMaterials;
        }

        private void ChangeToHologramMaterials()
        {
            Material[] materials = meshRenderer.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = _selectedMaterial;
            }

            meshRenderer.materials = materials;
        }

        #endregion

        #region Events systems

        private void OnMouseDown()
        {
            if (isGrounded) // Prevent the player from selecting this block if it's not grounded.
            {
                BuildController.Singleton.SelectCopy(this);
            }
        }

        #endregion

    }

}
