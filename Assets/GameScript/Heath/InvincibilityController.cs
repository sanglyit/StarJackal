using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityController : MonoBehaviour
{
    private HeathController heathController;
    private void Awake()
    {
        heathController = GetComponent<HeathController>();
    }
    public void StartInvincibility(float invincibilityDuration)
    {
        StartCoroutine(InvincibilityCorutine(invincibilityDuration));
    }

    private IEnumerator InvincibilityCorutine(float invincibilityDuration) 
    {
        heathController.IsInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        heathController.IsInvincible = false;
    }
}
