using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ball", menuName = "Balancing/Ball/BallData", order = 1)]
public class BallStats : ScriptableObject {

    public float scale;
    public int scoreValue;
    public int growthID;
    public float mass;

}
