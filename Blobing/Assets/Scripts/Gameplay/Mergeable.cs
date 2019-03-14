using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mergeable : MonoBehaviour {

    [Header("Animation")]
    [SerializeField] private AnimationCurve scaleCurve;
    [SerializeField] private float scaleAmplitude;
    [SerializeField] private float animationTime;
    [SerializeField] private GameObject sprite;

    [Header("References")]
    [SerializeField] private Rigidbody2D rB2d;
    [SerializeField] private PointGiver pG;
    [SerializeField] private BallPool pool;
    [SerializeField] private BallList list;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Mergeable other = collision.gameObject.GetComponent<Mergeable>();

        if (other != null && other.enabled)
        {
            PointGiver opG = other.transform.GetComponent<PointGiver>(); 
            if (opG.isOwnedByPlayer == pG.isOwnedByPlayer && other.rB2d.velocity.magnitude < rB2d.velocity.magnitude)
            {
                int newGrowthValue = opG.stats.growthID + pG.stats.growthID;
                Absorb(newGrowthValue -1);
                other.DestroyBall();
            }
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

    private void Absorb(int ballTypeId)
    {
        BallStats bS = list.balls[ballTypeId];
        StartCoroutine(ScaleWobble(bS.scale));
        pG.SetStats(bS, pG.isOwnedByPlayer);
    }

    public void DestroyBall()
    {
        Target.Instance.Remove(pG);
        pool.Disable();
    }
}
