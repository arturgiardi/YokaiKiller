#undef BATTLELOG
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour 
{
	public delegate void ResetStateController(EnemyStats stats);
	public ResetStateController ResetEvent;
	public AIState currentState;
	public EnemyStats stats;
	EnemyStats _stats;
	public DropController dropController;
	public Transform origin;
	public Transform rangedPoint;
	public CharacterController body;

	public LayerMask groundLayerMask;

	public AIAttack attack;
	public AiReaction reaction;

	public float cooldownTimer = 0;

	[HideInInspector] public Animator animator;

	private bool aiActive;
	private bool awake;	

	[HideInInspector] public Transform target;

	Vector3 initialPosition;
	Vector3 initialScale;
	AIState initialState;

	EnemyHealthbar healthBar;


	float zPosition;
	public static int lastPosit = 0;

    IEnumerator stutterCoroutine;



	void OnDrawGizmos()
	{
		#if UNITY_EDITOR
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(origin.position, stats.activationRange);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(origin.position, stats.interactionRange);
			Gizmos.DrawRay(origin.position, (origin.position + Vector3.right*stats.leapRange*transform.localScale.x + Vector3.down*stats.leapDepth) - origin.position);
			Gizmos.color = Color.yellow;
			//Gizmos.DrawRay(origin.position + Vector3.up/5, (origin.position + Vector3.up/5 + Vector3.right*transform.localScale.x*stats.wallDetectionRange) - (origin.position + Vector3.up/5));
			Gizmos.DrawRay(origin.position, (origin.position + Vector3.right*transform.localScale.x*stats.wallDetectionRange) - (origin.position));
			//Gizmos.DrawRay(origin.position - Vector3.up/5, (origin.position - Vector3.up/5 + Vector3.right*transform.localScale.x*stats.wallDetectionRange) - (origin.position - Vector3.up/5));
			Gizmos.color = Color.blue;
			Gizmos.DrawRay(origin.position + Vector3.up*stats.activationHeight + Vector3.right*transform.localScale.x*stats.activationRange*-1, (origin.position + Vector3.up*stats.activationHeight + Vector3.right*transform.localScale.x*stats.activationRange) - (origin.position + Vector3.up*stats.activationHeight + Vector3.right*transform.localScale.x*stats.activationRange*-1));
			Gizmos.DrawRay(origin.position - Vector3.up*stats.activationHeight + Vector3.right*transform.localScale.x*stats.activationRange*-1, (origin.position - Vector3.up*stats.activationHeight + Vector3.right*transform.localScale.x*stats.activationRange) - (origin.position - Vector3.up*stats.activationHeight + Vector3.right*transform.localScale.x*stats.activationRange*-1));
		#endif
		
	}

	void Awake()
	{
		initialPosition = transform.position;
		initialScale = transform.localScale;
		initialState = currentState;

		zPosition = GetPosit();

		_stats = stats;
		stats = stats.GetInstance(stats);
		stats.Setup();
		animator = GetComponent<Animator>();
		aiActive = true;
		target = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
		stats.OnDeath+=DeathEvent;
		stats.OnDamage += ReactToDamage;
		if(reaction != null)
		{
			reaction = Object.Instantiate(reaction);
		}
	}



	void OnEnable()
	{
		if(!awake)
		{
			awake = true;
			if(stats.boss)
			{
				GameManager.instance.bossHp.BossHpAmmount(stats.maxHealth, stats.currentHealth);
				GameManager.instance.bossHp.ShowBossHP();
			}
			return;
		}
		transform.position = initialPosition;
		transform.localScale = initialScale;
		currentState = initialState;
		//animator.Rebind();
		aiActive = true;
		//animator.Rebind();
		if(target == null)
			target = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
		stats = stats.GetInstance(_stats);
		stats.Setup();
		stats.OnDeath+=DeathEvent;
		stats.OnDamage += ReactToDamage;
		if(ResetEvent != null)
			ResetEvent(stats);
	}

	void OnDisable()
	{
		if(stats.boss)
			GameManager.instance.bossHp.HideBossHP();
	}

	float GetPosit()
	{
		lastPosit ++;
		float returnPosit = 0.3f + (0.003f * lastPosit);
		return returnPosit;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.B))
			OnEnable();
		if(!aiActive || !stats.alive)
			return;
		
		transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
		if(cooldownTimer > 0)
		{
			cooldownTimer -= Time.deltaTime;
			return;
		}

		
		currentState.UpdateState(this);
		
	}

	// void OnDisable()
	// {
	// 	animator.Rebind();	
	// }

	public void Attack()
	{
		if(cooldownTimer <= 0)
		{
			animator.Play("AttackBlink",2);
			attack.DoAttack(this);
		}
	}

	public bool TransitionToState(AIState nextState)
	{
		if(nextState != null)
		{
			if(body != null)
			{
				if(CheckGrounded())
					currentState = nextState;
			}
			else
			{
				currentState = nextState;
			}
			return true;
		}
		return false;
	}

	public void TurnToTarget()
	{
		if(cooldownTimer > 0)
			return;
		if(target.position.x > origin.position.x && transform.localScale.x < 0)
		{	
			transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
		}
		else if(target.position.x < origin.position.x)
		{
			transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
		}
	}

	public void FireProjectile(Object atkData)
	{
		Debug.Log(atkData);
		ProjectileData newData = atkData as ProjectileData;
		GameObject newProjectile = Instantiate(newData.projectile, rangedPoint.position, Quaternion.identity) as GameObject;
		newProjectile.transform.forward = rangedPoint.forward;
		newProjectile.GetComponent<Projectile>().ShootProjectile(newData.throwForce);
	}

	bool CheckGrounded()
	{
		Ray ray = new Ray(origin.position, (origin.position + Vector3.down*stats.leapDepth) - origin.position);
		Debug.DrawRay(ray.origin, ray.direction.normalized*((stats.leapDepth)), Color.cyan);
		if(!Physics.Raycast(ray, ((stats.leapDepth)), groundLayerMask))
		{
			return false;
		}
		else
			return true;
	}

	public void ReactToDamage(GameObject instigator, DamageInfo damage, Stats attacker)
	{
		if(stats.alive)
		{
            if (reaction != null)
            {
                reaction.React(this, instigator, damage);
            }
            //brilho de dano
			//animator.Play("Damage",1,0);

			DamageInfo finalDamageInfo = Stats.CalculateDamage(damage, stats.defense, attacker, stats);
			float finalDamage = finalDamageInfo.ammount;

			if (finalDamage > 0)
			{
				if(finalDamageInfo.critical)
				{
					FindObjectOfType<CameraManager> ().ShakeCamera (damage.contactTime*(damage.criticalMultiplier/2)*3, damage.intensity*(damage.criticalMultiplier/2)*3);
					DamagePopup.InstantiateDamage (DamagePopup.DamageColor.WhiteCritical, origin.position, finalDamage);
				}
				else
				{
					FindObjectOfType<CameraManager> ().ShakeCamera (damage.contactTime*3, damage.intensity*2);
					DamagePopup.InstantiateDamage (DamagePopup.DamageColor.White, origin.position, finalDamage);
				}
				
				stats.currentHealth -= finalDamage;
				//stats.currentHealth = Mathf.Round(stats.currentHealth);
				if(stats.OnChangeHP != null)
				{
					stats.OnChangeHP(stats.maxHealth, stats.currentHealth);
				}

				//Debug.Log(PlayerStatsController.instance.specialEffects.healthInspector);

				if(!stats.boss)
				{
					if(PlayerStatsController.instance.specialEffects.healthInspector == true && stats.showLifeBar)
					{
						ChangeHealthbar();
					}

					if(EnemyNamePrompt.OnHitEnemy != null && stats.showName)
						EnemyNamePrompt.OnHitEnemy(stats.enemyName);
				}
				else
				{
					GameManager.instance.bossHp.BossHpAmmount(stats.maxHealth, stats.currentHealth);
				}
			}

			if(stats.currentHealth <= 0 && stats.OnDeath != null)
			{
				stats.OnDeath(instigator,damage,attacker);
				#if BATTLELOG
					//Debug.Log("<color=orange><b>>>>>>>>>>>>>>>>>> </b></color><color=brown><b>" + stats.enemyName + "</b></color> took <color=brown><b>" + finalDamage + "</b></color> damage from <color=green><b>" + instigator.name +".</b></color><color=brown><b> " + stats.enemyName + "</b></color> is <color=brown><b>dead! </b></color><color=orange><b><<<<<<<<<<<<<<<<<<<<</b></color>");
				#endif
			}
			else
			{
				#if BATTLELOG
					//Debug.Log("<color=orange><b>>>>>>>>>>>>>>>>>> </b></color><color=brown><b>" + stats.enemyName + "</b></color> took <color=brown><b>" + finalDamage + "</b></color> damage from <color=green><b>" + instigator.name +".</b></color><color=brown><b> " + stats.enemyName + "</b></color> has <color=brown><b>" + stats.currentHealth + "</b></color> health left!<color=orange><b> <<<<<<<<<<<<<<<<<<<<</b></color>");
				#endif
			}
		
			
		}
	}
	
	[ContextMenu("KILL!")]
	public void KillTest()
	{
		DeathEvent(null, null, null);
	}
	public void DeathEvent(GameObject instigator, DamageInfo damage, Stats attacker)
	{
        animator.speed = 1;
		if(stats.boss)
			GameManager.instance.bossHp.HideBossHP();
		StopAllCoroutines();
		if(dropController != null)
			dropController.DropItems();
		aiActive = false;
		print("morto");
		animator.Play("Dead",0);
		//animator.Play("Damage",1,0);
		PlayerStats.instance.AddXP(stats.experience);
	}

	public void ChangeHealthbar()
	{
		if(healthBar == null)
		{		
			healthBar = GameManager.instance.enemyHbManager.RequestNewHealthbar(origin, stats.currentHealth/stats.maxHealth);
		}
		healthBar.ChangeHP(stats.currentHealth, stats.maxHealth);

	}

    public void Stutter(float time)
    {
        if (!aiActive)
            return;
        Debug.Log("STUTTERING");
        if(stutterCoroutine == null)
        {
            stutterCoroutine = _Stutter(time);
            StartCoroutine(stutterCoroutine);
        }
    }

    IEnumerator _Stutter(float time)
    {
        animator.SetBool("Stun", true);
        animator.speed = 0;
        yield return new WaitForSeconds(time);
        animator.SetBool("Stun", false);
        animator.speed = 1;
        yield return new WaitForSeconds(time/2);
        stutterCoroutine = null;
    }


}
