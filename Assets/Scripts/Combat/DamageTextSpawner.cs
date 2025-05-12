using UnityEngine;

namespace Combat
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText _damageTextPrefab;

        public void SpawnDamage(float damageAmount)
        {
            var instance = Instantiate(_damageTextPrefab, transform);
            instance.SetValue(Mathf.RoundToInt(damageAmount));
        }
    }
}