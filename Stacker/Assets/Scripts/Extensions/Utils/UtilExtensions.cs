using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Stacker.Extensions.Utils
{

    static class UtilExtensions
    {

        #region Transforms

        /// <summary>
        /// Removes all children of this parent.
        /// </summary>
        public static void Clear(this Transform parent)
        {
            foreach (Transform child in parent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        #endregion

        #region Vector3s

        /// <summary>
        /// Traps the position within a box with two corners, one in min and the other in max.
        /// </summary>
        public static Vector3 TrapInBox(this Vector3 position, Vector3 min, Vector3 max)
        {
            return new Vector3(Mathf.Clamp(position.x, min.x, max.x), Mathf.Clamp(position.y, min.y, max.y), Mathf.Clamp(position.z, min.z, max.z));
        }

        #endregion

        #region Event systems

        /// <summary>
        /// Raycasts everything under the given position in UI space.
        /// </summary>
        public static List<RaycastResult> UIRaycastResults(Vector2 position)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1
            };

            pointerData.position = position;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            return results;
        }

        /// <summary>
        /// Raycasts everything under the given position in UI space.
        /// Removes any results that does not have a tag equal to <paramref name="tagFilter"/>.
        /// </summary>
        public static List<RaycastResult> UIRaycastResults(Vector2 position, string tagFilter)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1
            };

            pointerData.position = position;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            // Remove objects based on tag:
            foreach (var result in results.ToList())
            {
                if (!result.gameObject.tag.Equals(tagFilter))
                {
                    results.Remove(result);
                }
            }

            return results;
        }

        #endregion

        #region Layer masks

        public static bool IsLayerInLayerMask(LayerMask layerMask, int layer)
        {
            // Refer to https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html for help and explanation.
            return (layerMask | (1 << layer)) == layerMask;
        }

        #endregion

    }

}
