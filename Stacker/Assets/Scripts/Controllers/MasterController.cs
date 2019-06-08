using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Controllers
{

    class MasterController : Controller<MasterController>
    {

        #region Editor

        [SerializeField] private MonoController[] preAwokenControllers;

        #endregion

        public override void OnAwake()
        {
            foreach (MonoController mc in preAwokenControllers)
            {
                mc.Awake();
            }
        }

    }

}
