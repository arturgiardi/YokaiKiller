using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Homebrew;

public class NewController : MonoBehaviour 
{
	#region Combo Setups

	[SerializeField] ComboSetup groundedComboSequence;
	[SerializeField] ComboSetup airComboSequence;
	[SerializeField] ComboSetup airComboSequenceMoving;
	[SerializeField] ComboSetup crouchingComboSequence;
	[SerializeField] ComboSetup powerAttacks;

    #endregion

    #region Enums

    public enum AttackState {WantToAttack, WantToAttackSub, WillAttack, AttackBlocked, None, Attacking, ChainAttack};
    public enum JumpState {WantToJump, Jumping, None, JumpLocked, NoGravity};
    public enum DodgeState {WantToDodge, Dodging, None, DodgeLocked, Attacking};
	public enum MoveState {Enabled, Attacking, Damage, Dodging, Blocked};
	public enum ComboState {Grounded, Air, AirMoving, Crouching};
	enum ControllerState {Enabled, Disabled, Waiting, Death};
	enum AttackType {Normal, Super};
	enum DodgeStyle {OnlyGrounded, OnAir};

    public enum ControllerMoveState {Grounded, Air, AirMoving, Crouching};

	#endregion

    #region States
	
    [Foldout("States", true)]
    [Header("Controller States")]
	[Space(10)]
	[SerializeField] ControllerState controllerState = ControllerState.Enabled;
	public MoveState moveState;
    public DodgeState dodgeState = DodgeState.None;
    public JumpState jumpState = JumpState.None;
    public AttackState attackState = AttackState.None;

	#endregion

    //////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////Action Types/////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////
    [Foldout("Actions", true)]
    [Space(5)]
	[Header("Action Types")]
	[Space(10)]
	    
    [SerializeField] AttackType attackType = AttackType.Normal;
	[SerializeField] DodgeStyle dodgeStyle = DodgeStyle.OnlyGrounded;

    //////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////Linked Refferences//////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////

    [Foldout("Refferences", true)]
    [Header("Linked Refferences")]
	[Space(10)]
    [SerializeField] CharacterController body;
	public Animator animator;
	[SerializeField] ParticleSystem chargingParticle;
	//[SerializeField] ParticleSystem chargingParticleB;
	[SerializeField] ParticleSystem chargedParticle;
	[SerializeField] Animator PowerExplosion;
	[SerializeField] ParticleSystem jumpParticle;
	[SerializeField] ParticleSystem landParticle;
	[SerializeField] ParticleSystem impulseParticles;
	[SerializeField] GameObject hitBox;
	[SerializeField] GameObject damageDealerBox;
	[SerializeField] GameObject swingSFX;
	[SerializeField] GameObject hitSFX;
	[SerializeField] GameObject dodgeSFX;
	[SerializeField] GameObject powerAttackChargedShadow;
	[SerializeField] SpriteRenderer raikoRenderer;
	[SerializeField] SpriteRenderer chargedShadowRenderer;
	[SerializeField] CreateShadow shadowCreator;
	[SerializeField] Transform projectileInstantiatingPoint;
	
	public SimpleAudioPlayer aPlayer;

    //////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////Preset Variables//////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////

    [Foldout("Presets", true)]
    [Space(5)]
	[Header("Preset Variables")]
	[Space(10)]
	
	[SerializeField] private LayerMask groundMask;
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float jump = 5;
    [SerializeField] private float gravity = 15;
	[SerializeField] private float dodgeCD = 1;
	[SerializeField] private float comboImpulse = 1f;
	[SerializeField] private float comboImpulseDecreaseRating = 3f;
	[SerializeField] private float dodgeImpulse = 3f;
	[SerializeField] private float dodgeTime = 0.3f;
    //[SerializeField] float chargeAttackTimer = 0;

    //////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////Intern Variables/////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////

    [Foldout("Internal", true)]
    [Space(5)]
	[Header("Intern Variables")]
	[Space(10)]

	[SerializeField] int comboChainCounter = 0;
	[SerializeField] float airTime = 0;
	[SerializeField] float yInput;
	[SerializeField] float dodgeCounter = 0;
	[SerializeField] Vector3 moveBoast;
	[SerializeField] Vector3 directionalInput;
	[SerializeField] bool subweaponAvailable = true;

	[SerializeField] GameObject subweaponProjectile = null;

	public float SpecialSpeed;

    //////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////Booleans/////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////

    [Foldout("Bools", true)]
    [Space(5)]
	[Header("Booleans")]
	[Space(10)]

	[SerializeField] bool alive = true;
	//[SerializeField] bool powerAttackOn = false;
	//[SerializeField] bool comboInput = true;
	//[SerializeField] bool comboAvailable = true;
	//[SerializeField] bool dodging = false;
	[SerializeField] bool grounded;
	[SerializeField] public bool isCharged = false;
    //[SerializeField] bool isChargingAttack = false;

