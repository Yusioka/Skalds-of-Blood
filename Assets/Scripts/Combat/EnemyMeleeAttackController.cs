using Control;
using Core;
using UnityEngine;

namespace Combat
{
    public class EnemyMeleeAttackController : BaseAttackController
    {
        [SerializeField] protected float _attackRange = 3f;

        protected GameObject _player;
        protected EnemyHealth _health;

        private bool _isPlayerInRange;

        protected bool IsPlayerInRange
        {
            get => _isPlayerInRange;
            set
            {
                if (_isPlayerInRange == value)
                {
                    return;
                }

                _isPlayerInRange = value;
            }
        }

        protected override void Start()
        {
            base.Start();

            _player = GameManager.Player;
            _health = GetComponent<EnemyHealth>();
        }

        protected virtual void Update()
        {
            _firstAttackTimer -= Time.deltaTime;

            IsPlayerInRange = Vector3.Distance(transform.position, _player.transform.position) < _attackRange && _health.IsDead == false;

            if (IsPlayerInRange && GameManager.IsGameOver == false && _health.IsDead == false && _firstAttackTimer < 0)
            {
                PerformAttack();
            }
        }

        public override void PerformAttack()
        {
            var cooldownMultiplier = GameManager.GetStat(StatType.AttackSpeed, GetComponent<Entity>().Type);
            _firstAttackTimer = _firstAttackCooldown / cooldownMultiplier;
            _animator.Play("attack");
        }
    }
}