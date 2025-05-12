using Core;
using UnityEngine;

namespace Combat
{
    public class PlayerHealth : HealthController
    {
        protected override bool CanTakeDamage(Collider other)
        {
            return other.CompareTag("EnemyWeapon") || other.CompareTag("Arrow");
        }

        public override void TakeHit(float value)
        {
            if (GameManager.IsGodMode)
            {
                return;
            }

            base.TakeHit(value);

            GameManager.PlayerHit(_currentHealthValue);
        }

        public override void Die()
        {
            base.Die();

            GameManager.PlayerHit(0);
        }
    }
}