    //////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////Sounds///////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////
    [Foldout("Sounds", true)]
    [SerializeField] GameObject SwordSlashSfx;
	[SerializeField] GameObject jumpSfx;
	[SerializeField] GameObject landSfx;
	[SerializeField] GameObject hurtSfx;

	[SerializeField] AudioClip chargingClip, chargeCompleteClip, chargedClip, pAttackClip;
	[SerializeField] AudioSource aSource, chargingSound;



//////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////Coroutines////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////

	private IEnumerator boostCoroutine;
	private IEnumerator iFrameCoroutine;
	private IEnumerator attackInputDelay;
	public IEnumerator attackCoroutine;
	private IEnumerator dodgeCoroutine;
	private IEnumerator chargingCoroutine;
	
//////////////////////////////////////////////////////////////////////////////////
////////////////////////////////Runtime Coroutines////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////

	void OnEnable()
	{	
		//PlayerStatus.Instance.OnDeath += Died;
		//PlayerStatus.Instance.OnDamage += TookDamage;

		PlayerStats.instance.OnDamage += TookDamage;
		PlayerStats.instance.OnDeath += Death;
		InputManager.OnPressX += AttackInputDown;
        InputManager.OnPressY += SubweaponInputDown;
        InputManager.OnReleaseX += AvaliatePowerAttack;
		InputManager.OnPressA += JumpInputDown;
		InputManager.OnReleaseA += JumpInputUp;
		InputManager.OnPressRT += DodgeInputDown;
	}

