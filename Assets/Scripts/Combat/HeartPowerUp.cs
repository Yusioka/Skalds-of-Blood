using Combat;
using Core;
using UnityEngine;

namespace Combat
{
    public class HeartPowerUp : MonoBehaviour
    {
        [SerializeField] private AudioSource _pickUpAudio;

        private PlayerHealth _playerHealth;

        void Start()
        {
            _pickUpAudio = GetComponent<AudioSource>();
            _playerHealth = GameManager.Instance.Player.GetComponent<PlayerHealth>();

            GameManager.Instance.RegisterPowerUp();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player")
            {
                return;
            }

            _playerHealth.RestoreHealth();
            _pickUpAudio.PlayOneShot(_pickUpAudio.clip);

            Destroy(gameObject);

            GameManager.Instance.CanSpawnPowerUp = true;
        }
    }
}