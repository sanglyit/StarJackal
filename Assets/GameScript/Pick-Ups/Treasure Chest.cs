using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    InventoryManager inventory;
    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<InventoryManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OpenTreasureChest();
            Destroy(gameObject);
        }
    }

    public void OpenTreasureChest()
    {
        if (inventory.GetPossibleEvolution().Count <= 0)
        {
            Debug.LogWarning("No available evol");
            return;
        }
        WeaponEvolutionBlueprint toEvolve = inventory.GetPossibleEvolution()[Random.Range(0, inventory.GetPossibleEvolution().Count)];
        inventory.EvolveWeapon(toEvolve);
    }
}