	void OnDisable()
	{
		if(PlayerStats.instance != null)
		{
			PlayerStats.instance.OnDamage -= TookDamage;
			PlayerStats.instance.OnDeath -= Death;
		}
		InputManager.OnPressX -= AttackInputDown;
        InputManager.OnPressY -= SubweaponInputDown;
        InputManager.OnReleaseX -= AvaliatePowerAttack;
		InputManager.OnPressA -= JumpInputDown;
		InputManager.OnPressRT -= DodgeInputDown;
	}
	void Update()
    {
		if(GameManager.gameState != GameState.Playing)
			return;

		if(isCharged)
		{
			chargedShadowRenderer.sprite = raikoRenderer.sprite;
		}

		if (Time.timeScale > 0) 
		{
			if (CheckGrounded()) 
			{
				if (!grounded) 
				{
					if(moveState != MoveState.Blocked)
						moveState = MoveState.Enabled;

					aPlayer.PlayAudio(landSfx);
					landParticle.Play();
					if(controllerState != ControllerState.Death)
					{
						if(jumpState != JumpState.JumpLocked)
							jumpState = JumpState.None;
						if(attackState != AttackState.AttackBlocked)
							attackState = AttackState.None;
						dodgeCounter = 0;
						animator.SetBool("Grounded", true);
						animator.Play("No Combo", 1);
					}
					else
					{
						animator.Play("Death_2", 3);
					}
				}
                else
                {
                    //if (jumpState == JumpState.Jumping)
                        //jumpState = JumpState.None;
                }
                grounded = true;
			}
			else 
			{
				grounded = false;
				animator.SetBool("Grounded", false);
				jumpState = jumpState != JumpState.NoGravity ? JumpState.Jumping : jumpState;
			}
			if(jumpState != JumpState.NoGravity)
				yInput -= gravity*Time.deltaTime*3.5f;
			else
				yInput = 0;
		}
		else
			return;
		if(yInput < -2.25f*3.5f)
			yInput = -2.25f*3.5f;


		if (moveState == MoveState.Enabled && controllerState != ControllerState.Death)
        {

			directionalInput = GetDirectionalInput();
			if(directionalInput.x > 0)
			{
				transform.localScale = new Vector3(1,1,1);
				if(attackCoroutine != null)
				{
					if(grounded)
					{
						StopCoroutine(attackCoroutine);
						attackCoroutine = null;
						ComboOutput();
					}
				}
			}
			else if (directionalInput.x < 0)
			{
				transform.localScale = new Vector3(-1,1,1);
				if(attackCoroutine != null)
				{
					if(grounded)
					{
						StopCoroutine(attackCoroutine);
						attackCoroutine = null;
						ComboOutput();
					}
				}
			}
			directionalInput.y = yInput;
			if(InputManager.lAxis.y < -0.4f)
			{
				animator.SetFloat("Crouch", 1);
			}
			else
			{
				animator.SetFloat("Crouch", 0);
			}
        }
		else{
			directionalInput = new Vector3(0, yInput, 0);
		}
		directionalInput.x = directionalInput.x * speed;
		body.Move((directionalInput + (moveBoast*speed)) * Time.deltaTime);

		moveBoast = Vector3.zero;
		if(dodgeCounter > 0 && grounded)
			dodgeCounter -= Time.deltaTime;

		
		if(attackState == AttackState.WantToAttack)
		{
			if(dodgeState != DodgeState.Dodging)
			{
				if(attackType == AttackType.Normal)
				{
					Attack();
				}
			}
		}
        else if (attackState == AttackState.WantToAttackSub)
        {
            if (dodgeState != DodgeState.Dodging)
            {
                if (attackType == AttackType.Normal)
                {
                    ComboInput();
                    SubAttack();
                }
            }
        }
    }
	void LateUpdate () 
	{
		if(GameManager.gameState != GameState.Playing)
			return;

		if(!grounded)
		{
			animator.SetFloat("YSpeed", body.velocity.y);
			if((body.collisionFlags & CollisionFlags.Above) != 0)
			{
				if(yInput > 0)
					yInput = -0.2f;
			}
		}
		else
			animator.SetFloat("YSpeed", 0);

		transform.position = new Vector3(transform.position.x, transform.position.y, 0.3f);

		if (dodgeState == DodgeState.WantToDodge)
        {
            dodgeCounter = dodgeCD;
            PlaySFX(dodgeSFX);
            hitBox.SetActive(false);
            if (grounded)
                impulseParticles.Play(true);
            attackState = AttackState.None;
            if (InputManager.lAxis.x > InputManager.singleton.deadZone)//Direita
            {
                Dodge(dodgeImpulse * Vector3.right, comboImpulseDecreaseRating / 1.25f);
                animator.SetBool("DodgingBackwards", false);
            }
            if (InputManager.lAxis.x < -InputManager.singleton.deadZone)//Esquerda
            {
                Dodge(dodgeImpulse * Vector3.left, comboImpulseDecreaseRating / 1.25f);
                animator.SetBool("DodgingBackwards", false);
            }
            else if (InputManager.lAxis.x == 0) //Nenhumm
            {
                animator.SetBool("DodgingBackwards", true);
                if (body.transform.localScale == new Vector3(-1, 1, 1))
                {
                    Dodge(dodgeImpulse * Vector3.right, comboImpulseDecreaseRating / 1.25f);
                }
                else
                {
                    Dodge(dodgeImpulse * Vector3.left, comboImpulseDecreaseRating / 1.25f);
                }
            }
        }

        if (attackState != AttackState.Attacking)
		{
			if(jumpState == JumpState.WantToJump && attackState == AttackState.None)
			{
				jumpParticle.Play();
				aPlayer.PlayAudio(jumpSfx);
				yInput = jump*3.5f;
				if(moveState != MoveState.Blocked)
				{
					//moveState = MoveState.Enabled;
				}
				comboChainCounter = 0;
				animator.Play("No Combo", 1);
				if(jumpState != JumpState.JumpLocked)
				{
					jumpState = JumpState.Jumping;
				}
				impulseParticles.Stop();
			}
		}

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Raiko@DashBackward") && moveState==MoveState.Enabled)
		{
			if (InputManager.lAxis.x > InputManager.singleton.deadZone)//Direita
			{
				//Dodge (dodgeImpulse * Vector3.right, comboImpulseDecreaseRating / 1.25f);
				animator.Play("Raiko@DashForward", 0);
			}
			if (InputManager.lAxis.x < -InputManager.singleton.deadZone)//Esquerda
			{             
				//Dodge (dodgeImpulse * Vector3.left, comboImpulseDecreaseRating / 1.25f);
				animator.Play("Raiko@DashForward", 0);
			}
		}
		
		if(Mathf.Abs(directionalInput.x) > 0)
			animator.SetBool("Idle", false);
		else
			animator.SetBool("Idle", true);
		
	}

	//////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////Input Events//////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////

