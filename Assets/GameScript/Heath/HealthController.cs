using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    //coi phan tram mau
    public float RemainingHealthPercentage
    {
        get
        {
            return (currentHealth / maxHealth);
        }
    }

    public bool IsInvincible { get; set; }
    public UnityEvent OnDied;
    public UnityEvent OnDamaged;
    public UnityEvent OnHealthChanged;
    private void Start()
    {
        OnDied.AddListener(Destroy);
    }
    public void TakeDamage(float damageAmount)
    {
        //Stop Health go below 0
        if (currentHealth == 0)
        {
            return;
        }
        //Give player *Queue title card*
        if (IsInvincible)
        {
            return;
        }

        currentHealth -= damageAmount;
        OnHealthChanged.Invoke();

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        // check player die
        if (currentHealth == 0) 
        { 
            OnDied.Invoke();
        } else
        {
            OnDamaged.Invoke();
        }
    }

    public void Heal(float HealAmount) 
    {
        if (currentHealth == maxHealth)
        {
            return ;
        }

        currentHealth += HealAmount;
        OnHealthChanged.Invoke();

        if (currentHealth > maxHealth) 
        {
            currentHealth = maxHealth;
        }
    }
    public void Destroy()
    {

        Destroy(gameObject);
    }
}
