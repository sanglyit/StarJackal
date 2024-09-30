using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public PlayerScriptableObject playerData;
    //Current stats
    float currentHealth;
    float currentHeal;
    float currentMoveSpeed;
    float currentFireRate;
    float currentStrength;
    float currentMagnet;

    #region Current Stat Property
    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            //check if health changed
            if (CurrentHealth != value)
            {
                currentHealth = value;
                //update real time of stat
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = "HP: " + currentHealth;
                }
            }
        }
    }
    public float CurrentHeal
    {
        get { return currentHeal; }
        set
        {
            //check if stat changed
            if (CurrentHeal != value)
            {
                currentHeal = value;
                //update real time of stat
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealDisplay.text = "Regen: " + currentHeal;
                }
            }
        }
    }
    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            //check if stat changed
            if (CurrentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                //update real time of stat
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "Speed: " + currentMoveSpeed;
                }
                Debug.Log("Speed Changed");
            }
        }
    }
    public float CurrentFireRate
    {
        get { return currentFireRate; }
        set
        {
            //check if stat changed
            if (CurrentFireRate != value)
            {
                currentFireRate = value;
                //update real time of stat
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentFireRateDisplay.text = "Fire Rate: " + currentFireRate;
                }

            }
        }
    }
    public float CurrentStrength
    {
        get { return currentStrength; }
        set
        {
            //check if stat changed
            if (CurrentStrength != value)
            {
                currentStrength = value;
                //update real time of stat
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentStrengthDisplay.text = "Strength: " + currentStrength;
                }

            }
        }
    }

    
    #endregion
    //Level system
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;
    
    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }
    //I-frame
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;
    public List<LevelRange> levelRanges;

    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;

    public GameObject secondWeaponTest;
    public GameObject firstPassiveItemTest, secondPassiveItemTest;

    PlayerCollector collector;
    void Awake()
    {
        playerData = ShipSelector.GetData();
        ShipSelector.instance.DestroySingleTon();

        inventory = GetComponent <InventoryManager>();

        //Assigning variables
        CurrentHealth = playerData.MaxHealth;
        CurrentHeal = playerData.Heal;
        CurrentMoveSpeed = playerData.MoveSpeed;
        CurrentFireRate = playerData.FireRate;
        CurrentStrength = playerData.Strength;
        currentMagnet = playerData.Magnet;

        collector = GetComponentInChildren<PlayerCollector>();

        //Spawn starting weapon
        SpawnWeapon(playerData.StartingWeapon);

        SpawnWeapon(secondWeaponTest);
        SpawnPassiveItem(firstPassiveItemTest);
        SpawnPassiveItem(secondPassiveItemTest);
        
    }
    void Start()
    {
        //initialize exp cap as the first exp cap increase
        experienceCap = levelRanges[0].experienceCapIncrease;

        //set current stat display
        GameManager.instance.currentHealthDisplay.text = "HP: " + currentHealth;
        GameManager.instance.currentHealDisplay.text = "Regen: " + currentHeal;
        GameManager.instance.currentMoveSpeedDisplay.text = "Speed: " + currentMoveSpeed;
        GameManager.instance.currentFireRateDisplay.text = "Fire Rate: " + currentFireRate;
        GameManager.instance.currentStrengthDisplay.text = "Strength: " + currentStrength;

        GameManager.instance.AssignChosenCharacterUI(playerData);
    }
    void Update()
    {
        if(invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        } else if (isInvincible) {
            isInvincible = false;
        }
        Regen();
    }
    public void IncreaseExperience(int amount)
    {
        experience += amount;

        LevelUpChecker();
    }

    void LevelUpChecker()
    {
        if(experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;
            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if(level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;
        }
    }

    public void TakeDamage(float dmg)
    {
        if (!isInvincible)
        {
            CurrentHealth -= dmg;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (CurrentHealth <= 0)
            {
                kill();
            }
        }
    }
    public void kill()
    {
        Destroy(gameObject);
        if (!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignChosenWeaponsAndPassiveItemUI(inventory.weaponUISlots, inventory.passiveItemUISlots);
            GameManager.instance.GameOver();
        }
        Debug.Log("Player Got Skill Issued");
    }

    public void HealHealth(float amount)
    { 
        if (CurrentHealth < playerData.MaxHealth)
        {
            CurrentHealth += amount;

            if (CurrentHealth > playerData.MaxHealth)
            {
                CurrentHealth = playerData.MaxHealth;
            }
        }
        
    }

    void Regen()
    {
        if (CurrentHealth < playerData.MaxHealth)
        {
            CurrentHealth += CurrentHeal * Time.deltaTime;
            if (CurrentHealth > playerData.MaxHealth)
            { 
                CurrentHealth = playerData.MaxHealth;
            }
        }
    }
    public void SpawnWeapon(GameObject weapon)
    {
       
            //Check if the inventory slots are full, and returning if it is
            if (weaponIndex >= inventory.weaponSlots.Count - 1) //Must be -1 because the list starts from 0
            {
                Debug.LogError("Inventory slots already full");
                return;
            }
            // Instantiate the weapon and set its parent to WeaponHolder
            GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
            spawnedWeapon.transform.SetParent(transform);
            inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<GunController>()); //Add the weapon to it's inventory slot

            weaponIndex++;
        }

    public void SpawnPassiveItem(GameObject passiveItem)
    {

        //Check if the inventory slots are full, and returning if it is
        if (passiveItemIndex >= inventory.passiveItemSlots.Count - 1) //Must be -1 because the list starts from 0
        {
            Debug.LogError("Inventory slots already full");
            return;
        }
        // Instantiate the passive Item 
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>()); //Add the passive item to it's inventory slot

        passiveItemIndex++;
    }
}
