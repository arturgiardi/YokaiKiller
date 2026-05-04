using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour 
{
	enum DodgeDirection {Right, Left};

	private DodgeDirection dodgeDir;
	private IEnumerator boostCoroutine;

    [SerializeField]
    private CharacterController body;
	public Animator animator;
	//[SerializeField]
	//private PlaySoundEffect sfxPlayer;
    [SerializeField]
    private string jumpImput;
	[SerializeField]
	ParticleSystem chargingParticle;
	[SerializeField]
	ParticleSystem chargedParticle;
	[SerializeField]
	ParticleSystem jumpParticle;
    [SerializeField]
    GameObject jumpClip;
    [SerializeField]
	ParticleSystem landParticle;
    [SerializeField]
    GameObject landClip;

    [SerializeField]
	private LayerMask groundMask;


    [SerializeField]
    private float speed = 3.5f;
    [SerializeField]
    private float jump = 5;
    [SerializeField]
    private float gravity = 15;
	[SerializeField]
	private float dodgeCD = 1;

	[SerializeField]
	private GameObject hitBox;
	[SerializeField]
	private GameObject damageDealerBox;

	[SerializeField]
	private float comboImpulse = 1f;
	[SerializeField]
	private float comboImpulseDecreaseRating = 3f;
	[SerializeField]
	private ParticleSystem impulseParticles;

	Vector3 moveBoast;

	bool comboInput = true;
	//bool comboAvailable = true;
	bool comboTry = false;


	float comboTimer = 0;
	float chargeAttackTimer = 0;

	[SerializeField]
	GameObject swingSFX;
	[SerializeField]
	GameObject hitSFX;
	[SerializeField]
	GameObject dodgeSFX;

	[SerializeField]
    Vector3 directionalInput;
	[SerializeField]
    float yInput;

	[SerializeField]
	CreateShadow shadowCreator;

	bool hasControl = true;

	bool dodging = false;
	[SerializeField]
	bool canInput = true;
	[SerializeField]
	bool canMove = true;
	[SerializeField]
	bool canCombo= true;
	bool isChargingAttack = false;
	bool grounded;

	[SerializeField]
	float airTime = 0;
	float dodgeCounter = 0;
	float lastVel;

	public bool isCharged = false;

	[SerializeField]
	bool alive = true;


	////////////////////////////////
	/// Powers
    [SerializeField]
	bool powerAttackOn = false;



	// Use this for initialization
	void Start ()
	{
		print ("Registrando");
        //PlayerStatus.Instance.OnDeath += Died;
		//PlayerStatus.Instance.OnDamage += TookDamage;
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<DamageDealer>() != null)
        {
            //PlayerStatus.Instance.TakeDamage(collider.gameObject.GetComponent<DamageDealer>().damageAmmount, collider.gameObject);
        }
    }
 	
	void TakeHit(float damage){
		//PlayerStatus.Instance.TakeDamage(damage, this.gameObject);
		FindObjectOfType<CameraManager>().ShakeCamera(0.12f,3f);
		StartCoroutine(_DisableHitbox(0.6f));
        SoundEffectInstantiator.PlaySoundFX(hitSFX, transform.position+body.center);
		DamagePopup.InstantiateDamage (DamagePopup.DamageColor.Red, body.transform.position + body.center, damage);
	}

    //Update is called once per frame
    void Update()
    {
		if (Time.timeScale == 0)
			return;
		if(yInput < -8)
			yInput = -8;

		if (Time.timeScale > 0) {
			//if (dodging) {
				//yInput = Mathf.Lerp (directionalInput.y, -gravity, Time.deltaTime);
				//yInput = 0;
			//}
			//else 
			if (CheckGrounded()) {
				//if (body.velocity.y <= 0) {
				//yInput = Mathf.Lerp(yInput, -0.8f, 5*Time.deltaTime);
				if (yInput > -0.8f)
					yInput -= gravity * Time.deltaTime;
				else
					yInput = -0.8f;
				if (!grounded) {
					landParticle.Play();
                    SoundEffectInstantiator.PlaySoundFX(landClip, body.transform.position + body.center);
						ComboAvailable ();
						//print ("AQUI!!!");
				}
				grounded = true;
				///}
			}
			else {
				//yInput = Mathf.Lerp (yInput, -gravity, Time.deltaTime);
				if(yInput > -8f)
					yInput -= gravity * Time.deltaTime;
				grounded = false;
			}
		}
		if(hasControl)
			DodgePass ();
		AnimationPass();
		if(hasControl)
			CombatPass();

		if (canInput && hasControl)
        {
			InputPass();
            
        }
		else{
			directionalInput = new Vector3(0, yInput, 0);
		}

		if(body.collisionFlags == CollisionFlags.Above && yInput > 0.1f)
        {
			yInput = -0.1f;
        }

        if (canMove)
        {
			//print (moveBoast);
			body.Move((directionalInput + moveBoast) * speed * Time.deltaTime);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.3f);
		moveBoast = Vector3.zero;


        //Debug Load
        //if (Input.GetButtonDown("LoadDebug"))
       // {
            //PlayerStatus.Load();
        //}



		//if (body.velocity.y < -2) {
			//print ("tentando");
			//body.SimpleMove (Vector3.zero);
		//}
    }

    // Fixed Update is called once per physics iteration
    void LateUpdate () 
	{
	}

    public void Died(GameObject instigator)
    {
		alive = false;
		canInput = false;
		hasControl = false;
		animator.SetTrigger("Death");
		Debug.Log("Died to: " + instigator.name);
		GetComponent<CharacterController> ().enabled = false;

        //gameObject.SetActive(false);
    }

	public void Revive(){
		alive = true;
		canInput = true;
		hasControl = true;
		isCharged = false;
		isChargingAttack = false;
		chargedParticle.Stop ();
		chargingParticle.Stop ();
		GetComponent<CharacterController> ().enabled = true;
		RecoverInput ();
		RecoverCombo ();
		hitBox.SetActive(true);
		animator.SetTrigger ("Restore");
	}

	void ComboInput(){
		canInput = false;
	}
		

	bool CheckGrounded(){
		if (Physics.SphereCast (new Ray (transform.position + body.center, Vector3.down), body.radius - 0.00f, body.height / 2 - 0.25f + 0.16f, groundMask)) {
			//airTime = 0;
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

	//Reset triggers to make combo available
	public void ComboAvailable(){
		comboInput = true;
		//comboAvailable = true;
	}

	//Consume combo input, making combo impossible for the time
	public void ComboConsume(){
		comboInput = false;
		animator.SetBool("Null", false);
	}

	//Combo exit. Force return to null attack animation
	public void ComboOutput(){
		animator.SetBool("Null", true);
		canInput = true;
	}


	public void TookDamage(GameObject instigator)
	{
		//Debug.Log("Took Damage from " + instigator.name);
		//Debug.Log(PlayerStatus.Instance.CurrentHealth);
		animator.SetTrigger("Damage");
	}

	void AnimationPass(){
		//Check horizontal movement
		if (Mathf.Abs(body.velocity.x) > 0.1f){
			animator.SetBool("Idle", false);
		}
		else
		{
			animator.SetBool("Idle", true);
		}

		//Check if grounded
		if (grounded) {
			if (!animator.GetBool ("Grounded")) {
				animator.SetBool ("Grounded", true);
			}
			//Check if its possible to crouch
			if (hasControl) {
                if (Input.GetAxis ("Vertical") < -0.25f && Mathf.Abs (Input.GetAxis ("Horizontal")) < 0.25f) {
					animator.SetBool ("Down", true);
				} else {
					animator.SetBool ("Down", false);
				}
			}
		}

		else
		{
			if (airTime <= 0.15f) {
				if (groundDistance() >= 0.6f) {
					animator.SetBool ("Grounded", false);
					animator.SetBool ("Down", false);
					//Check vertical velocity to tell animator if were going up or down into mid-air
					if (body.velocity.y > 0) {
						animator.SetBool ("Rising", true);
					} 
					else if (body.velocity.y < 0) {
						animator.SetBool ("Rising", false);
					}
				}
			} 
			else {
				animator.SetBool ("Grounded", false);
				animator.SetBool ("Down", false);
				//Check vertical velocity to tell animator if were going up or down into mid-air
				if (body.velocity.y > 0) {
					animator.SetBool ("Rising", true);
				} 
				else if (body.velocity.y < 0) {
					animator.SetBool ("Rising", false);
				}
			}
		}
	}

	void CombatPass(){
		//Avoid triggering combo after long waits
		if(comboTimer > 0){
			comboTimer -= Time.deltaTime;
		}
		else{
			comboTry = false;
			if (isChargingAttack && !isCharged) {
				if (!chargingParticle.isPlaying) {
					chargingParticle.Play ();
				}
				chargeAttackTimer -= Time.deltaTime;
				if (chargeAttackTimer < 0) {
					isCharged = true;
					chargedParticle.Play();
					Debug.Log ("Charged");
				}
			}
		}
		//Get fire input
		if (Input.GetButtonDown ("Fire1") && canCombo) {
			isCharged = false;
			//StartCoroutine(
			comboTry = true;
			comboTimer = 0.05f;
			chargeAttackTimer = 1f;
			if (powerAttackOn) {
				isChargingAttack = true;
			}
		}
		//Fire combo if possible
		if(comboInput && comboTry){
			comboTry = false;
			animator.SetTrigger("Attack");
			if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.25f && grounded){
				
				if(Input.GetAxis("Horizontal") > 0){
					//impulseParticles.transform.forward = Vector3.left;
					if(!dodging)
						MoveBoast(Vector3.right * comboImpulse, comboImpulseDecreaseRating); 
					body.transform.localScale = new Vector3(1, 1, 1);
				}
				else{
					//impulseParticles.transform.forward = Vector3.right;
					if(!dodging)
						MoveBoast(Vector3.left * comboImpulse, comboImpulseDecreaseRating); 
					body.transform.localScale = new Vector3(-1, 1, 1);
				}
				impulseParticles.Play();
			}
			animator.SetBool("Null", false);
		}

		if (Input.GetButtonUp ("Fire1")) {
			isChargingAttack = false;
			chargingParticle.Stop ();
			chargedParticle.Stop ();
			chargedParticle.Clear ();
            if (isCharged && canCombo && !dodging)
            {
                //if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.25f){
                //StartCoroutine(_DisableHitbox(0.25f));
                //StartCoroutine(_ModifyDamageDealerSize(0.25f, 2f));
                //Charged attack dash
                //if(Input.GetAxis("Horizontal") > 0){
                //impulseParticles.transform.forward = Vector3.left;
                //MoveBoast(Vector3.right * 6 *comboImpulse, comboImpulseDecreaseRating/1.5f); 
                //}
                //else{
                //impulseParticles.transform.forward = Vector3.right;
                //MoveBoast(Vector3.left * 6 *comboImpulse, comboImpulseDecreaseRating/1.5f); 
                //}
                //if(grounded)
                //impulseParticles.Play();
					
                //}

				FindObjectOfType<CameraManager>().ShakeCamera(0.12f, 3f);
                animator.SetTrigger("Attack");
                //sfxPlayer.PlaySoundFX(swingSFX, transform.position+body.center);
                animator.SetBool("Null", false);
            }
            else if (isCharged && canCombo && dodging)
            {
                StartCoroutine(_ModifyDamageDealerSize(0.25f, 2f));
                animator.SetLayerWeight(1, 1);
                animator.SetBool ("Dodging", false);
				FindObjectOfType<CameraManager>().ShakeCamera(0.12f, 3f);
                animator.SetTrigger("Attack");
                //sfxPlayer.PlaySoundFX(swingSFX, transform.position+body.center);
                animator.SetBool("Null", false);
				if(dodgeDir == DodgeDirection.Left)
					MoveBoast(Vector3.left * 7 *comboImpulse, comboImpulseDecreaseRating/1.5f); 
				if(dodgeDir == DodgeDirection.Right)
					MoveBoast(Vector3.right * 7 *comboImpulse, comboImpulseDecreaseRating/1.5f); 
            }
		}
	}

	void InputPass(){
		if(Input.GetButtonUp(jumpImput) && !grounded && yInput > 0){
			yInput = yInput/2;
		}
		if (!dodging) {
			if (Input.GetAxis ("Horizontal") > 0.25f) {
				directionalInput = new Vector3 (1, yInput, 0);
				body.transform.localScale = new Vector3 (1, 1, 1);
			} else if (Input.GetAxis ("Horizontal") < -0.25f) {
				directionalInput = new Vector3 (-1, yInput, 0);
				body.transform.localScale = new Vector3 (-1, 1, 1);
			} else {
				directionalInput = new Vector3 (0, yInput, 0);
			}
		}
		else {
			directionalInput = new Vector3 (0, yInput, 0);
		}
		if (Input.GetAxis ("Vertical") < -0.25f && Mathf.Abs (Input.GetAxis ("Horizontal")) < 0.25f) {
			//Disable platform under character so you can drop from it
			if (Input.GetButtonDown (jumpImput) && grounded) {
				GameObject detectedPlatform = DetectUnderPlatform ();
				print("Tried to detect: " + detectedPlatform.name);
				if (detectedPlatform != null) {
					StartCoroutine (_disablePlatform (detectedPlatform));
				}
			}
		} 
		else {
			if (Input.GetButtonDown(jumpImput) && grounded)
			{
				airTime = 1;
				grounded = false;
				yInput= jump;
				jumpParticle.Play();
                SoundEffectInstantiator.PlaySoundFX(jumpClip, body.transform.position + body.center);
            }
		}



	}

	void DodgePass(){
        
		if (dodgeCounter <= 0 && grounded) {
            if (Input.GetButtonDown("RightTrigger") && canCombo)
            {
				dodgeDir = DodgeDirection.Right;
                hitBox.SetActive (false);
                PlaySFX (dodgeSFX);
                dodgeCounter = dodgeCD;
                impulseParticles.Play (true);
				Dodge (comboImpulse * Vector3.right * 3.5f, comboImpulseDecreaseRating / 1.6f);
            }
            else if (Input.GetButtonDown("LeftTrigger") && canCombo)
            {
				dodgeDir = DodgeDirection.Left;
                hitBox.SetActive (false);
                PlaySFX (dodgeSFX);
                dodgeCounter = dodgeCD;
                impulseParticles.Play (true);
				Dodge (comboImpulse * Vector3.left * 3.5f, comboImpulseDecreaseRating / 1.6f);
            }
			else if (Input.GetButtonDown ("Dodge") && canCombo) {
				hitBox.SetActive (false);
				PlaySFX (dodgeSFX);
				dodgeCounter = dodgeCD;
				impulseParticles.Play (true);
				if (Input.GetAxis ("Horizontal") > 0.25f) {
					dodgeDir = DodgeDirection.Right;
					Dodge (comboImpulse * Vector3.right * 3.5f, comboImpulseDecreaseRating / 1.6f);
					if (animator.GetBool ("Null"))
						body.transform.localScale = new Vector3 (1, 1, 1);


				} 
				else if (Input.GetAxis ("Horizontal") < -0.25f) {
					dodgeDir = DodgeDirection.Left;
					Dodge (comboImpulse * Vector3.left * 3.5f, comboImpulseDecreaseRating / 1.6f);
					if (animator.GetBool ("Null"))
						body.transform.localScale = new Vector3 (-1, 1, 1);
				} 
				else {
					if(body.transform.localScale == new Vector3 (-1, 1, 1)){
						dodgeDir = DodgeDirection.Right;
						Dodge (comboImpulse * Vector3.right * 3.5f, comboImpulseDecreaseRating / 1.6f);
					}
					else{
						dodgeDir = DodgeDirection.Left;
						Dodge (comboImpulse * Vector3.left * 3.5f, comboImpulseDecreaseRating / 1.6f);
					}
				}
			}
		} else {
			dodgeCounter -= Time.deltaTime;
			if (Input.GetButtonUp ("Dodge")) 
			{
				StopCoroutine (boostCoroutine);
				ComboAvailable();
				ComboOutput();
				animator.SetBool("Dodging", false);
				dodging = false;
				animator.SetBool("Null", true);
				animator.SetLayerWeight(1, 1);
				hitBox.SetActive (true);
			}

		}
	}

	public void BlockInput(){
		canInput = false;
	}
		
	public void RecoverInput(){
		canInput = true;
	}

	void BlockCombo(){
		canCombo= false;
	}

	public void RecoverCombo(){
		canCombo= true;
	}

	void MoveBoast(Vector3 impulse, float decreaseRating){
		if(boostCoroutine != null)
			StopCoroutine(boostCoroutine);
		StartCoroutine(_MoveBoast(impulse, decreaseRating));
	}

	void Dodge(Vector3 impulse, float decreaseRating){
		boostCoroutine = (_Dodge(impulse, decreaseRating));
		StartCoroutine(boostCoroutine);
	}

	IEnumerator _MoveBoast(Vector3 impulse, float decreaseRating){
		dodging = true;
		float shadowTimer = 0;
		while(impulse.magnitude > 0.5f){
			shadowTimer += 5*Time.deltaTime;
			if(shadowTimer >= 0.4f/impulse.magnitude){
				shadowCreator.InstantiateShadow(this.transform, this.GetComponent<SpriteRenderer>().sprite);
				shadowTimer = 0;
			}

			//directionalInput.y = directionalInput.y / 2;
			directionalInput.x = 0;
			hitBox.SetActive (false);
			impulse = Vector3.Lerp(impulse, Vector3.zero, decreaseRating*Time.deltaTime);
			moveBoast += impulse;
			yield return null;
		}
		if(alive)
			hitBox.SetActive (true);
		dodging = false;

	}

	IEnumerator _Dodge(Vector3 impulse, float decreaseRating){
		dodging = true;
		animator.SetBool ("Dodging", true);
        animator.SetLayerWeight(1, 0);
		//animator.SetBool("Null", true);
		float shadowTimer = 0;
		while(impulse.magnitude > 0.5f){
			shadowTimer += 5*Time.deltaTime;
			if(shadowTimer >= 0.4f/impulse.magnitude){
				shadowCreator.InstantiateShadow(this.transform,  this.GetComponent<SpriteRenderer>().sprite);
				shadowTimer = 0;
			}
				
			//directionalInput.y = directionalInput.y / 2;
			directionalInput.x = 0;
			hitBox.SetActive (false);
			impulse = Vector3.Lerp(impulse, Vector3.zero, decreaseRating*Time.deltaTime);
			moveBoast += impulse;
			yield return null;
		}
		animator.SetLayerWeight(1, 1);
        if (animator.GetBool("Dodging"))
        {
            animator.SetBool("Null", true);
            if (animator.GetBool("Null"))
            {
                ComboAvailable();
                ComboOutput();
            }
            animator.SetBool("Dodging", false);
        }
        else
        {
            ComboAvailable();
            ComboOutput();
        }
		if(alive)
			hitBox.SetActive (true);
		dodging = false;
	}

	IEnumerator _DisableHitbox(float time){
		hitBox.SetActive(false);

		yield return new WaitForSeconds(time);
		if(alive)
			hitBox.SetActive(true);

	}

	IEnumerator _ModifyDamageDealerSize(float time, float size){
		//Time.timeScale = 0.2f;
		damageDealerBox.transform.localScale = new Vector3 (size, size, size);
		yield return new WaitForSeconds(time);
		//Time.timeScale = 1f;
		damageDealerBox.transform.localScale = new Vector3 (1, 1, 1);
	}

	public void PlaySFX(GameObject sfx){
        SoundEffectInstantiator.PlaySoundFX(sfx, transform.position+body.center);
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

	IEnumerator _disablePlatform(GameObject platform){
		platform.GetComponent<Collider>().enabled = false;
		yield return new WaitForSeconds (0.5f);
		//platform.GetComponent<Collider>().enabled = true;

	}


	public void EnablePowerAttack(){
		powerAttackOn = true;
	}
	public void DissablePowerAttack(){
		powerAttackOn = false;
	}
	public void interruptPowerAttack(){
		chargingParticle.Stop ();
		chargedParticle.Stop ();
		isCharged = false;
		isChargingAttack = false;
	}

	public void DisableController(){
		hasControl = false;
		canInput = false;
		interruptPowerAttack ();
	}
	public void EnableController(){
		canInput = true;
		hasControl = true;
	}

}
