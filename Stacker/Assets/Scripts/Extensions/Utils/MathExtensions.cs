using UnityEngine;

namespace Stacker.Extensions.Utils
{

    static class MathExtensions
    {

        #region Layer masks

        public static bool IsLayerInLayerMask(LayerMask layerMask, int layer)
        {
            // Refer to https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html for help and explanation.
            return (layerMask | (1 << layer)) == layerMask;
        }

        #endregion

    }

}
