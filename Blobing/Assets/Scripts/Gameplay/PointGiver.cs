using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointGiver : MonoBehaviour {

    /*[HideInInspector] */public BallStats stats;
    [HideInInspector] public bool isOwnedByPlayer;
    [SerializeField] private Poolable pool;
    [SerializeField] private GameObject sprite;
    [SerializeField] private CircleCollider2D collider;
    [SerializeField] private Rigidbody2D rB2d;

    public void SetStats(BallStats _stats, bool _isOwnbedByPlayer)
    {
        stats = _stats;
        isOwnedByPlayer = _isOwnbedByPlayer;

        collider.radius = stats.scale / 10;
        sprite.transform.localScale = Vector3.one * stats.scale;
        rB2d.mass = stats.mass;
    }

    public int GetPointValue()
    {
        return stats.scoreValue;
    }

}
