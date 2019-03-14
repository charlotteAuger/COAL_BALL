using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DragInput : MonoBehaviour {

    public static DragInput Instance;

    public delegate void InputEvent();
    public InputEvent BeginSling;
    public InputEvent QuitSling;
    public InputEvent LaunchSling;

    private bool slinging = false;
    [SerializeField] private Camera cam;


    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else if (Instance != this) { Destroy(gameObject); }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
;
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(touch.position);


            if (Physics.Raycast(ray, out hit))// LayerMask.NameToLayer("InputDetection")))
            {
                if (!slinging)
                {
                    slinging = true;
                    if (BeginSling != null)
                    {
                        BeginSling();
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    slinging = false;
                    if (LaunchSling != null)
                    {
                        LaunchSling();
                    }
                }
            }
            else if (slinging)
            {
                slinging = false;
                if (QuitSling != null)
                {
                    QuitSling();
                }
            }
        }

        else 
        {
            
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out hit))// LayerMask.NameToLayer("InputDetection")))
            {
                if (!slinging && Input.GetMouseButton(0))
                {
                    slinging = true;
                    if (BeginSling != null)
                    {
                        BeginSling();
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    slinging = false;
                    if (LaunchSling != null)
                    {
                        LaunchSling();
                    }
                }
            }
            else if (slinging)
            {
                slinging = false;
                if (QuitSling != null)
                {
                    QuitSling();
                }
            }
        }
    }

    public Vector3 GetTouchPosition()
    {
        Vector2 screenPosition = Input.mousePosition;

        if (Input.touchCount > 0)
        {
            screenPosition = Input.GetTouch(0).position;
        }

        Vector3 worldPosition = cam.ScreenToWorldPoint(screenPosition);
        worldPosition = new Vector3(worldPosition.x, worldPosition.y, 0);
            

        return worldPosition;
    }
}
