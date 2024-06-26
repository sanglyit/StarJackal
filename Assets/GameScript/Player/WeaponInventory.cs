using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public List<Gun> equippedWeapons = new List<Gun>();
    private int currentWeaponIndex = 0;

    private void Start()
    {
        if (equippedWeapons.Count > 0)
        {
            EquipWeapon(0);
        }
    }

    public void EquipWeapon(int index)
    {
        for (int i = 0; i < equippedWeapons.Count; i++)
        {
            equippedWeapons[i].gameObject.SetActive(i == index);
        }
        currentWeaponIndex = index;
    }

    public void NextWeapon()
    {
        if (equippedWeapons.Count == 0) return;
        currentWeaponIndex = (currentWeaponIndex + 1) % equippedWeapons.Count;
        EquipWeapon(currentWeaponIndex);
    }

    public void PreviousWeapon()
    {
        if (equippedWeapons.Count == 0) return;
        currentWeaponIndex = (currentWeaponIndex - 1 + equippedWeapons.Count) % equippedWeapons.Count;
        EquipWeapon(currentWeaponIndex);
    }
}
