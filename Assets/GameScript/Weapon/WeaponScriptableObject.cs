using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Gun")]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField] public int Damage;
    [SerializeField] public string weaponName;

    [SerializeField] public float shootingCooldown;
    [SerializeField] public float spread;
    [SerializeField] public float reloadTime;
    [SerializeField] public float shootForce;
    [SerializeField] public float pierce;

    [SerializeField] public int magSize;
    [SerializeField] public bool isAutomatic;
    [SerializeField] public bool AutoReload;
    [SerializeField] public GameObject bulletPrefab;
}
