using System.Collections;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Controllers
{

    class MasterController : Controller<MasterController>
    {

        #region Editor

        [SerializeField] private MonoController[] preAwokenControllers;

        [Space(20)]
        [SerializeField] private MonoController[] lateStartControllers;

        #endregion

        public override void OnAwake()
        {
            foreach (MonoController mc in preAwokenControllers)
            {
                mc.Awake();
            }
        }

        private void Start()
        {
            StartCoroutine(LateStartCaller());
        }

        private IEnumerator LateStartCaller()
        {
            // Wait 10 frame after start:

            int frameCount = 0;

            while (frameCount < 10)
            {
                frameCount++;

                yield return new WaitForEndOfFrame();
            }

            foreach (MonoController mc in lateStartControllers)
            {
                mc.LateStart();
            }
        }

    }

}
