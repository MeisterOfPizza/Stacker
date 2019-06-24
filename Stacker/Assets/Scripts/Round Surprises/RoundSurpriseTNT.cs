using Stacker.Controllers;
using Stacker.Extensions.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649
#pragma warning disable 0109

namespace Stacker.RoundSurprises
{

    class RoundSurpriseTNT : RoundSurprise
    {

        #region Editor

        [Header("Referencs")]
        [SerializeField] private     GameObject     tntCrate;
        [SerializeField] private new Collider       collider;
        [SerializeField] private     MeshRenderer[] meshRenderers;
        [SerializeField] private     ParticleSystem explosionEffect;
        [SerializeField] private     ParticleSystem fuseLitEffect;
        [SerializeField] private     AudioSource    soundEffectsSource;

        [Header("TNT Values")]
        [SerializeField] private float     explosionCountdown = 5f;
        [SerializeField] private float     explosionForce     = 10f;
        [SerializeField] private float     explosionRadius    = 1f;
        [SerializeField] private LayerMask buildingBlockLayerMask;

        [Header("FX Values")]
        [SerializeField] private float dissolveSpeed = 1f;

        [Header("Audio")]
        [SerializeField] private AudioClip[] explosionSoundEffects;
        [SerializeField] private AudioClip   fuseLitHissingSoundEffect;

        #endregion

        #region Private variables

        private bool canDetectBuildingBlocks;
        private bool hasHitBuildingBlock;

        // Dissolving //

        private Material[] fxMaterials;

        private bool isDissolving;

        #endregion

        #region MonoBehaviour methods

        private void Awake()
        {
            List<Material> materials = new List<Material>();

            foreach (var mr in meshRenderers)
            {
                materials.AddRange(mr.materials);
            }

            fxMaterials = materials.ToArray();
        }

        #endregion

        #region Overriden methods

        public override void SpawnRoundSurprise()
        {
            hasHitBuildingBlock = false;

            float buildRadius = RoundController.Singleton.CurrentRound.BuildRadius - 1;
            transform.position = new Vector3(Random.Range(-buildRadius, buildRadius), 0, Random.Range(-buildRadius, buildRadius));

            StopCoroutine("DissolveOut");
            StartCoroutine("DissolveIn");
        }

        public override IEnumerator StartRoundSurprise()
        {
            canDetectBuildingBlocks = true;

            yield return null; // We do not need to wait for anything.
        }

        public override void RemoveRoundSurprise()
        {
            explosionEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            fuseLitEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            canDetectBuildingBlocks = false;

            StopCoroutine("DissolveIn");
            StartCoroutine("DissolveOut");
        }

        #endregion

        #region Physics

        private void OnCollisionEnter(Collision collision)
        {
            if (canDetectBuildingBlocks && !hasHitBuildingBlock && UtilExtensions.IsLayerInLayerMask(buildingBlockLayerMask, collision.gameObject.layer))
            {
                hasHitBuildingBlock = true;

                StartCoroutine("LightFuse");
            }
        }

        #endregion

        #region FX

        private IEnumerator DissolveIn()
        {
            tntCrate.SetActive(true);
            collider.enabled = true;

            // Go from dissolve amount 1 to 0.
            float dissolveAmount = 1f;
            isDissolving = true;

            while (isDissolving && dissolveAmount > 0f)
            {
                dissolveAmount = Mathf.MoveTowards(dissolveAmount, 0, dissolveSpeed * Time.deltaTime);

                foreach (var mat in fxMaterials)
                {
                    mat.SetFloat("_DissolveAmount", dissolveAmount);
                }

                yield return new WaitForEndOfFrame();
            }

            foreach (var mat in fxMaterials)
            {
                mat.SetFloat("_DissolveAmount", 0);
            }

            isDissolving = false;
        }

        private IEnumerator DissolveOut()
        {
            collider.enabled = false;

            // Go from dissolve amount 0 to 1.
            float dissolveAmount = 0f;
            isDissolving = true;

            while (isDissolving && dissolveAmount < 1f)
            {
                dissolveAmount = Mathf.MoveTowards(dissolveAmount, 1, dissolveSpeed * Time.deltaTime);

                foreach (var mat in fxMaterials)
                {
                    mat.SetFloat("_DissolveAmount", dissolveAmount);
                }

                yield return new WaitForEndOfFrame();
            }

            foreach (var mat in fxMaterials)
            {
                mat.SetFloat("_DissolveAmount", 1);
            }

            isDissolving = false;

            tntCrate.SetActive(false);
        }

        private IEnumerator LightFuse()
        {
            float countdown = explosionCountdown;
            fuseLitEffect.Play(true);
            soundEffectsSource.PlayOneShot(fuseLitHissingSoundEffect, AudioController.EffectsVolume * 0.25f);
            float soundEffectCooldown = GetLightFuseSoundEffectCooldown(countdown);

            while (countdown > 0 && !isDissolving)
            {
                countdown           -= Time.deltaTime;
                soundEffectCooldown -= Time.deltaTime;

                if (soundEffectCooldown <= 0f)
                {
                    soundEffectsSource.PlayOneShot(fuseLitHissingSoundEffect, AudioController.EffectsVolume * 0.25f);
                    soundEffectCooldown = GetLightFuseSoundEffectCooldown(countdown);
                }

                yield return new WaitForEndOfFrame();
            }

            if (!isDissolving)
            {
                foreach (var collider in Physics.OverlapSphere(transform.position, explosionRadius, buildingBlockLayerMask, QueryTriggerInteraction.Ignore))
                {
                    collider.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 2.5f, ForceMode.Force);
                }

                soundEffectsSource.PlayOneShot(explosionSoundEffects[Random.Range(0, explosionSoundEffects.Length)], AudioController.EffectsVolume);
                explosionEffect.Play(true);
                collider.enabled = false;
                tntCrate.SetActive(false);
            }
        }

        private float GetLightFuseSoundEffectCooldown(float countdown) => countdown * 0.2f;

        #endregion

    }

}
