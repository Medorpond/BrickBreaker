using System;
using UnityEngine;

[Serializable]
public class LevelData
{
    [field: SerializeField] public GameObject LevelPrefab;
    [field: SerializeField] public AudioClip LevelBgm;
}
