using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Components
{

    class HologramEffect : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private MeshRenderer[] meshRenderers;

        [Header("Values")]
        [SerializeField, Range(0.01f, 1f)] private float flickerChance = 0.1f;
        [SerializeField, Range(0f, 1f)]    private float flickerAlpha  = 0.5f;

        [Header("Predefined values")]
        [SerializeField] private Shader _hologramShader;

        #endregion

        private void OnRenderObject()
        {
            float flickerValue = Random.Range(0, (int)(100 * (1 - flickerChance))) == 0 ? flickerAlpha : 1f;

            foreach (var meshRenderer in meshRenderers)
            {
                foreach (var material in meshRenderer.materials)
                {
                    if (material.shader == _hologramShader)
                    {
                        material.SetFloat("_Flicker", flickerValue);
                    }
                }
            }
        }

    }

}
