using UnityEngine;

namespace EndlessRunner
{
    /// <summary>
    /// C# version the original javascript called SmoothFollow.
    /// </summary>
    [AddComponentMenu("CUSTOM / Camera Follow")]
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform m_target;
        [SerializeField]
        private float m_distance;
        [SerializeField]
        private float m_height;
        [SerializeField]
        private float m_heightDamping;
        [SerializeField]
        private float m_rotationDamping;

        void LateUpdate()
        {
            var wantedRotationAngle = m_target.eulerAngles.y;
            var wantedHeight = m_target.position.y + m_height;

            var currentRotationAngle = transform.eulerAngles.y;
            var currentHeight = transform.position.y;

            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, m_rotationDamping * Time.deltaTime);

            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, m_heightDamping * Time.deltaTime);

            var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            transform.position = m_target.position;
            transform.position -= currentRotation * Vector3.forward * m_distance;

            transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

            transform.LookAt(m_target);
        }
    }
}