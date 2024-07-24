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

    public float EnemySpeed {  get { return enemySpeed; } set {  enemySpeed = value; } }
    public float RotateSpeed { get { return rotateSpeed; } set {  rotateSpeed = value; } }
    public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public float Damage { get { return damage; } set { damage = value; } }
}
