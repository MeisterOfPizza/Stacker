using Stacker.Controllers;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.UI.Extensions
{

    public class Canvas3DFollow2D : MonoBehaviour
    {

        #region Editor

        [SerializeField] private Transform target;

        #endregion

        private void LateUpdate()
        {
            FollowTarget();
        }

        private void FollowTarget()
        {
            if (target != null)
            {
                Vector3 pos = CameraController.UI3DOverlayCamera.ScreenToWorldPoint(target.position);
                pos.z = 0;
                transform.position = pos;
            }
        }

        public void SetTarget(Transform target)
        {
            this.target = target;

            FollowTarget();
        }

    }

}
