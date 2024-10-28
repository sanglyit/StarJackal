using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        Instance = this;
        poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();
    }

    // Creates a pool for a specific prefab with the desired size
    public void CreatePool(GameObject prefab, int size)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab] = new Queue<GameObject>();

            for (int i = 0; i < size; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                poolDictionary[prefab].Enqueue(obj);
            }
        }
    }

    // Retrieves an object from the pool or creates a new one if needed
    public GameObject GetFromPool(GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogWarning("Pool for prefab " + prefab.name + " does not exist.");
            return null;
        }

        // Check if there are objects in the pool
        if (poolDictionary[prefab].Count > 0)
        {
            GameObject obj = poolDictionary[prefab].Dequeue();
            obj.SetActive(true);
            return obj;
        }

        // If the pool is empty, instantiate a new object
        GameObject newObj = Instantiate(prefab);
        newObj.SetActive(true);
        return newObj;
    }

    // Returns an object back to the pool
    public void ReturnObject(GameObject obj, GameObject prefab)
    {
        if (obj == null || prefab == null)
        {
            Debug.LogWarning("Attempted to return a null object or prefab to the pool.");
            return;
        }

        obj.SetActive(false);

        // Ensure the prefab's pool exists before returning the object
        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab] = new Queue<GameObject>();
        }

        poolDictionary[prefab].Enqueue(obj);
    }
}

