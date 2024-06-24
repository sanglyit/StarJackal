using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIFrame : MonoBehaviour
{
    [SerializeField] private float _invincibilityDuration;
    private InvincibilityController _invincibilityController;
    

    public void Awake()
    {
        _invincibilityController = GetComponent<InvincibilityController>();
    }
    public void StartInvincibility()
    {
        _invincibilityController.StartInvincibility(_invincibilityDuration);
    }
}
