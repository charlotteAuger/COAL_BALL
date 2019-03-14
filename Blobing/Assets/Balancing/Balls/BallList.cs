using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BallList", menuName = "Ball/BallList", order = 1)]
public class BallList : ScriptableObject
{
    public BallStats[] balls;
}
