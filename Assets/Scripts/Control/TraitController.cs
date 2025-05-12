using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    public class TraitController : MonoBehaviour
    {
        [SerializeField] TraitBonus[] _bonusConfigs;

        private Dictionary<TraitType, int> _assigned = new();
        private Dictionary<TraitType, int> _staged = new();

        private Dictionary<StatType, Dictionary<TraitType, float>> _bonusBuffer = new();

        private void Awake()
        {
            foreach (var bonusConfig in _bonusConfigs)
            {
                if (_bonusBuffer.ContainsKey(bonusConfig.StatType) == false)
                {
                    _bonusBuffer[bonusConfig.StatType] = new();
                }

                _bonusBuffer[bonusConfig.StatType][bonusConfig.TraitType] = bonusConfig.Multiplier;
            }
        }
        public int GetProposedPoints(TraitType trait)
        {
            return GetPoints(trait) + GetStagedPoints(trait);
        }

        public int GetPoints(TraitType trait)
        {
            return _assigned.ContainsKey(trait) ? _assigned[trait] : 0;
        }

        public int GetStagedPoints(TraitType trait)
        {
            return _staged.ContainsKey(trait) ? _staged[trait] : 0;
        }

        public void AssignPoints(TraitType trait, int points)
        {
            if (CanAssignPoints(trait, points) == false)
            {
                return;
            }

            var bonusConfig = _bonusConfigs.FirstOrDefault(config => config.TraitType == trait);
            _staged[trait] = GetStagedPoints(trait) + points;
        }

        public bool CanAssignPoints(TraitType trait, int points)
        {
            if (GetStagedPoints(trait) + points < 0)
            {
                return false;
            }

            if (GetUnassignedPoints() < points)
            {
                return false;
            }

            return true;
        }

        public int GetUnassignedPoints()
        {
            return GetAssignablePoints() - GetTotalProposedPoints();
        }

        public void ConfirmTrait()
        {
            foreach (TraitType trait in _staged.Keys)
            {
                _assigned[trait] = GetProposedPoints(trait);
            }

            _staged.Clear();
        }

        public IEnumerable<float> GetModifiers(StatType stat)
        {
            if (_bonusBuffer.ContainsKey(stat) == false)
            {
                yield break;
            }

            foreach (TraitType trait in _bonusBuffer[stat].Keys)
            {
                var bonus = _bonusBuffer[stat][trait];
                yield return bonus * GetPoints(trait);
            }
        }

        private int GetAssignablePoints()
        {
            return (int)GameManager.Instance.GetStat(StatType.TotalTraitPoints, CharacterType.Player);
        }

        private int GetTotalProposedPoints()
        {
            var total = 0;
            foreach (int points in _assigned.Values)
            {
                total += points;
            }
            foreach (int points in _staged.Values)
            {
                total += points;
            }
            return total;
        }

        [System.Serializable]
        private class TraitBonus
        {
            public TraitType TraitType;
            public StatType StatType;
            public float Multiplier;
        }
    }
}