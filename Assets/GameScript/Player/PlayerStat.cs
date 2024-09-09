using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public PlayerScriptableObject playerData;
    //Current stats
    public float currentHealth;
    [SerializeField] public float currentHeal;
    [SerializeField] public float currentMoveSpeed;
    [SerializeField] public float currentFireRate;
    [SerializeField] public float currentStrength;
    [SerializeField] public float currentMagnet;

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
        currentHealth = playerData.MaxHealth;
        currentHeal = playerData.Heal;
        currentMoveSpeed = playerData.MoveSpeed;
        currentFireRate = playerData.FireRate;
        currentStrength = playerData.Strength;
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
            currentHealth -= dmg;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (currentHealth <= 0)
            {
                kill();
            }
        }
    }
    public void kill()
    {
        Destroy(gameObject);
        Debug.Log("Player Got Skill Issued");
    }

    public void HealHealth(float amount)
    { 
        currentHealth += amount;

        if (currentHealth > playerData.MaxHealth) 
        {
            currentHealth = playerData.MaxHealth;
        }
    }

    void Regen()
    {
        if (currentHealth < playerData.MaxHealth)
        {
            currentHealth += currentHeal * Time.deltaTime;
            if (currentHealth > playerData.MaxHealth)
            { 
                currentHealth = playerData.MaxHealth;
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
