using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    [SerializeField] float enemySpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] float maxHealth;
    [SerializeField] float damage;
    [SerializeField] float range = 0f;      // Range for ranged attacks; default is 0 for melee units
    [SerializeField] float fireRate = 0f;   // Fire rate for ranged attacks; default is 0 for melee units
    [SerializeField] bool canCharge = false;
    [SerializeField] bool countsTowardWinCondition = false;

    public float EnemySpeed {  get { return enemySpeed; } set {  enemySpeed = value; } }
    public float RotateSpeed { get { return rotateSpeed; } set {  rotateSpeed = value; } }
    public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public float Damage { get { return damage; } set { damage = value; } }
    public float Range { get { return range; } set { range = value; } }
    public float FireRate { get { return fireRate; } set { fireRate = value; } }
    public bool CanCharge { get { return canCharge; } set { canCharge = value; } } 
    public bool CountsTowardWinCondition { get { return countsTowardWinCondition; } set { countsTowardWinCondition = value; }
    }
}
