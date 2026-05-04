using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHpManager : MonoBehaviour {
    public Image bar;
    public TMP_Text ammount;
    public Animator animator;

    public bool on = false;

    //IEnumerator transitionCoroutine;

    public void HideBossHP()
    {
        animator.SetTrigger("TriggerOff");
        on = false;
    }

    public void ShowBossHP()
    {
        if(PlayerStatsController.instance.specialEffects.healthInspector)
        {
            animator.SetTrigger("TriggerOn");
        }
        else
        {
            animator.SetTrigger("TriggerOn");
        }
        on = true;
    }

    public void BossHpAmmount(float maxHP, float currentHP)
    {
        bar.fillAmount = currentHP / maxHP;
        ammount.text = currentHP.ToString() + "/" + maxHP.ToString();
    }

}
