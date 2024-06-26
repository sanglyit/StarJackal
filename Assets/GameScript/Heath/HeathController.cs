using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeathController : MonoBehaviour
{
    [SerializeField] private float currentHeath;
    [SerializeField] private float maxHeath;

    //coi phan tram mau
    public float RemainingHeathPercentage
    {
        get
        {
            return (currentHeath / maxHeath);
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
        //Stop heath go below 0
        if (currentHeath == 0)
        {
            return;
        }
        //Give player *Queue title card*
        if (IsInvincible)
        {
            return;
        }

        currentHeath -= damageAmount;
        OnHealthChanged.Invoke();

        if (currentHeath < 0)
        {
            currentHeath = 0;
        }
        // check player die
        if (currentHeath == 0) 
        { 
            OnDied.Invoke();
        } else
        {
            OnDamaged.Invoke();
        }
    }

    public void Heal(float HealAmount) 
    {
        if (currentHeath == maxHeath)
        {
            return ;
        }

        currentHeath += HealAmount;
        OnHealthChanged.Invoke();

        if (currentHeath > maxHeath) 
        {
            currentHeath = maxHeath;
        }
    }
    public void Destroy()
    {
        // Destroy the enemy GameObject
        Destroy(gameObject);
    }
}
