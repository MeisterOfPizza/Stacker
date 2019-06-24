using Stacker.Building;
using Stacker.Controllers;
using System.Collections;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.RoundSurprises
{

    class RoundSurpriseUFO : RoundSurprise
    {

        #region Private constants

        private const string FLY_TO_PLAYER_TRIGGER_NAME   = "Fly To Player";
        private const string FLY_FROM_PLAYER_TRIGGER_NAME = "Fly From Player";

        #endregion

        #region Editor

        [Header("References")]
        [SerializeField] private Animator     animator;
        [SerializeField] private Transform    ufoTransform;
        [SerializeField] private Transform    beamTransform;
        [SerializeField] private MeshRenderer beamMeshRenderer;

        [Header("UFO Values")]
        [SerializeField] private float stackHeightOffset = 4f;
        [SerializeField] private float escapeWaitTime    = 3f;

        [Header("Audio")]
        [SerializeField] private AudioSource ufoAudioSource;
        [SerializeField] private AudioSource beamAudioSource;

        [Space]
        [SerializeField] private AudioClip[] ufoFlyingSoundEffects;
        [SerializeField] private AudioClip[] beamSoundEffects;

        #endregion

        #region Hidden public variables

        [HideInInspector] public bool isReadyForBeam;

        #endregion

        #region Private variables

        private BuildingBlockCopy preselectedBuildingBlock;

        private float currentFlyAltitude;

        private bool isFlying;

        #endregion

        #region MonoBehaviour methods

        private void Awake()
        {
            transform.SetParent(RoundSurpriseController.Singleton.UFOOffset);
        }

        #endregion

        #region Overriden methods

        public override void SpawnRoundSurprise()
        {
            preselectedBuildingBlock = BuildController.PlacedBuildingBlockCopies[Random.Range(0, BuildController.NumberOfPlacedBuildingBlockCopies)];

            currentFlyAltitude = CalculateFlyAltitude();
            ufoTransform.position  = new Vector3(ufoTransform.position.x, currentFlyAltitude, ufoTransform.position.z);
            beamTransform.position = new Vector3(beamTransform.position.x, currentFlyAltitude, beamTransform.position.z);

            RoundSurpriseController.Singleton.UFOOffset.position = new Vector3(preselectedBuildingBlock.transform.position.x, 0, preselectedBuildingBlock.transform.position.z);
        }

        public override IEnumerator StartRoundSurprise()
        {
            animator.SetTrigger(FLY_TO_PLAYER_TRIGGER_NAME); // Fly to the build area.
            isFlying = true;
            StartCoroutine(LoopUfoFlyingSoundEffects());

            yield return new WaitUntil(() => isReadyForBeam); // Wait until the hover state is in. (isReadyForBeam is set to true in that animation clip)

            isFlying = false;
            yield return StartCoroutine(BeamUpBuildingBlock()); // Pick up block.

            animator.SetTrigger(FLY_FROM_PLAYER_TRIGGER_NAME); // Fly away, but do not wait.
            isFlying = true;
            StartCoroutine(LoopUfoFlyingSoundEffects());

            yield return new WaitForSeconds(escapeWaitTime); // Instead, wait x amount of seconds before returning.
        }

        public override void RemoveRoundSurprise()
        {
            isFlying = false;
        }

        #endregion

        #region Coroutines

        private IEnumerator BeamUpBuildingBlock()
        {
            beamTransform.gameObject.SetActive(true);

            // Setup beam shader values:
            beamTransform.localScale = new Vector3(beamTransform.localScale.x, CalculateBeamScale(currentFlyAltitude), beamTransform.localScale.z);
            beamMeshRenderer.material.SetFloat("_EdgeThickness", CalculateBeamEdgeThickness(currentFlyAltitude));

            // Play beam sound effect:
            beamAudioSource.PlayOneShot(beamSoundEffects[Random.Range(0, beamSoundEffects.Length)], AudioController.EffectsVolume * 0.1f);

            // Show beam and play the shader effect:
            float beamVerticalCutout = 1f;

            while (beamVerticalCutout > 0f)
            {
                beamVerticalCutout -= Time.deltaTime;

                beamMeshRenderer.material.SetFloat("_VerticalCutout", beamVerticalCutout);

                yield return new WaitForEndOfFrame();
            }

            // Beam up building block:
            preselectedBuildingBlock.BeamBuildingBlock();

            while (preselectedBuildingBlock.transform.position.y < currentFlyAltitude - 0.1f)
            {
                preselectedBuildingBlock.transform.position = Vector3.MoveTowards(preselectedBuildingBlock.transform.position, ufoTransform.position, 5f * Time.deltaTime);

                yield return new WaitForEndOfFrame();
            }

            preselectedBuildingBlock.gameObject.SetActive(false);

            // Remove beam:
            while (beamVerticalCutout < 1f)
            {
                beamVerticalCutout += Time.deltaTime;

                beamMeshRenderer.material.SetFloat("_VerticalCutout", beamVerticalCutout);

                yield return new WaitForEndOfFrame();
            }

            beamTransform.gameObject.SetActive(false);
        }

        #endregion

        #region Audio

        private IEnumerator LoopUfoFlyingSoundEffects()
        {
            while (isFlying)
            {
                if (!ufoAudioSource.isPlaying && ufoAudioSource.gameObject.activeInHierarchy)
                {
                    ufoAudioSource.clip   = ufoFlyingSoundEffects[Random.Range(0, ufoFlyingSoundEffects.Length)];
                    ufoAudioSource.volume = AudioController.EffectsVolume * 0.025f;
                    ufoAudioSource.Play();
                }

                yield return new WaitForEndOfFrame();
            }
        }

        #endregion

        #region Helpers

        private float CalculateBeamScale(float altitude)
        {
            return altitude / 4f;
        }

        private float CalculateBeamEdgeThickness(float altitude)
        {
            return altitude > 0 ? 0.4f / altitude : 0f;
        }

        private float CalculateFlyAltitude()
        {
            return StackHeightController.CurrentBuildAltitude + stackHeightOffset;
        }

        #endregion

    }

}
