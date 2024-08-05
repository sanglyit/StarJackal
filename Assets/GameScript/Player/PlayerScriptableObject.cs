using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "ScriptableObjects/PlayerShip")]
public class PlayerScriptableObject : ScriptableObject
{
    [SerializeField] GameObject startingWeapon;
    public GameObject StartingWeapon { get => startingWeapon; private set => startingWeapon = value; }

    [SerializeField] float maxHealth;
    public float MaxHealth { get => maxHealth; private set => maxHealth = value; }

    [SerializeField] float heal;
    public float Heal { get => heal; private set => heal = value; }

    [SerializeField] float moveSpeed;
    public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }
    //Ship fire rate
    [SerializeField] float fireRate;
    public float FireRate { get => fireRate; private set => fireRate = value; }
    //Ship damage bonus
    [SerializeField] float strength;
    public float Strength { get => strength; private set => strength = value; }
    //Magnet
    [SerializeField] float magnet;
    public float Magnet { get => magnet; private set => magnet = value; }
}
