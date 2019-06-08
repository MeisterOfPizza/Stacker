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

            TargetRotation = buildingBlock.transform.rotation;
        }

        private void FixedUpdate()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, Time.fixedDeltaTime * constructionBlockRotateSpeed);
        }

        #region Positioning

        private void FollowPointer()
        {
            Vector3 worldPosition;

#if UNITY_STANDALONE
            worldPosition = CameraController.MainCamera.ScreenToWorldPoint(Input.mousePosition);
#elif UNITY_IOS || UNITY_ANDROID
            worldPosition = CameraController.MainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
#endif

            BuildController.Singleton.MoveConstructionBuildingBlock(worldPosition);
        }

        #endregion

        #region Event systems

        private void OnMouseDrag()
        {
            FollowPointer();
        }

        private void OnMouseUpAsButton()
        {
            FollowPointer();

            BuildController.Singleton.PlaceBuildingBlock();
        }

        #endregion

    }

}
