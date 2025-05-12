using Core;
using UnityEngine;

namespace Combat
{
    public class EnemyRangeredAttackController : EnemyMeleeAttackController
    {
        [SerializeField] private GameObject _arrowPrefab;
        [SerializeField] private Transform _arrowTransform;
        [SerializeField] private float _arrowSpeed = 20f;

        protected override void Update()
        {
            base.Update();

            _animator.SetBool("PlayerInRange", IsPlayerInRange);
            RotateTowards(_player.transform);
        }

        public override void PerformAttack()
        {
            if (IsPlayerInRange == false || GameManager.IsGameOver || _health.IsDead || _firstAttackTimer > 0)
            {
                return;
            }

            base.PerformAttack();

            FireArrow();
        }

        public void FireArrow()
        {
            GameObject newArrow = Instantiate(_arrowPrefab, _arrowTransform.position, _arrowTransform.rotation);
            newArrow.GetComponent<Rigidbody>().velocity = transform.forward * _arrowSpeed;
        }

        private void RotateTowards(Transform target)
        {
            if (IsPlayerInRange == false)
            {
                return;
            }

            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }
}