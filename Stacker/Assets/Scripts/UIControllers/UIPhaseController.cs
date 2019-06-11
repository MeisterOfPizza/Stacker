using Stacker.Controllers;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649

namespace Stacker.UIControllers
{

    class UIPhaseController : Controller<UIPhaseController>
    {

        #region Editor

        [Header("UI Phase Indicator")]
        [SerializeField] private Image phaseIndicatorImage;

        [Space]
        [SerializeField] private float phaseIndicatorMoveSpeed = 5;

        [Header("UI Phases")]
        [SerializeField] private Animator buildPhaseAnimator;
        [SerializeField] private Animator fortressPhaseAnimator;
        [SerializeField] private Animator tunnelPhaseAnimator;

        #endregion

        #region Private variables

        private Phase currentPhase        = Phase.Build;
        private float startPhaseProgress  = 0;
        private float targetPhaseProgress = 0;

        #endregion

        #region Enums

        private enum Phase
        {
            Build,
            Fortress,
            Tunnel
        }

        #endregion

        #region MonoBehaviour methods

        private void Update()
        {
            UpdatePhaseIndicator();
        }

        #endregion

        #region Phase management

        public void BeginPhases()
        {
            currentPhase        = Phase.Build;
            GetPhaseAnimator(currentPhase)?.SetTrigger("Scale Up");
            startPhaseProgress  = 0;
            targetPhaseProgress = GetTargetPhaseProgress(currentPhase);
        }

        public void NextPhase()
        {
            Animator current = GetPhaseAnimator(currentPhase);
            Animator next    = GetPhaseAnimator(currentPhase + 1);

            startPhaseProgress  = targetPhaseProgress;
            targetPhaseProgress = GetTargetPhaseProgress(currentPhase + 1);
            
            current?.SetTrigger("Scale Down");
            next?.SetTrigger("Scale Up");

            if (currentPhase != Phase.Tunnel)
            {
                currentPhase++;
            }
        }

        public void EndPhases()
        {
            GetPhaseAnimator(currentPhase)?.SetTrigger("Scale Down");
            startPhaseProgress  = 1;
            targetPhaseProgress = 1;
        }

        #endregion

        #region Phase indicator

        private void UpdatePhaseIndicator()
        {
            if (currentPhase == Phase.Build)
            {
                phaseIndicatorImage.fillAmount = Mathf.Lerp(phaseIndicatorImage.fillAmount, startPhaseProgress + (targetPhaseProgress - startPhaseProgress) * RoundController.Singleton.BuildPhaseProgress, Time.deltaTime * phaseIndicatorMoveSpeed);
            }
            else
            {
                phaseIndicatorImage.fillAmount = startPhaseProgress;
            }
        }

        #endregion

        #region Helper methods

        private Animator GetPhaseAnimator(Phase phase)
        {
            switch (phase)
            {
                case Phase.Build:
                    return buildPhaseAnimator;
                case Phase.Fortress:
                    return fortressPhaseAnimator;
                case Phase.Tunnel:
                    return tunnelPhaseAnimator;
                default:
                    return null;
            }
        }

        private float GetTargetPhaseProgress(Phase phase)
        {
            switch (phase)
            {
                case Phase.Build:
                    return 0.5f;
                case Phase.Fortress:
                    return 0.75f;
                case Phase.Tunnel:
                    return 1f;
                default:
                    return 0f;
            }
        }

        #endregion

    }

}
