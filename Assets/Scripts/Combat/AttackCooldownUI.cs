using Combat;
using UnityEngine;
using UnityEngine.UI;

public class AttackCooldownUI : MonoBehaviour
{
    [SerializeField] private PlayerAttackController _attackController;
    [SerializeField] private Image _fisrtAttackCooldown;
    [SerializeField] private Image _secondAttackCooldown;
    [SerializeField] private Image _fisrtAbilityCooldown;
    [SerializeField] private Image _secondAbilityCooldown;

    private void Update()
    {
        _fisrtAttackCooldown.fillAmount = _attackController.FirstAttackFillAmount;
        _secondAttackCooldown.fillAmount = _attackController.SecondAttackFillAmount;
        _fisrtAbilityCooldown.fillAmount = _attackController.FirstAbilityFillAmount;
        _secondAbilityCooldown.fillAmount = _attackController.SecondAbilityFillAmount;
    }

    public void OnFirstAttack()
    {
        _attackController.FirstAttack();
    }

    public void OnSecondAttack()
    {
        _attackController.SecondAttack();
    }

    public void OnFirstAbility()
    {
        _attackController.UseFirstAbility();
    }

    public void OnSecondAbility()
    {
        _attackController.UseSecondAbility();
    }
}