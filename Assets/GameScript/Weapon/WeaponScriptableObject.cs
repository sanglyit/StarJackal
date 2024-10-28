using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Gun")]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] new string description;
    [SerializeField] int level; //Not meant to be modified in the game [Only in Editor]
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
    

    
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefab; //the prefab of the next level = what the object becomes when it levels up
                                //Not to be confused with the prefab to be spawned at the next level.
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    
    public string Name { get => name; private set => name = value; }
    
    public string Description { get => description; private set => description = value; }

    [SerializeField] // not meant to be modified during run time
    Sprite icon;
    public Sprite Icon { get => icon; private set => icon = value; }
}