	void AttackInputDown()
    {
		if(GameManager.gameState != GameState.Playing || controllerState != ControllerState.Enabled)
			return;
        attackType = AttackType.Normal;
		if(attackState == AttackState.None)
       		attackState = AttackState.WantToAttack;
		else
		{
			if (attackInputDelay != null)
				StopCoroutine(attackInputDelay);
			attackInputDelay = _ChainCombo();
			StartCoroutine(attackInputDelay);
		}
		if(PlayerStats.instance.skillFlags.havePowerAttack)
		{
			if(chargingCoroutine != null)
				StopCoroutine(chargingCoroutine);
			chargingCoroutine = _ChargeUp();
			StartCoroutine(chargingCoroutine);
		}
		
    }
    void SubweaponInputDown()
    {
        if (GameManager.gameState != GameState.Playing || controllerState != ControllerState.Enabled || !subweaponAvailable)
            return;
        attackType = AttackType.Normal;
        if (attackState == AttackState.None)
            attackState = AttackState.WantToAttackSub;
        else
        {
            if (attackInputDelay != null)
                StopCoroutine(attackInputDelay);
            attackInputDelay = _ChainSub();
            StartCoroutine(attackInputDelay);
        }
    }
    void DodgeInputDown()
    {
		if(GameManager.gameState != GameState.Playing)
			return;
		if(controllerState != ControllerState.Enabled)
			return;

		if(dodgeState == DodgeState.Attacking)
		{
			if(dodgeCoroutine == null)
			{
				dodgeCoroutine = _DodgeWhileAttack();
				StartCoroutine(dodgeCoroutine);
			}
		}

		else  if (dodgeCounter <= 0) 
		{
			if (dodgeStyle == DodgeStyle.OnlyGrounded && grounded)
        	{
                dodgeState = DodgeState.WantToDodge;
            }
			else if(dodgeStyle == DodgeStyle.OnAir)
			{
           		dodgeState = DodgeState.WantToDodge;
			}
        }
    }
	void DodgeInputUp()
    {
		if(GameManager.gameState != GameState.Playing)
			return;
		if(controllerState != ControllerState.Enabled)
			return;
        
		if(dodgeState == DodgeState.Dodging)
		{	
			//Interrupt Dodge here!!! <---------------
		}
    }
	void JumpInputDown()
	{
		if(GameManager.gameState != GameState.Playing)
			return;
		if(controllerState != ControllerState.Enabled)
			return;
		if(grounded && jumpState == JumpState.None){
			if(InputManager.lAxis.y < -0.95f)
			{
				GameObject platform = DetectUnderPlatform();
				if(platform != null)
					StartCoroutine(_disablePlatform(platform));
			}
			else
				jumpState = JumpState.WantToJump;
		}
	}
	void JumpInputUp()
	{
		if(GameManager.gameState != GameState.Playing)
			return;
		if(controllerState != ControllerState.Enabled)
			return;
		//if(!grounded)
		//{
			if(yInput > 0)
				yInput /= 1.5f;
		//}
	}
	Vector3 GetDirectionalInput()
	{
		Vector3 horizontalMovement = new Vector3(InputManager.lAxis.x, 0, 0);
		if(horizontalMovement.x > 0.5f)
			horizontalMovement.x = 1;
		else if(horizontalMovement.x < -0.5f)
			horizontalMovement.x = -1;
		else
			horizontalMovement.x = 0;
		//horizontalMovement = Vector3.ClampMagnitude(horizontalMovement, 1);
		return horizontalMovement;
	}
    
	//////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////Damage Events/////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////
 	
	void TakeHit(float damage)
	{
		if(iFrameCoroutine == null)
		{
			iFrameCoroutine = _DisableHitbox(1f);
			StartCoroutine(iFrameCoroutine);
		}	
	}
	public void TookDamage(GameObject instigator, DamageInfo damage, Stats attacker)
	{
		animator.SetTrigger("Damage");
		TakeHit(damage.ammount);
	}
	public void Death(GameObject instigator, DamageInfo damage, Stats attacker)
    {
		powerAttackChargedShadow.SetActive(false);	
		chargingParticle.Stop(true);
		chargingParticle.GetComponentInChildren<ParticleSystem>().Clear();
		chargedParticle.Stop(true);
		if(chargingCoroutine != null)
			StopCoroutine(chargingCoroutine);
		Time.timeScale = 0.3f;
		yInput = jump/2;
		animator.Play("Death_1", 3);
		alive = false;
		controllerState = ControllerState.Death;
		attackState = AttackState.None;
		jumpState = JumpState.None;
		//animator.SetTrigger("Death");
		Debug.Log("Died to: " + instigator.name);
		//GetComponent<CharacterController> ().enabled = false;
    }
	public void Revive(){
		alive = true;
		controllerState = ControllerState.Enabled;
		moveState = MoveState.Enabled;
		attackState = AttackState.None;
		jumpState = JumpState.None;
		chargingSound.Stop();
		isCharged = false;
		PlayerStatsController.instance.stats.damage.charged = false;
		//isChargingAttack = false;
		chargedParticle.Stop ();
		chargingParticle.Stop ();
		GetComponent<CharacterController> ().enabled = true;
		RecoverInput ();
		//RecoverCombo ();
		hitBox.SetActive(true);
		animator.Play("Null", 3);
		animator.Play("Idle", 0);
		animator.SetBool("Grounded", true);
		//animator.SetTrigger ("Restore");
	}

