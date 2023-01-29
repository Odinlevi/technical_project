using UnityEngine;

namespace Features.Player.Camera
{

    [RequireComponent(typeof(UnityEngine.Camera))]
    public class OrbitCamera : MonoBehaviour
    {
        public Transform focus = default;
        [SerializeField, Range(1f, 20f)] private float _distance = 5f;
        [SerializeField, Min(0f)] private float _focusRadius = 1f;

        [SerializeField, Range(0f, 1f)] float _focusCentering = 0.5f;
        
        private Vector3 _focusPoint;
        private Vector2 _orbitAngles = new Vector2(45f, 0f);

        [SerializeField, Range(1f, 720f)] private float _rotationSpeed = 90f;
        [SerializeField, Range(-89f, 89f)] private float _minVerticalAngle = -30f, _maxVerticalAngle = 60f;

        private void OnEnable()
        {
            _focusPoint = focus.position;
        }

        private void LateUpdate()
        {
            UpdateFocusPoint();
            Quaternion lookRotation;
            ManualRotation();
            if (ManualRotation())
            {
                ConstrainAngles();
                lookRotation = Quaternion.Euler(_orbitAngles);
            }
            else
            {
                lookRotation = transform.localRotation;
            }

            Vector3 lookDirection = lookRotation * Vector3.forward;
            Vector3 lookPosition = _focusPoint - lookDirection * _distance;
            transform.SetPositionAndRotation(lookPosition, lookRotation);
        }

        private void UpdateFocusPoint()
        {
            Vector3 targetPoint = focus.position;
            if (_focusRadius > 0f)
            {
                float distance = Vector3.Distance(targetPoint, _focusPoint);
                float t = 1f;
                if (distance > 0.01f && _focusCentering > 0f)
                {
                    t = Mathf.Pow(1f - _focusCentering, Time.unscaledDeltaTime);
                }

                if (distance > _focusRadius)
                {
                    //focusPoint = Vector3.Lerp(
                    //	targetPoint, focusPoint, focusRadius / distance
                    //);
                    t = Mathf.Min(t, _focusRadius / distance);
                }

                _focusPoint = Vector3.Lerp(targetPoint, _focusPoint, t);
            }
            else
            {
                _focusPoint = targetPoint;
            }
        }

        private bool ManualRotation()
        {
            Vector2 input = new Vector2(
                -Input.GetAxis("Mouse Y"),
                Input.GetAxis("Mouse X")
            );

            const float e = 0.001f;

            if (input.x < -e || input.x > e || input.y < -e || input.y > e)
            {
                _orbitAngles += _rotationSpeed * Time.unscaledDeltaTime * input;
                return true;
            }

            return false;
        }

        private void ConstrainAngles()
        {
            _orbitAngles.x = Mathf.Clamp(_orbitAngles.x, _minVerticalAngle, _maxVerticalAngle);

            if (_orbitAngles.y < 0f)
            {
                _orbitAngles.y += 360f;
            }
            else if (_orbitAngles.y >= 360f)
            {
                _orbitAngles.y -= 360f;
            }
        }
    }
}