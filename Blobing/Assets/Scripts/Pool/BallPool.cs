using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPool : Poolable {

    private BallStats stats;
    [SerializeField] private CircleCollider2D collider;
    [SerializeField] private PointGiver pGiver;
    [SerializeField] private SpriteRenderer sRend;
    [SerializeField] private Sprite playerBallSprite;
    [SerializeField] private Sprite aiBallSprite;

    public override void Spawn()
    {
        collider.enabled = true;
        base.Spawn();
    }

    public override void Disable()
    {
        collider.enabled = false;
        base.Disable();
    }

    public void SetUp(BallStats _stats, bool _isOwnbedByPlayer)
    {
        pGiver.SetStats(_stats, _isOwnbedByPlayer);
        sRend.sprite = _isOwnbedByPlayer ? playerBallSprite : aiBallSprite;
    }

    private void Update()
    {
        if (!isAvailable && Input.GetKeyDown(KeyCode.Space))
        {
            Disable();
        }
    }
}
