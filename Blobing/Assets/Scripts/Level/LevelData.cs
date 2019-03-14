using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public int id;
    public float scoreGoal;
}
