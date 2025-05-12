using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Contol
{
    public class TraitUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _unassignedPointsText;
        [SerializeField] private Button _applyButton;

        private TraitController _traitController;

        private void Start()
        {
            _traitController = GameManager.Instance.TraitController;
            _applyButton.onClick.AddListener(_traitController.ConfirmTrait);
        }

        private void Update()
        {
            _unassignedPointsText.text = _traitController.GetUnassignedPoints().ToString();
        }
    }
}