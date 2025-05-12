using UnityEngine;

namespace Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _distanceToPlayer = 5.0f;
        [SerializeField] private float _height = 2.0f;
        [SerializeField] private float _rotationSpeed = 5.0f;
        [SerializeField] private float _zoomSpeed = 2.0f;
        [SerializeField] private float _minDistance = 2.0f;
        [SerializeField] private float _maxDistance = 10.0f;

        private GameObject player;
        private Vector2 rotationInput;

        void Start()
        {
            player = GameManager.Instance.Player;
        }

        void Update()
        {
            if (GameManager.Instance.IsPaused || GameManager.Instance.IsSkillUsing)
            {
                return;
            }

            rotationInput.x += Input.GetAxis("Mouse X") * _rotationSpeed;
            rotationInput.y -= Input.GetAxis("Mouse Y") * _rotationSpeed;
            rotationInput.y = Mathf.Clamp(rotationInput.y, -20f, 60f);

            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            _distanceToPlayer -= scrollInput * _zoomSpeed;
            _distanceToPlayer = Mathf.Clamp(_distanceToPlayer, _minDistance, _maxDistance);
        }

        void LateUpdate()
        {
            if (player == null || GameManager.Instance.IsPaused || GameManager.Instance.IsSkillUsing)
            {
                return;
            }

            Quaternion rotation = Quaternion.Euler(rotationInput.y, rotationInput.x, 0);
            Vector3 position = player.transform.position - (rotation * Vector3.forward * _distanceToPlayer) + (Vector3.up * _height);

            transform.position = position;
            transform.LookAt(player.transform.position + Vector3.up * _height * 0.5f);

            RotateCharacter();
        }

        void RotateCharacter()
        {
            Vector3 cameraForward = transform.forward;
            cameraForward.y = 0;

            if (cameraForward.sqrMagnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
            }
        }
    }
}