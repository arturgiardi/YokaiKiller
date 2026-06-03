using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BossHpManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    public Image bar;
    public TMP_Text ammount;

    public bool on = false;

    //IEnumerator transitionCoroutine;
    private void Start()
    {
        ammount.gameObject.SetActive(false);
        _canvasGroup.alpha = 0;
        on = false;
    }
    public void HideBossHP()
    {
        _canvasGroup.DOFade(0, 0.5f);
        on = false;
    }

    public void ShowBossHP()
    {
        _canvasGroup.DOFade(1, 0.5f);
        on = true;
    }

    public void BossHpAmmount(float maxHP, float currentHP)
    {
        bar.fillAmount = currentHP / maxHP;
        ammount.text = currentHP.ToString() + "/" + maxHP.ToString();
    }

}
