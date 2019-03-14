using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList", menuName = "Level/LevelList", order = 1)]
public class LevelList : ScriptableObject
{
    public LevelData[] levels;
}