	//////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////Combo Methods/////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////
	void ComboInput()
	{
		//aPlayer.PlayAudio(swingSFX);
		if(grounded && moveState != MoveState.Blocked)
			moveState = MoveState.Attacking;
		if(attackState != AttackState.AttackBlocked)
			attackState = AttackState.WillAttack;
	}
	public void ComboOutput()
	{
		comboChainCounter = 0;
		animator.Play("No Combo", 1);
		
		if(moveState == MoveState.Attacking)
			moveState = MoveState.Enabled;
		if(attackState != AttackState.AttackBlocked)
			attackState = AttackState.None;
	}
	void PickComboFromSequence(ComboSetup combo)
	{
		if(attackCoroutine != null)
			StopCoroutine(attackCoroutine);
		// if somehow the chain overflow the sequence, we reset the sequence
		if(comboChainCounter >= combo.sequence.Length)
			comboChainCounter = 0;
		attackCoroutine = _Combo(combo.sequence[comboChainCounter]);
		StartCoroutine(attackCoroutine);
		if(comboChainCounter+1 >= combo.sequence.Length)
			comboChainCounter = 0;
		else
			comboChainCounter++;
	}

	//////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////Ground Methods////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////
	public bool CheckGrounded(){
		if (Physics.SphereCast (new Ray (transform.position + body.center, Vector3.down), body.radius, (body.height / 2) - (body.radius*1f) + (body.radius-0.1f), groundMask)) {
			return true;
		} 
		else {
			airTime += Time.deltaTime;
			return false;
		}

	}
	float groundDistance(){
		RaycastHit hit = new RaycastHit ();
		if (Physics.SphereCast (new Ray (transform.position + body.center, Vector3.down), body.radius - 0.02f, out hit, 5, groundMask)) {
			return hit.distance;
		}
		else{
			return 5;
		}
	}

	//////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////External Methods//////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////

	public void BlockInput(){
		
	}
	public void RecoverInput(){
		
	}
	void BlockAttack(){
		attackState = AttackState.AttackBlocked;
	}
	public void RecoverAttack(){
		attackState = AttackState.None;
	}

	//////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////Move Methods//////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////

	void MoveBoast(Vector3 impulse, float decreaseRating)
	{
		if(boostCoroutine != null)
			StopCoroutine(boostCoroutine);
		StartCoroutine(_MoveBoast(impulse, decreaseRating));
	}
	void Dodge(Vector3 impulse, float decreaseRating)
	{
		if(DodgeReactor.OnDodgeStart != null)
			DodgeReactor.OnDodgeStart();
		aPlayer.PlayAudio(dodgeSFX);
		dodgeState = DodgeState.Dodging;
		moveState = MoveState.Dodging;
		//boostCoroutine = (_Dodge(impulse, decreaseRating));
		boostCoroutine = (_Dodge(impulse, dodgeTime));
		StartCoroutine(boostCoroutine);
	}
	IEnumerator _MoveBoast(Vector3 impulse, float decreaseRating)
	{
		//dodging = true;
		float shadowTimer = 0;
		while(impulse.magnitude > 0.5f){
			while(GameManager.gameState != GameState.Playing || Time.timeScale == 0)
			{
				yield return null;
			}
			shadowTimer += 5*Time.deltaTime;
			if(shadowTimer >= 0.4f/impulse.magnitude){
				//shadowCreator.InstantiateShadow(this.transform);
				shadowTimer = 0;
			}

			//directionalInput.y = directionalInput.y / 2;
			directionalInput.x = 0;
			hitBox.SetActive (false);
			impulse = Vector3.Lerp(impulse, Vector3.zero, decreaseRating*Time.deltaTime);
			moveBoast += impulse;
			yield return null;
		}
	    if (animator.GetBool("Dodging"))
        {
			//animator.SetBool("Dodging", false);
            //print("SIT1");
		}
		if(alive && iFrameCoroutine == null)
			hitBox.SetActive (true);
		//dodging = false;

	}

	IEnumerator _Dodge(Vector3 impulse, float dodgeTime)
	{
		float startDodgeTime = dodgeTime;
		//dodging = true;
		animator.SetBool ("Dodging", true);
		animator.Play("No Combo", 1);
		animator.SetBool("Null", true);
		float shadowTimer = 0;
		while(dodgeTime > 0.0f){
			while(GameManager.gameState != GameState.Playing || Time.timeScale == 0)
			{
				yield return null;
			}
			if(shadowTimer >= 0.3f && dodgeTime < startDodgeTime-0.06f){
				shadowCreator.InstantiateShadow(this.transform, raikoRenderer.sprite);
				shadowTimer = 0;
			}
			shadowTimer += 8*Time.deltaTime;
			directionalInput.x = 0;
			hitBox.SetActive (false);
			moveBoast += impulse;
			dodgeTime -= Time.deltaTime;
			yield return null;
		}
		while(!CheckGrounded())
		{
			speed = 4.5f;
			if(dodgeState != DodgeState.DodgeLocked)
			{
				dodgeState = DodgeState.None;
				//animator.SetBool("Dodging", false);
			}	
			yield return null;
			if(moveState == MoveState.Dodging)
				moveState = MoveState.Enabled;
		}
		//dodging = false;
		speed = 3.5f;
        if (animator.GetBool("Dodging"))
        {
            animator.SetBool("Null", true);
            if (animator.GetBool("Null"))
            {
                ComboOutput();
            }
            animator.SetBool("Dodging", false);

        }
        else
        {
            ComboOutput();
        }
		if(alive && iFrameCoroutine == null)
			hitBox.SetActive (true);
		
		if(DodgeReactor.OnDodgeEnd != null)
			DodgeReactor.OnDodgeEnd();

		if(dodgeState != DodgeState.DodgeLocked)
			dodgeState = DodgeState.None;
		if(moveState == MoveState.Dodging)
			moveState = MoveState.Enabled;
	}	



