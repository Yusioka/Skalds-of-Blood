using Core;
using UnityEngine;

namespace Contol
{
    public class ToggleUI : MonoBehaviour
    {
        [SerializeField] private KeyCode _toggleKey;
        [SerializeField] private GameObject _uiContainer;
        [SerializeField] private bool _activateOnStart;

        private bool _isActive;

        private void Start()
        {
            if (_uiContainer == null)
            {
                return;
            }

            _isActive = _activateOnStart;

            foreach (Transform child in _uiContainer.transform)
            {
                child.gameObject.SetActive(_activateOnStart);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(_toggleKey))
            {
                Toggle();
            }
        }

        public void Toggle()
        {
            if (_uiContainer == null || GameManager.Instance.IsGameOver)
            {
                return;
            }

            _isActive = !_isActive;

            foreach (Transform child in _uiContainer.transform)
            {
                child.gameObject.SetActive(_isActive);
            }

            GameManager.Instance.IsPaused = _isActive;
        }
    }
}