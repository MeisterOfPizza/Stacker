using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#pragma warning disable 0649

namespace Stacker.UI.Extensions
{

    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    class DraggableRect : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        #region Wrapper class

        // To allow for editor interfacing with UnityEvents with parameter(s).
        [Serializable] private class DragEvent : UnityEvent<Vector2> { }

        #endregion

        #region Editor

        [SerializeField] private DragEvent onBeginDrag;
        [SerializeField] private DragEvent onEndDrag;

        #endregion

        #region Private variables

        private RectTransform rectTransform;
        private Vector2       restPosition;

        #endregion

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            restPosition  = rectTransform.anchoredPosition;
        }

        #region Event systems

        public void OnBeginDrag(PointerEventData eventData)
        {
            onBeginDrag.Invoke(rectTransform.anchoredPosition);
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition = rectTransform.parent.InverseTransformPoint(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            onEndDrag.Invoke(eventData.position);

            rectTransform.anchoredPosition = restPosition;
        }

        #endregion

    }

}
