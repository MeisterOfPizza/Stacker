using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Components
{

    [DisallowMultipleComponent]
    class Spin : MonoBehaviour
    {

        #region Editor

        [Header("Custom spin")]
        [SerializeField] private Vector3 customSpin = Vector3.one;
        [SerializeField] private bool    useCustomSpin;

        [Header("Auto spin")]
        [SerializeField] private float         minRandomSpinSpeed = 0.5f;
        [SerializeField] private float         maxRandomSpinSpeed = 1.5f;
        [SerializeField] private SpinDirection spinDirection      = SpinDirection.Random;

        [Header("Global settings")]
        [SerializeField] private float speed         = 5f;
        [SerializeField] private Space relativeSpace = Space.World;

        #endregion

        #region Enum

        private enum SpinDirection
        {
            Random,
            X,
            Y,
            Z,
            XY,
            XZ,
            YZ,
            XYZ
        }

        #endregion

        #region Private variables

        private float   angle;
        private Vector3 spin;
        private bool    isPlaying = true;

        #endregion

        #region MonoBehaviour methods

        private void Awake()
        {
            ApplySettings();
        }

        private void FixedUpdate()
        {
            if (isPlaying)
            {
                SpinTransform();
            }
        }

        #endregion

        #region Spin

        public void Play()
        {
            isPlaying = true;
        }

        public void Stop()
        {
            isPlaying = false;
        }

        private void ApplySettings()
        {
            if (useCustomSpin)
            {
                spin  = customSpin;
                angle = speed;
            }
            else
            {
                spin  = AutoSpinDirection();
                angle = Random.Range(minRandomSpinSpeed, maxRandomSpinSpeed) * speed;
            }
        }

        private void SpinTransform()
        {
            transform.Rotate(spin, angle, relativeSpace);
        }

        #endregion

        #region Helpers

        private Vector3 AutoSpinDirection()
        {
            switch (spinDirection)
            {
                case SpinDirection.Random:
                    return new Vector3(Random.value, Random.value, Random.value);
                case SpinDirection.X:
                    return Vector3.right;
                case SpinDirection.Y:
                    return Vector3.up;
                case SpinDirection.Z:
                    return Vector3.forward;
                case SpinDirection.XY:
                    return new Vector3(1, 1);
                case SpinDirection.XZ:
                    return new Vector3(1, 0, 1);
                case SpinDirection.YZ:
                    return new Vector3(0, 1, 1);
                case SpinDirection.XYZ:
                    return Vector3.one;
                default:
                    return Vector3.zero;
            }
        }

        #endregion

    }

}
