using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Gun")]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField] int damage;
    [SerializeField] float shootingCooldown;
    [SerializeField] float spread;
    [SerializeField] float reloadTime;
    [SerializeField] float shootForce;
    [SerializeField] int pierce;

    [SerializeField] int magSize;
    [SerializeField] bool isAutomatic;
    [SerializeField] bool autoReload;

    [SerializeField] GameObject bulletPrefab;
    public int Damage {get => damage; private set => damage = value; }
    public float ShootingCooldown { get => shootingCooldown; private set => shootingCooldown = value;}
    public float Spread { get => spread; private set => spread = value; }
    public float ReloadTime { get => reloadTime; private set => reloadTime = value; }
    public float ShootForce { get => shootForce; private set => shootForce = value; }
    public int Pierce {  get => pierce; private set => pierce = value; }

    public int MagSize { get => magSize; private set => magSize = value; }
    public bool IsAutomatic { get => isAutomatic; private set => isAutomatic = value; }
    public bool AutoReload { get => autoReload; private set => autoReload = value; }

    public GameObject BulletPrefab { get => bulletPrefab; private set => bulletPrefab = value; } 
}