	public void PlaySFX(GameObject sfx){
		//sfxPlayer.PlaySoundFX(sfx, transform.position+body.center);
	}

	GameObject DetectUnderPlatform(){
		Vector3 rayCastPoint = transform.position + body.center;
		Ray ray = new Ray (rayCastPoint, Vector3.down);
		RaycastHit hit = new RaycastHit ();
		if (Physics.Raycast (ray, out hit, body.height + 0.3f, 1<<20)) {
			if(hit.transform.tag == "Platform")
				return hit.transform.gameObject;
		} 

		rayCastPoint.x += body.radius;

		ray.origin = rayCastPoint;

		if (Physics.Raycast (ray, out hit, body.height + 0.3f, 1<<20)) {
			if(hit.transform.tag == "Platform")
				return hit.transform.gameObject;
		} 

		rayCastPoint.x -= body.radius*2;

		ray.origin = rayCastPoint;

		if (Physics.Raycast (ray, out hit, body.height + 0.3f, 1<<20)) {
			if(hit.transform.tag == "Platform")
				return hit.transform.gameObject;
		} 

		else {
			return null;
		}
		return null;
	}

	public void EnablePowerAttack(){
		PlayerStats.instance.skillFlags.havePowerAttack = true;
	}
	public void DissablePowerAttack(){
		PlayerStats.instance.skillFlags.havePowerAttack = false;
	}
	public void interruptPowerAttack(){
		chargingParticle.Stop ();
		chargedParticle.Stop ();
		isCharged = false;
		chargingSound.Stop();
		PlayerStatsController.instance.stats.damage.charged = false;
		//isChargingAttack = false;
		powerAttackChargedShadow.SetActive(false);
	}

	public void DisableController(){
		controllerState = ControllerState.Disabled;
		interruptPowerAttack ();
	}
	public void EnableController(){
		controllerState = ControllerState.Enabled;
	}

    void Attack()
    {
		chargingSound.Stop();
		isCharged = false;
		PlayerStatsController.instance.stats.damage.charged = false;
		if(InputManager.lAxis.y < -0.95f && grounded && jumpState != JumpState.Jumping)
		{
			PickComboFromSequence(crouchingComboSequence);
		}
		else if (grounded && jumpState != JumpState.Jumping)
		{
			if (InputManager.lAxis.x > InputManager.singleton.deadZone)
			{
				MoveBoast(Vector3.right * comboImpulse*1.5f, comboImpulseDecreaseRating); 
				body.transform.localScale = new Vector3(1, 1, 1);
				impulseParticles.Play();
			}
			else if (InputManager.lAxis.x < -InputManager.singleton.deadZone)
			{
				MoveBoast(Vector3.left * comboImpulse*1.5f, comboImpulseDecreaseRating); 
				body.transform.localScale = new Vector3(-1, 1, 1);
				impulseParticles.Play();	
			}
			if(dodgeState != DodgeState.DodgeLocked)
				dodgeState = DodgeState.Attacking;
			PickComboFromSequence(groundedComboSequence);
				
		}
		else
		{
			// Air attack here <-------------------------------
			if(body.velocity.x != 0)
				PickComboFromSequence(airComboSequenceMoving);
			else
				PickComboFromSequence(airComboSequence);
		}
    }

