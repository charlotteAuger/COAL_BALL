using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance = null;
    [SerializeField] private Transform[] playerSpawnPoints;
    [SerializeField] private Transform[] aiSpawnPoints;
    [SerializeField] private int ballsToSpawn;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }

    public void GenerateLevel()
    {
        List<Transform> tempPlayerSpawnPoints = playerSpawnPoints.ToList<Transform>();
        List<Transform> tempAISpawnPoints = aiSpawnPoints.ToList<Transform>();

        for (int i = 0; i < ballsToSpawn; i++)
        {
            int r = Random.Range(0, tempPlayerSpawnPoints.Count);

            PoolManager.instance.CreateBall(tempPlayerSpawnPoints[r].position, true, 0);
            PoolManager.instance.CreateBall(tempAISpawnPoints[r].position, false, 0);

            tempPlayerSpawnPoints.RemoveAt(r);
            tempAISpawnPoints.RemoveAt(r);
        }
        
    }

    public void ClearLevel()
    {
        print("Clear !");
        
    }
}
