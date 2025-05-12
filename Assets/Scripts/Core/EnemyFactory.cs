using Control;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Core
{
    public class EnemyFactory : MonoBehaviour
    {
        private readonly List<Entity> _enemyPrefabs = new();

        public EnemyFactory(List<Entity> enemyPrefabs)
        {
            _enemyPrefabs = enemyPrefabs;
        }

        public Entity CreateEnemy(CharacterType type, Vector3 position, Quaternion rotation)
        {
            var prefab = _enemyPrefabs.FirstOrDefault(unit => unit.Type == type);
            if (prefab == null)
            {
                return null;
            }

            var instance = Instantiate(prefab, position, rotation);
            return instance;
        }
    }
}