    void SubAttack()
    {
		if(!subweaponAvailable)
			return;

		StartCoroutine(_SubCooldown(PlayerStatsController.instance.currentSubweapon.cooldownTime));

        chargingSound.Stop();
        isCharged = false;
        PlayerStatsController.instance.stats.damage.charged = false;

		//Call subweapon Crouching
        if (InputManager.lAxis.y < -0.95f && grounded && jumpState != JumpState.Jumping)
        {
           PlayerStatsController.instance.currentSubweapon.EvaluateCombo(this, ()=>ComboOutput(), ComboState.Crouching);
        }
		//Call subweapon grounded
        else if (grounded && jumpState != JumpState.Jumping)
        {
            if (InputManager.lAxis.x > InputManager.singleton.deadZone)
            {
                MoveBoast(Vector3.right * comboImpulse * 1.5f, comboImpulseDecreaseRating);
                body.transform.localScale = new Vector3(1, 1, 1);
                impulseParticles.Play();
                PlayerStatsController.instance.currentSubweapon.EvaluateCombo(this, () => ComboOutput(), ComboState.Grounded);
            }
            else if (InputManager.lAxis.x < -InputManager.singleton.deadZone)
            {
                MoveBoast(Vector3.left * comboImpulse * 1.5f, comboImpulseDecreaseRating);
                body.transform.localScale = new Vector3(-1, 1, 1);
                impulseParticles.Play();
                PlayerStatsController.instance.currentSubweapon.EvaluateCombo(this, () => ComboOutput(), ComboState.Grounded);
            }
            else
            {
                PlayerStatsController.instance.currentSubweapon.EvaluateCombo(this, () => ComboOutput(), ComboState.Grounded);
            }
            if (dodgeState != DodgeState.DodgeLocked)
                dodgeState = DodgeState.Attacking;
        }
		//Call subweapon air
        else
        {
            // Air attack here <-------------------------------
            if (body.velocity.x != 0)
            {
                //Call subweapon AirMoving
                PlayerStatsController.instance.currentSubweapon.EvaluateCombo(this, () => ComboOutput(), ComboState.AirMoving);
            }
            else
            {
                //Call subweapon Air
                PlayerStatsController.instance.currentSubweapon.EvaluateCombo(this, () => ComboOutput(), ComboState.Air);
            }
        }
    }

	void PowerAttack()
	{
		aSource.PlayOneShot(pAttackClip);
		isCharged = false;
		PlayerStatsController.instance.stats.damage.charged = true;
		powerAttackChargedShadow.SetActive(false);
		if(attackCoroutine != null)
				StopCoroutine(attackCoroutine);
		if(InputManager.lAxis.y < -0.95f && grounded && jumpState != JumpState.Jumping)
		{
			attackCoroutine = _Combo(powerAttacks.sequence[0]);  //-> 0 = crouching power attack
		}
		else if (grounded && jumpState != JumpState.Jumping)
		{
			//print("unleashing attack");
			if (InputManager.lAxis.x > InputManager.singleton.deadZone)
			{
				MoveBoast(Vector3.right * comboImpulse*1.5f, comboImpulseDecreaseRating); 
				body.transform.localScale = new Vector3(1, 1, 1);
				impulseParticles.Play();
			}
			else if (InputManager.lAxis.x < -InputManager.singleton.deadZone)
			{
				MoveBoast(Vector3.left * comboImpulse*1.5f, comboImpulseDecreaseRating); 
				body.transform.localScale = new Vector3(-1, 1, 1);
				impulseParticles.Play();	
			}
			if(dodgeState != DodgeState.DodgeLocked)
				dodgeState = DodgeState.Attacking;

			attackCoroutine = _Combo(powerAttacks.sequence[1]); //-> 1 = grounded power attack
				
		}
		else
		{
			// Air attack here <-------------------------------
			//animator.SetTrigger("Attack");
			//animator.SetBool("Null", false);
			if(body.velocity.x != 0)
				attackCoroutine = _Combo(powerAttacks.sequence[2]); //-> 2 = air power attack
			else
				attackCoroutine = _Combo(powerAttacks.sequence[3]); //-> 3 =  air power attack forward
		}
		StartCoroutine(attackCoroutine);
    }


	IEnumerator _SubCooldown(float totalTime)
	{
		float currentTime = 0;
		subweaponAvailable = false;
		while(currentTime < totalTime)
		{
			HealthGuiManager.instance.SetSubweaponTimer(currentTime, totalTime);
			currentTime += Time.deltaTime;
			yield return null;
		}
		subweaponAvailable = true;
		HealthGuiManager.instance.SetSubweaponTimer(totalTime, totalTime);	
	}

