using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemScriptableObject", menuName = "ScriptableObjects/Passive Item")]
public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField]
    float multiplier;
    public float Multiplier { get => multiplier; private set => multiplier = value; }

    [SerializeField]
    int level; //Not meant to be modified in the game [Only in Editor]
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefab; //the prefab of the next level = what the object becomes when it levels up
                                //Not to be confused with the prefab to be spawned at the next level.
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    [SerializeField]
    new string name;
    public string Name { get => name; private set => name = value; }
    [SerializeField]
    new string description;
    public string Description { get => description; private set => description = value; }

    [SerializeField] // not meant to be modified during run time
    Sprite icon;
    public Sprite Icon { get => icon; private set => icon = value; }
}
