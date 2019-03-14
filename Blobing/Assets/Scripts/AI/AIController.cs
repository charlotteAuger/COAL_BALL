using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayType { MaxBalls, BiggestBall, Sniper}

public class AIController : MonoBehaviour
{
    private Transform currentBall;
    private PlayType currentStrategy;
    private Rigidbody2D rB2d;
    private Mergeable merge;
    [SerializeField] private Transform anchorPoint;
    [SerializeField] private SlingshotData data;
    private bool shootToMax = true;

    public Transform debugTarget;

    public void InitAIGame()
    {
        currentStrategy = (PlayType)Random.Range(0, 3);
    }

    public void InitAITurn()
    {
        currentBall = PoolManager.instance.CreateBall(anchorPoint.position, false, 0).transform;
        rB2d = currentBall.GetComponent<Rigidbody2D>();
        merge = currentBall.GetComponent<Mergeable>();
        merge.enabled = false;
    }

    public void Play()
    {
        ChooseStrategy();
        Vector3 t = ChooseTarget();
        Vector2 i = GetImpulseFromAim(t);
        StartCoroutine(SlingshotAnimation(i));
    }

    private void ChooseStrategy()
    {
        switch (currentStrategy)
        {
            case (PlayType.MaxBalls):
                if (Target.Instance.playerPoints.Count - Target.Instance.aiPoints.Count >= 2)
                {
                    currentStrategy = PlayType.Sniper;
                }
                else if (Target.Instance.DifferenceBetweenBiggestBalls() <= -2)
                {
                    currentStrategy = PlayType.BiggestBall;
                }
                break;

            case (PlayType.BiggestBall):
                if (Target.Instance.aiPoints.Count == 0)
                {
                    currentStrategy = PlayType.MaxBalls;
                }
                else if (Target.Instance.DifferenceBetweenBiggestBalls() >= 2)
                {
                    currentStrategy = PlayType.MaxBalls;
                }
                else if (Target.Instance.PlayerHasMoreBalls())
                {
                    currentStrategy = PlayType.Sniper;
                }
                break;

            case (PlayType.Sniper):
                if (Target.Instance.playerPoints.Count == 0)
                {
                    currentStrategy = PlayType.MaxBalls;
                }
                else if (Target.Instance.DifferenceBetweenBiggestBalls() >= 2)
                {
                    currentStrategy = PlayType.MaxBalls;
                }
                break;
        }
    }

    private Vector3 ChooseTarget()
    {
        print(currentStrategy);
        Vector3 target = Vector3.zero;
        switch (currentStrategy)
        {
            case (PlayType.MaxBalls):
                target = FindEmptyPosition();
                break;

            case (PlayType.BiggestBall):
                target = AimToMerge();
                break;

            case (PlayType.Sniper):
                target = AimToEject();
                break;
        }

        return target;
    }

    private Vector3 AimToMerge()
    {
        Transform target = Target.Instance.aiPoints[0].transform;
        shootToMax = false;

        return target.position;
    }

    private Vector3 AimToEject()
    {
        Transform target = Target.Instance.playerPoints[0].transform;
        shootToMax = true;

        return target.position;
    }

    private Vector3 FindEmptyPosition()
    {
        Vector3 target = Vector3.zero;
        shootToMax = false;

        bool correct = false;

        while (!correct)
        {
            Vector2 randomPoint = Random.insideUnitCircle/3;

            Collider2D[] onThePoint = Physics2D.OverlapCircleAll(randomPoint, 0.1f);

            if (onThePoint.Length <= 1)
            {
                bool empty = true;
                foreach (Collider2D c in onThePoint)
                {
                    if (c.transform.GetComponent<PointGiver>() != null)
                    {
                        empty = false;
                    }
                }

                if (empty)
                {
                    correct = true;
                    target = randomPoint;
                }
            }
        }

        return target;
    }

    private Vector2 GetImpulseFromAim(Vector3 target)
    {
        Vector2 impulse = Vector2.zero;
        Vector2 direction = target - anchorPoint.position;

        float factor = Mathf.Min(1,direction.magnitude / data.aimMaxDistance);

        if (shootToMax)
        {
            factor = 1;
        }

        impulse = direction.normalized * factor;

        return impulse;
    }

    private IEnumerator SlingshotAnimation(Vector2 impulse)
    {
        Vector2 pullVector = - impulse * data.maxDistance;
        Vector3 targetPosition = anchorPoint.position + new Vector3(pullVector.x, pullVector.y, 0);
        while (Vector3.Distance(targetPosition, currentBall.position) > 0.01f)
        {
            currentBall.position = Vector3.Lerp(currentBall.position, targetPosition, 0.2f);
            yield return new WaitForFixedUpdate();
        }

        currentBall.position = targetPosition;
        LaunchBall(impulse);
        StartCoroutine(TurnManager.Instance.EndTurn());
    }

    private void LaunchBall(Vector2 _impulse)
    {
        Vector2 impulse = _impulse * data.maxForce;

        rB2d.drag = data.ballLinearDrag;
        rB2d.AddForce(impulse, ForceMode2D.Impulse);

        merge.enabled = true;
    }
}
