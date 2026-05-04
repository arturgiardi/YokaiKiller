using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthGuiManager : MonoBehaviour 
{
	public static HealthGuiManager instance;

	[SerializeField] private Image healthBar;
	[SerializeField] private Image healthBarShadow;
	[SerializeField] private Image xpBar;
	[SerializeField] private Image healthBarFrame;
	[SerializeField] private Text healthAmount;
	[SerializeField] private Text level;
	[SerializeField] private Image subweaponCooldownFill;
	[SerializeField] private TMP_Text subweaponIconText;

	PlayerStats stats;

	IEnumerator healthUpdateCoroutine;

	void Awake()
	{
		if(instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	void Start()
	{
		if(instance == this)
		{
			stats = PlayerStatsController.instance.stats;
			stats.OnChangeHP += UpdateHealth;
			stats.OnEarnXP += UpdateXp;
			UpdateSubweaponIcon();
		}
	}

	public void SetLevel(int value){
		if (value < 10)
			level.text = "LV:0" + value.ToString ();
		else
			level.text = "LV:"+value.ToString ();
	}


	public void UpdateHealth(float maxHealth, float currentHealth)
	{
		if(instance != this)
			return;
		healthBar.fillAmount = currentHealth/maxHealth;
		//healthBarFrame.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, 1.92f * total + 15);
		//healthBar.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, 192 * health/total);
		//healthAmount.text = currentHealth.ToString () + "/" + maxHealth.ToString ();
		if(healthUpdateCoroutine != null)
			StopCoroutine(healthUpdateCoroutine);
		healthUpdateCoroutine = _ShadowHealthFollow();
		StartCoroutine(healthUpdateCoroutine);

	}

	public void UpdateXp(float ammount, float percentage)
	{
		if(instance != this)
			return;
		xpBar.fillAmount = percentage;
	}


	void Update(){
		if(instance != this)
			return;

	}


	IEnumerator _ShadowHealthFollow()
	{
		while(Mathf.Abs(healthBarShadow.fillAmount - healthBar.fillAmount) > 0.001f)
		{
			healthBarShadow.fillAmount = Mathf.MoveTowards(healthBarShadow.fillAmount, healthBar.fillAmount, 0.1f*Time.deltaTime);
			yield return null;
		}
	}

	public void SetSubweaponTimer(float currentTime, float totalTime)
	{
		subweaponCooldownFill.fillAmount = 1 - currentTime/totalTime;
	}

	public void UpdateSubweaponIcon()
	{
		int icon = PlayerStatsController.instance.currentSubweapon.icon;
		subweaponIconText.text = "<sprite="+icon+">";
	}

}
