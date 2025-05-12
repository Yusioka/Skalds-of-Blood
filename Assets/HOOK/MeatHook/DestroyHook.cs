using UnityEngine;

public class DestroyHook : MonoBehaviour
{
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}