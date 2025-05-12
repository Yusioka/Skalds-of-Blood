using TMPro;
using UnityEngine;

namespace Combat
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _damageText;

        public void DestroyText()
        {
            Destroy(gameObject);
        }

        private void LateUpdate()
        {
            transform.LookAt(2 * transform.position - Camera.main.transform.position);
        }

        public void SetValue(float amount)
        {
            _damageText.text = string.Format("{0:0.#}", amount);
        }
    }
}