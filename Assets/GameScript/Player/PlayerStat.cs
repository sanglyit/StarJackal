using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public PlayerScriptableObject playerData;
    //Current stats
    public float currentHealth;
    [HideInInspector] public float currentHeal;
    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] public float currentFireRate;
    [HideInInspector] public float currentStrength;
    [HideInInspector] public float currentMagnet;

    //Spawned Weapon
    public List<GameObject> spawnedWeapons;

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
    void Awake()
    {
        playerData = PlayerSelector.GetData();
        PlayerSelector.instance.DestroySingleton();

        currentHealth = playerData.MaxHealth;
        currentHeal = playerData.Heal;
        currentMoveSpeed = playerData.MoveSpeed;
        currentFireRate = playerData.FireRate;
        currentStrength = playerData.Strength;
        currentMagnet = playerData.Magnet;

        //Starting weapon
        SpawnedWeapon(playerData.StartingWeapon);
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

    public void SpawnedWeapon(GameObject weapon)
    {
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        spawnedWeapons.Add(spawnedWeapon); 
    }
}
