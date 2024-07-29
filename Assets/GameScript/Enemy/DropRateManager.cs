using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DropRateManager;

public class DropRateManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public string name;
        public GameObject itemPrefab;
        public float dropRate;
    }
    [Header("Make sure the total drop rate <= 100")]
    public List<Drops> drops;
    private void Start()
    {
        // Initialize the object pool for each drop item
        foreach (Drops drop in drops)
        {
            ObjectPool.Instance.CreatePool(drop.name, drop.itemPrefab, 10);
        }
    }
    void OnDestroy()
    {
        float randomNumber = Random.Range(0f, 100f);
        List<Drops> possibleDrop = new List<Drops>();
        foreach (Drops rate in drops)
        {
            if (randomNumber <= rate.dropRate)
            {
                possibleDrop.Add(rate);
            }
        }
        //check if there any others possible drop
        if (possibleDrop.Count > 0)
        {
            Drops drop = possibleDrop[Random.Range(0, possibleDrop.Count)];
            GameObject droppedItem = ObjectPool.Instance.GetFromPool(drop.name);
            if (droppedItem != null)
            {
                droppedItem.transform.position = transform.position;
                droppedItem.transform.rotation = Quaternion.identity;
            }
        }
        return;
    }
}
