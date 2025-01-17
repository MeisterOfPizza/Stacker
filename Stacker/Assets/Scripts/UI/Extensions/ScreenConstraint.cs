﻿using Stacker.Extensions.Utils;
using UnityEngine;

namespace Stacker.UI.Extensions
{

    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class ScreenConstraint : MonoBehaviour, IScreenResizeListener
    {

        private RectTransform rectTransform;
        private Vector2       screenSize;
        private Vector2       xAxisConstraint;
        private Vector2       yAxisConstraint;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            screenSize    = ScreenResizer.ScreenSize;

            UpdateConstraints();
        }

        private void OnRectTransformDimensionsChange()
        {
            if (rectTransform != null)
            {
                UpdateConstraints();
            }
        }

        private void UpdateConstraints()
        {
            Vector2 halfSize = rectTransform.sizeDelta / 2f;

            xAxisConstraint = new Vector2(halfSize.x, screenSize.x - halfSize.x);
            yAxisConstraint = new Vector2(halfSize.y, screenSize.y - halfSize.y);
        }

        private void LateUpdate()
        {
            float x = Mathf.Clamp(transform.position.x, xAxisConstraint.x, xAxisConstraint.y);
            float y = Mathf.Clamp(transform.position.y, yAxisConstraint.x, yAxisConstraint.y);

            rectTransform.position = new Vector3(x, y, rectTransform.position.z);
        }

        #region IScreenResizeListener

        public void ScreenResized(Vector2 size)
        {
            screenSize = size;

            UpdateConstraints();
        }

        #endregion

    }

}
