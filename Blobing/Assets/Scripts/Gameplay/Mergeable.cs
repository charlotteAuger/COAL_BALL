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

    public List<Vector3> pastVelocities = new List<Vector3>();

    private Vector3 GetAveragedVelocity()
    {
        Vector3 averagedVelocity = Vector3.zero;

        for (int i = 1; i < pastVelocities.Count; i++)
        {
            averagedVelocity = averagedVelocity + pastVelocities[i];
            print("add : " + pastVelocities[i]);
        }

        averagedVelocity = averagedVelocity / (pastVelocities.Count - 1);

        print("averaged = " + averagedVelocity);

        return averagedVelocity;
    }

    private void FixedUpdate()
    {
        pastVelocities.Insert(0, rB2d.velocity);

        if (pastVelocities.Count > 4)
        {
            pastVelocities.RemoveAt(4);
        }
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
                Vector3 newV = other.GetAveragedVelocity()/1.1f;
                Absorb(newGrowthValue - 1, newV);
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
        int lastValue = pG.stats.scoreValue;
        BallStats bS = list.balls[ballTypeId];
        StartCoroutine(ScaleWobble(bS.scale));
        pG.SetStats(bS, pG.isOwnedByPlayer);
        int pointDiff = bS.scoreValue - lastValue;
        if (Target.Instance.playerPoints.Contains(pG) && pG.isOwnedByPlayer)
        {
            pG.popup.StartDisplay(pointDiff, true, pG.isOwnedByPlayer);
        }
        else if (Target.Instance.aiPoints.Contains(pG) && !pG.isOwnedByPlayer)
        {
            pG.popup.StartDisplay(pointDiff, true, pG.isOwnedByPlayer);
        }

        rB2d.velocity = v;
    }

    public void DestroyBall()
    {
        Target.Instance.Remove(pG);
        Instantiate(mergeFX, transform.position, Quaternion.identity);
        pool.Disable();
    }
}
