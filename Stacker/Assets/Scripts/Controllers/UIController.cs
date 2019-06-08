using UnityEngine;

namespace Stacker.Controllers
{

    class UIController : Controller<UIController>
    {

        #region Editor

        [Header("Anchors")]
        [SerializeField] private RectTransform uiBuildingBlockAnchor;

        [Header("Prefabs")]
        [SerializeField] private GameObject uiBuildingBlockPrefab;

        #endregion

    }

}
