using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private GameObject _player;
        [SerializeField] private GameObject _ranger;
        [SerializeField] private GameObject _soldier;
        [SerializeField] private GameObject _tanker;

        private Animator _playerAnimator;
        private Animator _rangerAnimator;
        private Animator _soldierAnimator;
        private Animator _tankerAnimator;

        private void Start()
        {
            _playerAnimator = _player.GetComponent<Animator>();
            _rangerAnimator = _ranger.GetComponent<Animator>();
            _soldierAnimator = _soldier.GetComponent<Animator>();
            _tankerAnimator = _tanker.GetComponent<Animator>();
        }

        private void Update()
        {
            _rangerAnimator.Play("idle");
            _soldierAnimator.Play("idle");
            _tankerAnimator.Play("idle");
            _playerAnimator.Play("idle");
        }

        public void Battle()
        {
            SceneManager.LoadSceneAsync("SampleScene");
        }

        public void GameMenu()
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
