using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dragable : MonoBehaviour {


    [SerializeField] private Rigidbody2D rB2d;
    [SerializeField] private Transform anchorPoint;
    [SerializeField] private SlingshotData data;
    [SerializeField] private LineRenderer aimLine;
    [SerializeField] private Mergeable merge;

    private bool slinging;
    private Coroutine returnCoroutine;

    private bool dragged = false;

    private void Start()
    {
        rB2d.drag = data.ballLinearDrag;
    }

    public void InitDrag()
    {
        DragInput.Instance.BeginSling += BeginSlinging;
        DragInput.Instance.LaunchSling += Launch;
        DragInput.Instance.QuitSling += EndSlinging;
        Attach();
    }

    public void ResetDrag()
    {
        DragInput.Instance.BeginSling -= BeginSlinging;
        DragInput.Instance.LaunchSling -= Launch;
        DragInput.Instance.QuitSling -= EndSlinging;
    }

    private void Attach()
    {
        anchorPoint.rotation = Quaternion.Euler(Vector3.zero);
        transform.parent = anchorPoint;
        transform.localPosition = Vector3.zero;
        merge.enabled = false;
    }

    public void Detach()
    {
        transform.parent = null;
        ResetDrag();
        this.enabled = false;
        merge.enabled = true;
        TurnManager.Instance.StartEndTurnCoroutine();

    }

    private void BeginSlinging()
    {
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
        }
        slinging = true;
        StartCoroutine(Slinging());
    }

    private void Launch()
    {
        slinging = false;
        if (Vector3.Distance(transform.position, anchorPoint.position) < 0.1f)
        {
            returnCoroutine = StartCoroutine(ReturnToPoint());
        }
        else
        {
            Detach();
            float amount = Vector3.Distance(transform.position, anchorPoint.position) * data.maxForce / data.maxDistance;
            Vector2 force = (anchorPoint.position - transform.position).normalized * amount;

            rB2d.AddForce(force, ForceMode2D.Impulse);
        }

    }

    private void EndSlinging()
    {
        slinging = false;
        returnCoroutine = StartCoroutine(ReturnToPoint());
    }

    private IEnumerator Slinging()
    {
        aimLine.enabled = true;
        
        gameObject.layer = 2;
        while (slinging)
        {
            
            Vector3 targetPosition = DragInput.Instance.GetTouchPosition();

            Vector3 slingVector = anchorPoint.position - targetPosition;

            float m = Mathf.Min(slingVector.magnitude, data.maxDistance);

            anchorPoint.rotation = Quaternion.LookRotation(Vector3.forward, slingVector);
            
            float currentOffset = transform.localPosition.y;
            float newOffset = -m;
            if (m > Mathf.Abs(currentOffset))
            {
                newOffset = Mathf.Lerp(transform.localPosition.y, -m, data.pullingLerpCoeff);
            }

            transform.localPosition = new Vector3(0, newOffset, 0);

            float raycastDistance = m + m / data.maxDistance * data.aimMaxDistance;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, slingVector, raycastDistance, ~(1 << 2));
            aimLine.SetPosition(0, transform.position);
            if (hit)
            {
                aimLine.SetPosition(1, hit.point);
            }
            else
            {
                aimLine.SetPosition(1, transform.position + slingVector.normalized * raycastDistance);
            }

            yield return new WaitForFixedUpdate();
        }

        aimLine.enabled = false;
        gameObject.layer = 0;
    }

    private IEnumerator ReturnToPoint()
    {

        while (transform.localPosition.magnitude > Mathf.Epsilon)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, data.returnLerpCoeff);

            yield return new WaitForFixedUpdate();
        }

        transform.localPosition = Vector3.zero;

    }

}
