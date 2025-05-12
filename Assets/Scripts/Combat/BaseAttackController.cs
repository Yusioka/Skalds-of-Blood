using Control;
using Core;
using UnityEngine;

namespace Combat
{
    public abstract class BaseAttackController : MonoBehaviour, IAttackable
    {
        [SerializeField] protected float _firstAttackCooldown = 1f;
        [SerializeField] protected BoxCollider[] _weaponColliders;

        protected Entity _entity;
        protected Animator _animator;
        protected float _firstAttackTimer;

        protected GameManager GameManager => GameManager.Instance;

        protected virtual void Start()
        {
            _entity = GetComponent<Entity>();
            _animator = GetComponent<Animator>();
        }

        public void StartAttack()
        {
            foreach (var weapon in _weaponColliders)
            {
                weapon.enabled = true;
            }
        }

        public void EndAttack()
        {
            foreach (var weapon in _weaponColliders)
            {
                weapon.enabled = false;
            }
        }

        public abstract void PerformAttack();
    }
}