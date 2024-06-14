using UnityEngine;

namespace Introduction
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform player; // Drag your player here
        private Vector3 offset; // Distance between the player and camera
        public float smoothSpeed = 0.125f; // How smoothly the camera catches up to the player

        private void Start()
        {
            offset = transform.position - player.position;
        }

        private void LateUpdate()
        {
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}