using Core;
using System.Collections.Generic;
using UnityEngine;

namespace Control
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Progression/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacters[] _characters;

        Dictionary<CharacterType, Dictionary<StatType, int[]>> _statsTable;

        public int GetStat(StatType statType, CharacterType characterType, int level)
        {
            BuildTable();

            if (_statsTable[characterType].ContainsKey(statType) == false)
            {
                return 0;
            }

            var levels = _statsTable[characterType][statType];

            if (levels.Length == 0)
            {
                return 0;
            }

            if (levels.Length < level)
            {
                return levels[levels.Length];
            }

            return levels[level];
        }

        public int GetLevels(StatType statType, CharacterType characterType)
        {
            BuildTable();

            var levels = _statsTable[characterType][statType];
            return levels.Length;
        }

        private void BuildTable()
        {
            if (_statsTable != null)
            {
                return;
            }

            _statsTable = new Dictionary<CharacterType, Dictionary<StatType, int[]>>();

            foreach (ProgressionCharacters progressionClass in _characters)
            {
                var statLookupTable = new Dictionary<StatType, int[]>();

                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }

                _statsTable[progressionClass.characterClass] = statLookupTable;
            }
        }

        [System.Serializable]
        class ProgressionCharacters
        {
            public CharacterType characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public StatType stat;
            public int[] levels;
        }
    }
}
