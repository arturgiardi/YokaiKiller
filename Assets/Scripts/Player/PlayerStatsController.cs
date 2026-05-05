using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpecialEffects
{
	public bool healthInspector = true;
}

[System.Serializable]
public class PlayerSaveData
{
	public int[] inventory;
	public int weapon;
	public int artifactSlot1 = -1;
	public int artifactSlot2 = -1;
	public int artifactSlot3 = -1;
	public int artifactSlot4 = -1;
	public int artifactSlot5 = -1;
	public int artifactSlot6 = -1;
	public int artifactSlot7 = -1;
	public int artifactSlot8 = -1;
	public int level;
	public float exp;
	public int maxHealth;
	public int power;
	public float atckMult;
	public float hAtckMult;
	public float critChan;
	public float critMult;
	public float chargedMult;
	public SkillFlags skillFlags;
	public string scene;
	public Vector3 position;

}
public class PlayerStatsController : MonoBehaviour 
{
	public enum BonusStats {Health, Power, Luck};

	public static PlayerStatsController instance;

	public Transform origin;

	public delegate void EquipEvent();
	public static EquipEvent OnEquip;

	public delegate void HitEvent();
	public static HitEvent OnLandHit;
	public PlayerStats baseStats;
	public PlayerStats stats;

	public int availableArtifactSlots = 1;
	public Item currentWeapon;
	public SubweaponBase currentSubweapon;
	public Item artifactSlot1;
	public Item artifactSlot2;
	public Item artifactSlot3;
	public Item artifactSlot4;
	public Item artifactSlot5;
	public Item artifactSlot6;
	public Item artifactSlot7;
	public Item artifactSlot8;

	public SpecialEffects specialEffects;

	public int selectedArtifactSlot;

	public PlayerSaveData saveState;

	public SimpleAudioPlayer aPlayer;
	public ParticleSystem levelUpFX;
	public ParticleSystem bloodParticles;
	public GameObject levelUpSFX;
	public GameObject damageSFX;
	public GameObject deathSFX;
	public AudioClip gameoverMusic;

	void OnDisable()
	{
	}

