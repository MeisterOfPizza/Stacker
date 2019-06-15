using Stacker.UIControllers;
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

        [Header("Events")]
        [SerializeField] private DragEvent onBeginDrag;
        [SerializeField] private DragEvent onEndDrag;

        #endregion

        #region Private variables

        private Transform     parent;
        private int           childIndex;
        private RectTransform rectTransform;
        private Vector2       restPosition;

        #endregion

        #region Public properties

        public RectTransform DetachedParent { get; set; }

        #endregion

        private void Awake()
        {
            parent        = transform.parent;
            childIndex    = transform.GetSiblingIndex();
            rectTransform = GetComponent<RectTransform>();
            restPosition  = rectTransform.anchoredPosition;
        }

        #region Event systems

        public void OnBeginDrag(PointerEventData eventData)
        {
            onBeginDrag.Invoke(rectTransform.anchoredPosition);
            
            transform.SetParent(DetachedParent);
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            onEndDrag.Invoke(eventData.position);

            transform.SetParent(parent);
            transform.SetSiblingIndex(childIndex);
            rectTransform.anchoredPosition = restPosition;
        }

        #endregion

    }

}
