using Core;
using UnityEngine;

namespace Control
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] public CharacterType Type;
        [SerializeField] public Faction Faction;
    }
}