	void Awake()
	{
		//-> Check instance
		if(instance != null)
		{
			if(instance != this)
			{
				Destroy(this);
				return;
			}
		}
		else
			instance = this;
		
		//-> PlayerStats instance setup
		PlayerStats.Setup(baseStats);
		stats = GetCurrentStats();

		//-> External evetns registering
	
		PlayerStats.instance.OnEarnLevel += OnGetLevel;
		PlayerStats.instance.OnDamage += OnGetDamaged;
		PlayerStats.instance.OnDeath += OnDeath;

		//-> Re instantiate base stats in order to not get whiped
		DamageInfo damageInstance = Instantiate(baseStats.damage) as DamageInfo;
		DefenseInfo defenseInstance = Instantiate(baseStats.defense) as DefenseInfo;
		PlayerStats statsInstance = Instantiate(baseStats) as PlayerStats;
		statsInstance.damage = damageInstance;
		statsInstance.defense = defenseInstance;
		baseStats = statsInstance;

		
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Y))
		{
			//SaveState();
			Debug.Log(saveState.skillFlags.havePowerAttack);
		}
		if(Input.GetKeyDown(KeyCode.U))
		{
			LoadSaveState();
		}
	}

	public PlayerStats GetStatsPrediction(Item newItem)
	{
		PlayerStats returnStats = UnityEngine.Object.Instantiate(baseStats) as PlayerStats;
		returnStats.damage = UnityEngine.Object.Instantiate(baseStats.damage) as DamageInfo;
		returnStats.defense = UnityEngine.Object.Instantiate(baseStats.defense) as DefenseInfo;

		//if weapon
		if(newItem.iType == ItemType.Weapon)
			returnStats.AddEquipment(newItem);
		else
			returnStats.AddEquipment(currentWeapon);
		
		//if artifact s1
		if(newItem.iType == ItemType.Artifact && selectedArtifactSlot == 1)
			returnStats.AddEquipment(newItem);
		else
			returnStats.AddEquipment(artifactSlot1);

		//if artifact s2
		if(newItem.iType == ItemType.Artifact && selectedArtifactSlot == 2)
			returnStats.AddEquipment(newItem);
		else
			returnStats.AddEquipment(artifactSlot2);

		//if artifact s3
		if(newItem.iType == ItemType.Artifact && selectedArtifactSlot == 3)
			returnStats.AddEquipment(newItem);
		else
			returnStats.AddEquipment(artifactSlot3);

		//if artifact s4
		if(newItem.iType == ItemType.Artifact && selectedArtifactSlot == 4)
			returnStats.AddEquipment(newItem);
		else
			returnStats.AddEquipment(artifactSlot4);

		//if artifact s5
		if(newItem.iType == ItemType.Artifact && selectedArtifactSlot == 5)
			returnStats.AddEquipment(newItem);
		else
			returnStats.AddEquipment(artifactSlot5);

		//if artifact s6
		if(newItem.iType == ItemType.Artifact && selectedArtifactSlot == 6)
			returnStats.AddEquipment(newItem);
		else
			returnStats.AddEquipment(artifactSlot6);

		//if artifact s7
		if(newItem.iType == ItemType.Artifact && selectedArtifactSlot == 7)
			returnStats.AddEquipment(newItem);
		else
			returnStats.AddEquipment(artifactSlot7);

		//if artifact s8
		if(newItem.iType == ItemType.Artifact && selectedArtifactSlot == 8)
			returnStats.AddEquipment(newItem);
		else
			returnStats.AddEquipment(artifactSlot8);


		return returnStats;
	}

	public void ChangeWeapon(Item newWeapon)
	{
		if(EquipmentSelectionBtn.OnChangeWeapon != null)
			EquipmentSelectionBtn.OnChangeWeapon(newWeapon);
		currentWeapon = newWeapon;
		GetCurrentStats();
	}

	public void ChangeArtifact(Item newArtifact)
	{
		CheckArtifactDuplicate(newArtifact);
		if(EquipmentSelectionBtn.OnChangeArtifact != null)
				EquipmentSelectionBtn.OnChangeArtifact(newArtifact, selectedArtifactSlot);
		if(newArtifact.id == -1)
		{
			switch(selectedArtifactSlot)
			{
				case 1:
					PlayerStats.instance.RemoveEquipment(artifactSlot1);
					artifactSlot1 = null;
					break;
				case 2:
					PlayerStats.instance.RemoveEquipment(artifactSlot2);
					artifactSlot2 = null;
					break;
				case 3:
					PlayerStats.instance.RemoveEquipment(artifactSlot3);
					artifactSlot3 = null;
					break;
				case 4:
					PlayerStats.instance.RemoveEquipment(artifactSlot4);
					artifactSlot4 = null;
					break;
				case 5:
					PlayerStats.instance.RemoveEquipment(artifactSlot5);
					artifactSlot5 = null;
					break;
				case 6:
					PlayerStats.instance.RemoveEquipment(artifactSlot6);
					artifactSlot6 = null;
					break;
				case 7:
					PlayerStats.instance.RemoveEquipment(artifactSlot7);
					artifactSlot7 = null;
					break;
				case 8:
					PlayerStats.instance.RemoveEquipment(artifactSlot8);
					artifactSlot8 = null;
					break;
			}
		}
		else
		{
			switch(selectedArtifactSlot)
			{
				case 1:
					artifactSlot1 = newArtifact;
					break;
				case 2:
					artifactSlot2 = newArtifact;
					break;
				case 3:
					artifactSlot3 = newArtifact;
					break;
				case 4:
					artifactSlot4 = newArtifact;
					break;
				case 5:
					artifactSlot5 = newArtifact;
					break;
				case 6:
					artifactSlot6 = newArtifact;
					break;
				case 7:
					artifactSlot7 = newArtifact;
					break;
				case 8:
					artifactSlot8 = newArtifact;
					break;
			}
		}
		GetCurrentStats();
	}

	public void CheckArtifactDuplicate(Item newArtifact)
	{
		if(newArtifact == artifactSlot1)
		{
			if(EquipmentSelectionBtn.OnChangeArtifact != null)
				EquipmentSelectionBtn.OnChangeArtifact(null, 1);
			PlayerStats.instance.RemoveEquipment(artifactSlot1);
			artifactSlot1 = null;
		}
		if(newArtifact == artifactSlot2)
		{
			if(EquipmentSelectionBtn.OnChangeArtifact != null)
				EquipmentSelectionBtn.OnChangeArtifact(null, 2);
			PlayerStats.instance.RemoveEquipment(artifactSlot2);
			artifactSlot2 = null;
		}
		if(newArtifact == artifactSlot3)
		{
			if(EquipmentSelectionBtn.OnChangeArtifact != null)
				EquipmentSelectionBtn.OnChangeArtifact(null, 3);
			PlayerStats.instance.RemoveEquipment(artifactSlot3);
			artifactSlot3 = null;
		}
		if(newArtifact == artifactSlot4)
		{
			if(EquipmentSelectionBtn.OnChangeArtifact != null)
				EquipmentSelectionBtn.OnChangeArtifact(null, 4);
			PlayerStats.instance.RemoveEquipment(artifactSlot4);
			artifactSlot4 = null;
		}
		if(newArtifact == artifactSlot5)
		{
			if(EquipmentSelectionBtn.OnChangeArtifact != null)
				EquipmentSelectionBtn.OnChangeArtifact(null, 5);
			PlayerStats.instance.RemoveEquipment(artifactSlot5);
			artifactSlot5 = null;
		}
		if(newArtifact == artifactSlot6)
		{
			if(EquipmentSelectionBtn.OnChangeArtifact != null)
				EquipmentSelectionBtn.OnChangeArtifact(null, 6);
			PlayerStats.instance.RemoveEquipment(artifactSlot6);
			artifactSlot6 = null;
		}
		if(newArtifact == artifactSlot7)
		{
			if(EquipmentSelectionBtn.OnChangeArtifact != null)
				EquipmentSelectionBtn.OnChangeArtifact(null, 7);
			PlayerStats.instance.RemoveEquipment(artifactSlot7);
			artifactSlot7 = null;
		}
		if(newArtifact == artifactSlot8)
		{
			if(EquipmentSelectionBtn.OnChangeArtifact != null)
				EquipmentSelectionBtn.OnChangeArtifact(null, 8);
			PlayerStats.instance.RemoveEquipment(artifactSlot8);
			artifactSlot8 = null;
		}
	}
	public PlayerStats GetCurrentStats()
	{
		PlayerStats.ResetStats(baseStats);
		PlayerStats.instance.xpMultiplier = 1;
		List<Item> equipments = new List<Item> {currentWeapon, artifactSlot1, artifactSlot2, artifactSlot3, artifactSlot4, artifactSlot5 , artifactSlot6, artifactSlot7, artifactSlot8};
		List<Item> toRemove = new List<Item>();
		//Debug.Log("count: " + equipments.Count);
		for(int i = 0; i < equipments.Count; i++)
		{
			if(equipments[i] == null)
			{
				//Debug.Log("removing " + i);
				toRemove.Add(equipments[i]);
			}
			else if(equipments[i].equipBehaviour == null)
			{
				//Debug.Log("removing " + i);
				toRemove.Add(equipments[i]);
			}
			else
			{
				//Debug.Log("accepting " + i);
				equipments[i].equipBehaviour.RemoveItem(PlayerStats.instance, equipments[i]);
			}
		}

		foreach(Item item in toRemove)
		{
			equipments.Remove(item);
		}

		foreach(Item equipment in equipments)
		{
			//Debug.Log(equipment.iName);
		}

		equipments.Sort(delegate(Item x, Item y) { return x.equipBehaviour.priority.CompareTo(y.equipBehaviour.priority); });
			
		foreach(Item equipment in equipments)
		{
			PlayerStats.instance.AddEquipment(equipment);
		}
		// PlayerStats.instance.AddEquipment(currentWeapon);
		// PlayerStats.instance.AddEquipment(artifactSlot1);
		// PlayerStats.instance.AddEquipment(artifactSlot2);
		// PlayerStats.instance.AddEquipment(artifactSlot3);
		// PlayerStats.instance.AddEquipment(artifactSlot4);
		// PlayerStats.instance.AddEquipment(artifactSlot5);
		// PlayerStats.instance.AddEquipment(artifactSlot6);
		// PlayerStats.instance.AddEquipment(artifactSlot7);
		// PlayerStats.instance.AddEquipment(artifactSlot8);
		if(PlayerStats.instance.currentHealth > PlayerStats.instance.maxHealth)
			PlayerStats.instance.currentHealth = PlayerStats.instance.maxHealth;
        if (PlayerStats.instance.OnChangeHP != null)
            PlayerStats.instance.OnChangeHP(PlayerStats.instance.maxHealth, PlayerStats.instance.currentHealth);

        return PlayerStats.instance;

	}

	public void LoadSaveState()
	{
		GameManager.instance.playerInventory.inventory.Clear();
		foreach(int item in saveState.inventory)
		{
			GameManager.instance.playerInventory.inventory.Add(GameManager.instance.itemDB.items[item]);
		}

		//Debug.Log("SLOT 1!");
		//Debug.Log(saveState.artifactSlot1);
		//Debug.Log(GameManager.instance.playerInventory.inventory[saveState.artifactSlot1].iName);

		if(saveState.artifactSlot1 != -1)
		{
			artifactSlot1 = GameManager.instance.playerInventory.inventory[saveState.artifactSlot1];
			if(EquipmentSelectionBtn.OnChangeArtifact != null)
				EquipmentSelectionBtn.OnChangeArtifact(artifactSlot1, 1);
		}
        else
        {
            if (EquipmentSelectionBtn.OnChangeArtifact != null)
                EquipmentSelectionBtn.OnChangeArtifact(null, 1);
        }
			

		if(saveState.artifactSlot2 != -1)
		{
			artifactSlot2 = GameManager.instance.playerInventory.inventory[saveState.artifactSlot2];
			if(EquipmentSelectionBtn.OnChangeArtifact != null)
				EquipmentSelectionBtn.OnChangeArtifact(artifactSlot2, 2);
		}
        else
        {
            if (EquipmentSelectionBtn.OnChangeArtifact != null)
                EquipmentSelectionBtn.OnChangeArtifact(null, 2);
        }

        if (saveState.artifactSlot3 != -1)
		{
			artifactSlot3 = GameManager.instance.playerInventory.inventory[saveState.artifactSlot3];
			if(EquipmentSelectionBtn.OnChangeArtifact != null)
				EquipmentSelectionBtn.OnChangeArtifact(artifactSlot3, 3);
		}
        else
        {
            if (EquipmentSelectionBtn.OnChangeArtifact != null)
                EquipmentSelectionBtn.OnChangeArtifact(null, 3);
        }

        if (saveState.artifactSlot4 != -1)
		{
			artifactSlot4 = GameManager.instance.playerInventory.inventory[saveState.artifactSlot4];
			if(EquipmentSelectionBtn.OnChangeArtifact != null)
				EquipmentSelectionBtn.OnChangeArtifact(artifactSlot4, 4);
		}
        else
        {
            if (EquipmentSelectionBtn.OnChangeArtifact != null)
                EquipmentSelectionBtn.OnChangeArtifact(null, 4);
        }

        if (saveState.artifactSlot5 != -1)
		{
			artifactSlot5 = GameManager.instance.playerInventory.inventory[saveState.artifactSlot5];
			if(EquipmentSelectionBtn.OnChangeArtifact != null)
				EquipmentSelectionBtn.OnChangeArtifact(artifactSlot5, 5);
		}
        else
        {
            if (EquipmentSelectionBtn.OnChangeArtifact != null)
                EquipmentSelectionBtn.OnChangeArtifact(null, 5);
        }

        if (saveState.artifactSlot6 != -1)
		{
			artifactSlot6 = GameManager.instance.playerInventory.inventory[saveState.artifactSlot6];
			if(EquipmentSelectionBtn.OnChangeArtifact != null)
				EquipmentSelectionBtn.OnChangeArtifact(artifactSlot6, 6);
		
		}
        else
        {
            if (EquipmentSelectionBtn.OnChangeArtifact != null)
                EquipmentSelectionBtn.OnChangeArtifact(null, 6);
        }

        if (saveState.artifactSlot7 != -1)
		{
			artifactSlot7 = GameManager.instance.playerInventory.inventory[saveState.artifactSlot7];
			if(EquipmentSelectionBtn.OnChangeArtifact != null)
				EquipmentSelectionBtn.OnChangeArtifact(artifactSlot7, 7);
		}
        else
        {
            if (EquipmentSelectionBtn.OnChangeArtifact != null)
                EquipmentSelectionBtn.OnChangeArtifact(null, 7);
        }

        if (saveState.artifactSlot8 != -1)
		{
			artifactSlot8 = GameManager.instance.playerInventory.inventory[saveState.artifactSlot8];
			if(EquipmentSelectionBtn.OnChangeArtifact != null)
				EquipmentSelectionBtn.OnChangeArtifact(artifactSlot8, 8);
		}
        else
        {
            if (EquipmentSelectionBtn.OnChangeArtifact != null)
                EquipmentSelectionBtn.OnChangeArtifact(null, 8);
        }
        currentWeapon = GameManager.instance.playerInventory.inventory[saveState.weapon];

		ChangeWeapon(currentWeapon);

		baseStats.maxHealth = saveState.maxHealth;
		baseStats.currentHealth = saveState.maxHealth;
		baseStats.damage.power = saveState.power;
		baseStats.damage.criticalChance = saveState.critChan;
		baseStats.damage.criticalMultiplier = saveState.critMult;
		baseStats.damage.power = saveState.power;
		baseStats.damage.chargedMultiplier = saveState.chargedMult;

		PlayerStats.instance.skillFlags = saveState.skillFlags;
		Debug.Log("Loading: " + saveState.skillFlags.havePowerAttack);
		Debug.Log("Result: " + PlayerStats.instance.skillFlags.havePowerAttack);

		PlayerStats.instance.currentHealth = saveState.maxHealth;
		PlayerStats.instance.experience = saveState.exp;
		PlayerStats.instance.level = saveState.level;

		GameManager.instance.itemValidator.RestoreIds();
		
		GetCurrentStats();
		PlayerStats.instance.UpdateXP();

		//TriggerMessage.MessageContent customMessage = new TriggerMessage.MessageContent(null, null, new string[]{saveState.scene, "SAVE"});
		//GameManager.instance.sceneSwitcher.StartSwitch(customMessage);
		//GameManager.instance.controller.Revive();
		GameManager.instance.sceneSwitcher.StartLoad(saveState.scene);
	}


	public void SaveState()
	{

		PlayerSaveData newSS = new PlayerSaveData();

		List<int> itemIDList = new List<int>();
		for(int i = 0; i < GameManager.instance.playerInventory.inventory.Count; i++)
		{
			itemIDList.Add(GameManager.instance.playerInventory.inventory[i].id);

			if(GameManager.instance.playerInventory.inventory[i] == currentWeapon)
				newSS.weapon = i;

			if(GameManager.instance.playerInventory.inventory[i] == artifactSlot1)
				newSS.artifactSlot1 = i;

			if(GameManager.instance.playerInventory.inventory[i] == artifactSlot2)
				newSS.artifactSlot2 = i;

			if(GameManager.instance.playerInventory.inventory[i] == artifactSlot3)
				newSS.artifactSlot3 = i;

			if(GameManager.instance.playerInventory.inventory[i] == artifactSlot4)
				newSS.artifactSlot4 = i;

			if(GameManager.instance.playerInventory.inventory[i] == artifactSlot5)
				newSS.artifactSlot5 = i;

			if(GameManager.instance.playerInventory.inventory[i] == artifactSlot6)
				newSS.artifactSlot6 = i;

			if(GameManager.instance.playerInventory.inventory[i] == artifactSlot7)
				newSS.artifactSlot7 = i;

			if(GameManager.instance.playerInventory.inventory[i] == artifactSlot8)
				newSS.artifactSlot8 = i;	
		}

		newSS.inventory = itemIDList.ToArray();
		
		newSS.maxHealth = baseStats.maxHealth;

		newSS.power = baseStats.damage.power;
		newSS.critChan = baseStats.damage.criticalChance;
		newSS.critMult = baseStats.damage.criticalMultiplier;
		newSS.chargedMult = baseStats.damage.chargedMultiplier;

		newSS.skillFlags = new SkillFlags(PlayerStats.instance.skillFlags.havePowerAttack, PlayerStats.instance.skillFlags.hasAirDash, PlayerStats.instance.skillFlags.hasDoubleJump);

		GameManager.instance.itemValidator.SaveIds();

		// newSS.skillFlags.hasAirDash = PlayerStats.instance.skillFlags.hasAirDash;
		// newSS.skillFlags.hasDoubleJump = PlayerStats.instance.skillFlags.hasDoubleJump;
		// newSS.skillFlags.havePowerAttack = PlayerStats.instance.skillFlags.havePowerAttack;


		Debug.Log("Saving: " + PlayerStats.instance.skillFlags.havePowerAttack);

		newSS.exp = PlayerStats.instance.experience;
		newSS.level = PlayerStats.instance.level;

		newSS.scene = GameManager.instance.sceneSwitcher.currentRoom;

		//TriggerMessage.MessageContent customMessage = new TriggerMessage.MessageContent(null, null, new string[]{saveState.scene, "SAVE"});
		//GameManager.instance.sceneSwitcher.StartSwitch(customMessage);
		//GameManager.instance.controller.Revive();
		saveState = newSS;
	}

	public void OnGetLevel(int level)
	{
		baseStats.damage.power += 1;
		baseStats.damage.criticalChance += 0;
		baseStats.maxHealth += 2;	
		baseStats.currentHealth += 2;
		GetCurrentStats();
		levelUpFX.Play();
		aPlayer.PlayAudio(levelUpSFX);
		TextPopup.InstantiateText("Level Up!", 
			GameManager.instance.controller.transform.position + 
			GameManager.instance.controller.GetComponent<CharacterController>().center);
		GameManager.instance.SwitchLevelUp(true);

	}

	public void GetLevelBonus(string bonus)
	{
		switch(bonus)
		{
			case "health":
				baseStats.maxHealth += 3;
				baseStats.currentHealth += 3;
			break;
			case "power":
				baseStats.damage.power += 1;
			break;
			case "luck":
				baseStats.damage.criticalChance += 1f;
			break;
		}
		GameManager.instance.SwitchLevelUp(false);
		GetCurrentStats();
	}

	public void OnGetDamaged(GameObject instigator, DamageInfo damage, Stats attacker)
	{
		aPlayer.PlayAudio(damageSFX);
		if(attacker != null)
		{
			bloodParticles.Play();
		}
	}
	public void OnDeath(GameObject instigator, DamageInfo damage, Stats attacker)
	{
		aPlayer.PlayAudio(deathSFX);
		GameManager.instance.volumeManager.ChangeMusic(gameoverMusic, 20);
	}

	public void DropSW()
	{
		GameObject drop = Instantiate(currentSubweapon.dropPrefab);
		Vector3 targetPosition = transform.position;
		targetPosition.y += 1.2f;
		targetPosition.z += 0.05f;
		drop.transform.position = targetPosition;
		drop.GetComponentInChildren<SubWeaponInteraction>().SpawnSubWeapon();
	}
}
