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

        private const string UFO_BEGIN_FLIGHT_TRIGGER_NAME   = "Begin Flight";
        private const string UFO_BEGIN_FLIGHT_ANIMATION_NAME = "Begin Flight";
        private const string UFO_HOVER_BOOL_NAME             = "Hover";
        private const string UFO_HOVER_ANIMATION_NAME        = "Hover";

        #endregion

        #region Editor

        [Header("References")]
        [SerializeField] private GameObject ufo;
        [SerializeField] private Animator   animator;
        [SerializeField] private Transform  beamTransform;

        [Header("UFO Values")]
        [SerializeField] private float flySpeed          = 10f;
        [SerializeField] private float rotateSpeed       = 5f;
        [SerializeField] private float stackHeightOffset = 4f;
        [SerializeField] private float spawnRadius       = 100f;
        [SerializeField] private float escapeWaitTime    = 3f;

        #endregion

        #region Private variables

        private BuildingBlockCopy preselectedBuildingBlock;

        #endregion

        #region Overriden methods

        public override void SpawnRoundSurprise()
        {
            preselectedBuildingBlock = BuildController.PlacedBuildingBlockCopies[Random.Range(0, BuildController.NumberOfPlacedBuildingBlockCopies)];

            transform.position = GetPointOnSpawnCircle();
            ufo.SetActive(true);
        }

        public override IEnumerator StartRoundSurprise()
        {
            yield return StartCoroutine("FlyToTarget", GetBuildingBlockPosition(preselectedBuildingBlock)); // Fly to the build area.
            yield return StartCoroutine(BeamUpBuildingBlock()); // Pick up block.

            StartCoroutine("FlyToTarget", GetPointOnSpawnCircle()); // Fly away, but do not wait.
            yield return new WaitForSeconds(escapeWaitTime); // Instead, wait x amount of seconds before returning.
        }

        public override void RemoveRoundSurprise()
        {
            ufo.SetActive(false);
        }

        #endregion

        #region Controlling

        private IEnumerator FlyToTarget(Vector3 target)
        {
            animator.SetTrigger(UFO_BEGIN_FLIGHT_TRIGGER_NAME);

            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(1).IsName(UFO_BEGIN_FLIGHT_ANIMATION_NAME));

            float distance = Vector3.Distance(transform.position, target);

            while (distance > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, flySpeed * Time.deltaTime);
                distance = Vector3.Distance(transform.position, target);

                Vector3 direction  = target - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), rotateSpeed * Time.deltaTime);

                yield return new WaitForEndOfFrame();
            }

            animator.SetBool(UFO_HOVER_BOOL_NAME, true);
        }
        
        private IEnumerator BeamUpBuildingBlock()
        {
            animator.SetBool(UFO_HOVER_ANIMATION_NAME, true);

            yield return new WaitForSeconds(3f);

            animator.SetBool(UFO_HOVER_ANIMATION_NAME, false);

            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(1).IsName(UFO_HOVER_ANIMATION_NAME));
        }

        #endregion

        #region Helpers

        private Vector3 GetBuildingBlockPosition(BuildingBlockCopy buildingBlockCopy)
        {
            return new Vector3(buildingBlockCopy.transform.position.x, StackHeightController.CurrentBuildAltitude + stackHeightOffset, buildingBlockCopy.transform.position.z);
        }

        private Vector3 GetPointOnSpawnCircle()
        {
            float angle = Random.Range(0f, 360f);

            return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * spawnRadius, StackHeightController.CurrentBuildAltitude + stackHeightOffset, Mathf.Sin(angle * Mathf.Deg2Rad) * spawnRadius);
        }

        #endregion

    }

}