	IEnumerator _Combo(Combo comboData)
	{
		ComboInput();
        aPlayer.PlayAudio(swingSFX);
        animator.Play(comboData.animationName, 1);
		yield return new WaitForSeconds(comboData.minTime);
		if(dodgeState == DodgeState.Attacking)
			dodgeState = DodgeState.None;
		attackState = AttackState.Attacking;
		yield return new WaitForSeconds(comboData.nextChain/SpecialSpeed - comboData.minTime/SpecialSpeed);
		attackState = AttackState.None;
		yield return new WaitForSeconds(comboData.maxTime/SpecialSpeed - comboData.nextChain/SpecialSpeed - comboData.minTime/SpecialSpeed);
		if(moveState == MoveState.Attacking)
			moveState = MoveState.Enabled;
		ComboOutput();
	}
	IEnumerator _PowerAttack(Combo comboData)
	{
		ComboInput();
		animator.Play(comboData.animationName, 1);
		yield return new WaitForSeconds(comboData.minTime);
		if(dodgeState == DodgeState.Attacking)
			dodgeState = DodgeState.None;
		attackState = AttackState.Attacking;
		yield return new WaitForSeconds(comboData.nextChain - comboData.minTime);
		attackState = AttackState.None;
		yield return new WaitForSeconds(comboData.maxTime - comboData.nextChain - comboData.minTime);
		if(moveState == MoveState.Attacking)
			moveState = MoveState.Enabled;
		ComboOutput();
	}
	IEnumerator _ChainCombo()
	{
		float combotimer = 0;
		while(attackState != AttackState.None)
		{
			yield return null;
			combotimer += Time.deltaTime;
		}
		if(combotimer < 0.18f)
		{
			attackState = AttackState.WantToAttack;
			attackInputDelay = null;
		}
		
	}
    IEnumerator _ChainSub()
    {
        float combotimer = 0;
        while (attackState != AttackState.None)
        {
            yield return null;
            combotimer += Time.deltaTime;
        }
        if (combotimer < 0.18f)
        {
            attackState = AttackState.WantToAttackSub;
            attackInputDelay = null;
        }

    }

    IEnumerator _DisableHitbox(float time){
		hitBox.SetActive(false);
		moveState = MoveState.Damage;
		yield return new WaitForSeconds(time*0.2f);
		if(moveState == MoveState.Damage)
			moveState = MoveState.Enabled;
		yield return new WaitForSeconds(time*0.8f);
		if(alive)
			hitBox.SetActive(true);
		iFrameCoroutine = null;
	}

	IEnumerator _ModifyDamageDealerSize(float time, float size){
		//Time.timeScale = 0.2f;
		damageDealerBox.transform.localScale = new Vector3 (size, size, size);
		yield return new WaitForSeconds(time);
		//Time.timeScale = 1f;
		damageDealerBox.transform.localScale = new Vector3 (1, 1, 1);
	}
	IEnumerator _disablePlatform(GameObject platform){
		platform.GetComponent<Collider>().enabled = false;
		yield return new WaitForSeconds (0.5f);
		//platform.GetComponent<Collider>().enabled = true;

	}


	IEnumerator _DodgeWhileAttack()
	{
        //Buffer de dodge apos atacar
		yield return new WaitForSeconds(.05f);
		if(dodgeState != DodgeState.DodgeLocked)
		{
			yield return null;
			dodgeState = DodgeState.WantToDodge;
		}
		dodgeCoroutine = null;
	}

	IEnumerator _ChargeUp()
	{
		chargingParticle.Play(true);
		yield return new WaitForSeconds(0.2f);
		chargingSound.Play();
		yield return new WaitForSeconds(0.6f);
		aSource.PlayOneShot(chargeCompleteClip);
		if(grounded)
			chargedParticle.Play();
		PowerExplosion.SetTrigger("Activate");
		isCharged = true;
		powerAttackChargedShadow.SetActive(true);

	}

	void AvaliatePowerAttack()
	{
		if(controllerState == ControllerState.Enabled)
		{
			if(!isCharged)
			{
				chargingParticle.Stop(true);
				chargingParticle.GetComponentInChildren<ParticleSystem>().Clear();
				chargedParticle.Stop(true);
				chargingSound.Stop();
				if(chargingCoroutine != null)
					StopCoroutine(chargingCoroutine);
			}
			else
			{
				PowerAttack();
			}
		}
		else
		{
			chargingParticle.Stop(true);
			chargingParticle.GetComponentInChildren<ParticleSystem>().Clear();
			chargedParticle.Stop(true);
			if(chargingCoroutine != null)
				StopCoroutine(chargingCoroutine);
		}
		

	}

	public void StartSaving()
	{
		controllerState = ControllerState.Waiting;
		moveState = MoveState.Blocked;
		attackState = AttackState.None;
		jumpState = JumpState.None;
		animator.Play("Sitting",0);
	}

	public void EndSaving()
	{
		animator.Play("Standing",0);
	}

	public void ReEnableAfterSaving()
	{
		controllerState = ControllerState.Enabled;
		moveState = MoveState.Enabled;
		attackState = AttackState.None;
		jumpState = JumpState.None;
		animator.Play("Idle",0);
	}

	public void SetupSubweaponProjectile(GameObject prefab)
	{
		subweaponProjectile = prefab;
	}

	public void FireSubweaponProjectile()
	{
		GameObject newProjectile = Instantiate(subweaponProjectile, projectileInstantiatingPoint.position, projectileInstantiatingPoint.rotation);
        newProjectile.GetComponent<Projectile>().ShootProjectile(0);
	}

}
