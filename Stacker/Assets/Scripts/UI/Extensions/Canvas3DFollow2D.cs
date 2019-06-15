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
            if (target != null)
            {
                Vector3 pos = CameraController.CanvasCamera.ScreenToWorldPoint(target.position);
                pos.z = 0;
                transform.position = pos;
            }
        }

    }

}
