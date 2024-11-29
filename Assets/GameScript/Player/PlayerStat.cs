using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    public PlayerScriptableObject playerData;
    public SpriteRenderer spriteRenderer;
    public GameObject damageEffect;
    public GameObject ShieldEffect;

    //Current stats
    float currentHealth;
    float currentHeal;
    float currentMoveSpeed;
    float currentFireRate;
    float currentStrength;
    float currentMagnet;
    private float runtimeMaxHealth; // Temporary max health variable

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

    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI finalFireRateText;

    PlayerCollector collector;
    void Awake()
    {
        playerData = ShipSelector.GetData();
        ShipSelector.instance.DestroySingleTon();

        inventory = GetComponent <InventoryManager>();
        collector = GetComponent <PlayerCollector>();

        //Assigning variables
        runtimeMaxHealth = playerData.MaxHealth;
        CurrentHealth = runtimeMaxHealth;
        CurrentHeal = playerData.Heal;
        CurrentMoveSpeed = playerData.MoveSpeed;
        CurrentFireRate = playerData.FireRate;
        CurrentStrength = playerData.Strength;
        currentMagnet = playerData.Magnet;

        // Set up player sprite based on selected character
        UpdatePlayerSprite();
        collector = GetComponentInChildren<PlayerCollector>();

        //Spawn starting weapon
        SpawnWeapon(playerData.StartingWeapon);
        collector.SetRadius(playerData.Magnet);
        
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

        UpdateHealthBar();
        UpdateExpBar();
        UpdateLevelText();
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
        UpdateExpBar();
    }

    void UpdatePlayerSprite()
    {
        if (spriteRenderer != null && playerData.CharacterSprite != null)
        {
            // Assign the selected character's sprite
            spriteRenderer.sprite = playerData.CharacterSprite;
        }
    }
    void LevelUpChecker()
    {
        if(experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;
            IncreaseMaxHealth(10);
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

            UpdateLevelText();
            GameManager.instance.StartLevelUp();
        }
    }
    public void LevelUp()
    {
        level++;
        IncreaseMaxHealth(10);
        int experienceCapIncrease = 0;
        foreach (LevelRange range in levelRanges)
        {
            if (level >= range.startLevel && level <= range.endLevel)
            {
                experienceCapIncrease = range.experienceCapIncrease;
                break;
            }
        }
        experienceCap += experienceCapIncrease;

        UpdateLevelText();
        GameManager.instance.StartLevelUp();
    }
    public float GetAdjustedCooldown(float baseCooldown)
    {
        // Calculate the adjusted shooting cooldown using the player's fire rate
        float adjustedCooldown = baseCooldown / Mathf.Max(CurrentFireRate, 0.1f);
        // Update the UI with the calculated final fire rate (or fire rate per second)
        if (finalFireRateText != null)
        {
            float fireRatePerSecond = 1f / adjustedCooldown;
            finalFireRateText.text = "Shots/sec: " + fireRatePerSecond.ToString("F2") ;
        }
        return adjustedCooldown;

    }

    void UpdateExpBar()
    {
        // Update exp bar fill amount
        expBar.fillAmount = (float)experience / experienceCap;
    }

    void UpdateLevelText()
    {
        // Update level text
        levelText.text = "LV " + level.ToString(); 
    }

    public void TakeDamage(float dmg)
    {
        if (!isInvincible)
        {
            CurrentHealth -= dmg;

            if (damageEffect)
            {
                GameObject instantiatedEffect = Instantiate(damageEffect, transform.position, Quaternion.identity);
                Destroy(instantiatedEffect, 3f);
            }
            
            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (CurrentHealth <= 0)
            {
                kill();
            }
            PlayShieldEffect();
            UpdateHealthBar();
        }
    }

    void PlayShieldEffect()
    {
        if (ShieldEffect != null)
        {
            // Instantiate the shield effect as a child of the player to make it follow
            GameObject instantiatedShield = Instantiate(ShieldEffect, transform.position, Quaternion.identity, transform);
            Destroy(instantiatedShield, 3f);
        }
    }
    public void IncreaseMaxHealth(float amount)
    {
        runtimeMaxHealth += amount;
        CurrentHealth = runtimeMaxHealth;
        UpdateHealthBar(); // Reflect the changes in the UI
    }
    void UpdateHealthBar()
    {
        //Update the health bar
        healthBar.fillAmount = currentHealth / runtimeMaxHealth;
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
            UpdateHealthBar();
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
            UpdateHealthBar();
        }
    }
    public void SpawnWeapon(GameObject weapon)
    {
        // Check if the inventory slots are full
        if (weaponIndex >= inventory.weaponSlots.Count - 1)
        {
            Debug.LogError("Inventory slots already full");
            return;
        }

        // Instantiate the weapon with no rotation
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);

        // Set the weapon's parent to the WeaponHolder (to avoid inheriting player's rotation)
        spawnedWeapon.transform.SetParent(transform);

        // Reset the local rotation of the weapon to zero after parenting
        spawnedWeapon.transform.localRotation = Quaternion.Euler(0, 0, 0);

        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<GunController>());

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
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.Euler(0, 0, 0));
        spawnedPassiveItem.transform.SetParent(transform);
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>()); //Add the passive item to it's inventory slot

        passiveItemIndex++;
    }
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
}
