using UnityEngine;

namespace Core
{
    public class ObstacleController : MonoBehaviour
    {
        private Animator _animator;

        private bool _isRaised = false;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerExit(Collider other)
        {
            if (_isRaised == false)
            {
                return;
            }

            Lower();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") == false || _isRaised)
            {
                return;
            }

            Raise();
        }

        private void Raise()
        {
            _isRaised = true;
            _animator.SetBool("readyToUp", true);
        }

        private void Lower()
        {
            _isRaised = false;
            _animator.SetBool("readyToUp", false);
        }
    }
}