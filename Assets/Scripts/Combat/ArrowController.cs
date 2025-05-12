using Core;
using UnityEngine;

namespace Combat
{
    public class ArrowController : MonoBehaviour
    {
        private void Start()
        {
            var player = GameManager.Instance.Player;

            transform.LookAt(player.transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" || other.tag == "Environment")
            {
                Destroy(gameObject);
            }
        }
    }
}