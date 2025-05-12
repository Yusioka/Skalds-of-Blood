using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooldown : MonoBehaviour
{
    private float cooldownTime = 7f;
    private float timer = 0f;
    private PlayerAttackController playerAttack;
    [SerializeField] Image cooldown;


    void Update()
    {
        playerAttack = GetComponent<PlayerAttackController>();
        CanUseButton();
    }

    void CanUseButton()
    {
        if (playerAttack.WasUsedButton)
        {
            timer += Time.deltaTime;
        //    cooldown.fillAmount = timer/cooldownTime;

        }
        if(timer > cooldownTime && playerAttack.WasUsedButton)
        {
            timer = 0;
            playerAttack.WasUsedButton = false;
        }
    }
}