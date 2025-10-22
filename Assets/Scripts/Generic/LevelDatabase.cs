using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "Scriptable Objects/LevelDatabase")]
public class LevelDatabase : ScriptableObject
{
    public List<GameObject> stagePrefabs;
}
