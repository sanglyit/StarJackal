using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityController : MonoBehaviour
{
    private HealthController healthController;
    private void Awake()
    {
        healthController = GetComponent<HealthController>();
    }
    public void StartInvincibility(float invincibilityDuration)
    {
        StartCoroutine(InvincibilityCorutine(invincibilityDuration));
    }

    private IEnumerator InvincibilityCorutine(float invincibilityDuration) 
    {
        healthController.IsInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        healthController.IsInvincible = false;
    }
}
