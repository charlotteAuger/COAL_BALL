using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    public List<BallPool> pool = new List<BallPool>();
    public BallList list;

    private void Awake()
    {
        if (instance == null) instance = this;

        else Destroy(this);
    }

    public BallPool CreateBall(Vector3 position, bool ownedByPlayer, int typeId)
    {
        //Check pool for available bullet
        BallPool result = null;

        

        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].isAvailable)
            {
                result = pool[i];
                break;
            }
        }

        
        result.transform.position = position;
        result.Spawn();
        result.SetUp(list.balls[typeId], ownedByPlayer);
        

        return result;
    }

    public void DestroyObject(Poolable pToDestroy)
    {
        pToDestroy.Disable(); 
    }

    public void Clear()
    {
        List<BallPool> spawnedBalls = pool.FindAll(p => p.isAvailable == false);
        foreach (BallPool p in spawnedBalls)
        {
            p.Disable();
        }
    }
}
