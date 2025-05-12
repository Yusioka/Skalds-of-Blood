using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public AttributeSpell attribute;
    public virtual  void ActivateSpell(Vector3 point)
    {
        Debug.Log("Activate!");
    }
}