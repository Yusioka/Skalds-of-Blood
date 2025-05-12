using Combat;
using Core;
using UnityEngine;
using UnityEngine.AI;

namespace Control
{
    public class MovingController : MonoBehaviour
    {
        private GameObject _player;

        private Animator _animator;
        private NavMeshAgent _navMesh;

        private EnemyHealth _health;

        protected GameManager GameManager => GameManager.Instance;

        void Start()
        {
            _player = GameManager.Player;

            _animator = GetComponent<Animator>();
            _health = GetComponent<EnemyHealth>();
            _navMesh = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (_health.IsDead)
            {
                DisableNavMesh();
                return;
            }

            if (GameManager.IsGameOver)
            {
                return;
            }

            _navMesh.SetDestination(_player.transform.position);
        }

        private void DisableNavMesh()
        {
            _navMesh.enabled = false;
            //_animator.Play("idle");
        }
    }
}