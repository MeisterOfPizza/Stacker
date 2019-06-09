using Stacker.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Stacker.Components
{

    class ConstructionBuildingBlock : MonoBehaviour
    {

        #region Editor

        [SerializeField] private MeshFilter meshFilter;

        [Space]
        [SerializeField] private float constructionBlockRotateSpeed = 5f;

        #endregion

        #region Public properties

        public Quaternion TargetRotation { get; set; }

        #endregion

        public void Initialize(BuildingBlock buildingBlock)
        {
            meshFilter.mesh = buildingBlock.PrimaryMesh;

            TargetRotation = Quaternion.identity;
        }

        private void FixedUpdate()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, Time.fixedDeltaTime * constructionBlockRotateSpeed);
        }

    }

}
