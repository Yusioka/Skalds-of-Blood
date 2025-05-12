using Control;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class PlayerAttackController : BaseAttackController
    {
        private const float SecondAttackCooldown = 3;

        [SerializeField] private Text _attackingError;
        [SerializeField] private Ability _abilityFirst;
        [SerializeField] private Ability _abilitySecond;
        [SerializeField] public Transform AbilityPoint;

        public bool WasUsedButton { get; set; }
        public float FirstAttackFillAmount => Mathf.Clamp01(_firstAttackTimer / _firstAttackCooldown / _cooldownMultiplier);
        public float SecondAttackFillAmount => Mathf.Clamp01(_secondAttackTimer / SecondAttackCooldown / _cooldownMultiplier);
        public float FirstAbilityFillAmount => Mathf.Clamp01(_abilityFirst.CooldownTimer / _abilityFirst.cooldownTime / _cooldownMultiplier);
        public float SecondAbilityFillAmount => Mathf.Clamp01(_abilitySecond.CooldownTimer / _abilitySecond.cooldownTime / _cooldownMultiplier);

        private float _cooldownMultiplier;
        private float _secondAttackTimer;

        private void Update()
        {
            if (GameManager.IsGameOver || GameManager.IsPaused)
            {
                return;
            }

            _cooldownMultiplier = GameManager.GetStat(StatType.AttackSpeed, GetComponent<Entity>().Type);

            _firstAttackTimer -= Time.deltaTime;
            _secondAttackTimer -= Time.deltaTime;
            _abilityFirst?.UpdateCooldown(Time.deltaTime);
            _abilitySecond?.UpdateCooldown(Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Q) && GameManager.CanUseAbility)
            {
                UseFirstAbility();
            }

            if (Input.GetKeyDown(KeyCode.E) && GameManager.CanUseAbility)
            {
                UseSecondAbility();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetMouseButton(0))
            {
                FirstAttack();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetMouseButton(1))
            {
                SecondAttack();
            }
        }

        public void FirstAttack()
        {
            if (_firstAttackTimer < 0)
            {
                _firstAttackTimer = _firstAttackCooldown / _cooldownMultiplier;
                _animator.Play("chop");
            }
        }

        public void SecondAttack()
        {
            if (_secondAttackTimer < 0)
            {
                _secondAttackTimer = SecondAttackCooldown / _cooldownMultiplier;
                _animator.Play("spin");
            }
        }

        public void UseFirstAbility()
        {
            if (_abilityFirst.TryUse(_entity, _cooldownMultiplier))
            {
                GameManager.RegisterAbility(_abilityFirst);
            }
        }

        public void UseSecondAbility()
        {
            if (_abilitySecond.TryUse(_entity, _cooldownMultiplier))
            {
                GameManager.RegisterAbility(_abilitySecond);
            }
        }

        public override void PerformAttack()
        {
        }
    }
}