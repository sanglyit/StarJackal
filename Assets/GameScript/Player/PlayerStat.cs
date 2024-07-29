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
    float currentFireSpeed;
    float currentStrength;
    public float CurrentHealth => currentHealth;
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
        currentHealth = playerData.MaxHealth;
        currentHeal = playerData.Heal;
        currentMoveSpeed = playerData.MoveSpeed;
        currentFireRate = playerData.FireRate;
        currentStrength = playerData.Strength;
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
}
