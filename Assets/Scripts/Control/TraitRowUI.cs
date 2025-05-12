using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Contol
{
    public class TraitRowUI : MonoBehaviour
    {
        [SerializeField] private TraitType _traitType;
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private Button _minusButton;
        [SerializeField] private Button _plusButton;

        private TraitController _traitController;

        private void OnEnable()
        {
            _traitController = GameManager.Instance.TraitController;
            _minusButton.onClick.AddListener(() => Allocate(-1));
            _plusButton.onClick.AddListener(() => Allocate(+1));
        }

        private void Update()
        {
            _minusButton.interactable = _traitController.CanAssignPoints(_traitType, -1);
            _plusButton.interactable = _traitController.CanAssignPoints(_traitType, +1);
            _valueText.text = _traitController.GetProposedPoints(_traitType).ToString();
        }

        public void Allocate(int points)
        {
            _traitController.AssignPoints(_traitType, points);
        }
    }
}