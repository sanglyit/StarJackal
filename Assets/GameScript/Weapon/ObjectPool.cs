using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    private void Awake()
    {
        Instance = this;
    }

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    public void CreatePool(string tag, GameObject prefab, int size)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            poolDictionary[tag] = new Queue<GameObject>();

            for (int i = 0; i < size; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                poolDictionary[tag].Enqueue(obj);
            }
        }
    }

    public GameObject GetFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        if (poolDictionary[tag].Count == 0)
        {
            Debug.LogWarning("Pool with tag " + tag + " is empty, adding more objects.");
            // If pool is empty, instantiate a new object and add it to the pool
            return ExpandPool(tag);
        }

        GameObject objToSpawn = poolDictionary[tag].Dequeue();
        if (objToSpawn == null)
        {
            Debug.LogWarning("The object to spawn is null.");
            return null;
        }

        objToSpawn.SetActive(true);
        poolDictionary[tag].Enqueue(objToSpawn); // Return to the pool after being used

        return objToSpawn;
    }

    public void ReturnObject(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }

        obj.SetActive(false);
        string tag = obj.name.Replace("(Clone)", "").Trim(); // Extract tag from the object name

        if (poolDictionary.ContainsKey(tag))
        {
            poolDictionary[tag].Enqueue(obj); // Return object to the pool
        }
        else
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist. Adding a new pool.");
            // Create a new pool if it doesn't exist and add the object
            CreatePool(tag, obj, 1);
            poolDictionary[tag].Enqueue(obj);
        }
    }

    // Method to expand the pool dynamically when it's empty
    private GameObject ExpandPool(string tag)
    {
        if (poolDictionary.ContainsKey(tag) && poolDictionary[tag].Count > 0)
        {
            GameObject prefab = poolDictionary[tag].Peek();
            GameObject newObj = Instantiate(prefab);
            newObj.SetActive(false);
            poolDictionary[tag].Enqueue(newObj);
            return newObj;
        }

        Debug.LogWarning("Cannot expand pool, tag " + tag + " does not exist or has no reference.");
        return null;
    }
}
