using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Building
{

    class ConstructionBuildingBlock : MonoBehaviour
    {

        #region Editor

        [SerializeField] private MeshFilter meshFilter;

        [Space]
        [SerializeField] private float constructionBlockMoveSpeed   = 5f;
        [SerializeField] private float constructionBlockRotateSpeed = 5f;

        #endregion

        #region Public properties

        public Vector3    TargetPosition { get; set; }
        public Quaternion TargetRotation { get; set; }

        #endregion

        public void Initialize(Mesh mesh)
        {
            meshFilter.mesh = mesh;

            transform.rotation = Quaternion.identity;

            TargetPosition = transform.position;
            TargetRotation = Quaternion.identity;
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
