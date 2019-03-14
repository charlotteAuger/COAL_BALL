using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mergeable : MonoBehaviour {

    [Header("Animation")]
    [SerializeField] private GameObject sprite;
    [SerializeField] private GameObject mergeFX;
    [SerializeField] private GameObject collisionFX;

    [Header("References")]
    [SerializeField] private Rigidbody2D rB2d;
    [SerializeField] private PointGiver pG;
    [SerializeField] private BallPool pool;
    [SerializeField] private BallList list;

    private Vector3 lastVelocity;

    private void Update()
    {
        lastVelocity = rB2d.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Mergeable other = collision.gameObject.GetComponent<Mergeable>();

        if (other != null && other.enabled)
        {
            PointGiver opG = other.transform.GetComponent<PointGiver>();
            if (opG.isOwnedByPlayer == pG.isOwnedByPlayer && other.rB2d.velocity.magnitude < rB2d.velocity.magnitude)
            {
                int newGrowthValue = opG.stats.growthID + pG.stats.growthID;
                Absorb(newGrowthValue - 1, collision.relativeVelocity);
                other.DestroyBall();
            }
        }
        else
        {
            Instantiate(collisionFX, collision.GetContact(0).point, Quaternion.identity);
        }
    }

    private IEnumerator ScaleWobble(float targetScale)
    {
        while (sprite.transform.localScale.x > targetScale)
        {
            float newScale = Mathf.Lerp(sprite.transform.localScale.x, targetScale, 0.2f);
            sprite.transform.localScale = new Vector3(newScale, newScale, newScale);
            yield return null;
        }

        sprite.transform.localScale = new Vector3(targetScale, targetScale, targetScale);
    }

    private void Absorb(int ballTypeId, Vector3 v)
    {
        BallStats bS = list.balls[ballTypeId];
        StartCoroutine(ScaleWobble(bS.scale));
        pG.SetStats(bS, pG.isOwnedByPlayer);
        rB2d.velocity = v;
    }

    public void DestroyBall()
    {
        Target.Instance.Remove(pG);
        Instantiate(mergeFX, transform.position, Quaternion.identity);
        pool.Disable();
    }
}
