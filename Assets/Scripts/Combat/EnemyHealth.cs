using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Core;

namespace Combat
{
    public class EnemyHealth : HealthController
    {
        [SerializeField] private float _disappearSpeed = 2f;
        [SerializeField] private Slider _mapSlider;

        private Rigidbody _rigidBody;
        private NavMeshAgent _navMesh;
        private CapsuleCollider _collider;

        private bool _startDisappearing = false;

        protected override void Start()
        {
            GameManager.RegisterEnemy(this);

            UnityEngine.Debug.LogError($"Spawned {gameObject.name}");

            base.Start();

            _navMesh = GetComponent<NavMeshAgent>();
            _rigidBody = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();

            _mapSlider.maxValue = StartHealthValue;
        }

        protected override void Update()
        {
            base.Update();

            if (_startDisappearing)
            {
                transform.Translate(-Vector3.up * _disappearSpeed * Time.deltaTime);
            }

            _mapSlider.value = _currentHealthValue;
        }

        protected override bool CanTakeDamage(Collider other)
        {
            return other.CompareTag("PlayerWeapon");
        }

        public override void Die()
        {
            base.Die();

            GameManager.RegisterKilledEnemy(this);

            _collider.enabled = false;
            _navMesh.enabled = false;
            _rigidBody.isKinematic = true;

            _mapSlider.gameObject.SetActive(false);

            StartCoroutine(RemoveEnemy());
        }

        private IEnumerator RemoveEnemy()
        {
            yield return new WaitForSeconds(4f);

            _startDisappearing = true;

            yield return new WaitForSeconds(2f);

            Destroy(gameObject);
        }
    }
}
