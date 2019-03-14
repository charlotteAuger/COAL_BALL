using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private SpriteRenderer renderer;
    private GameObject spriteObject;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        spriteObject = target.GetComponentInChildren<SpriteRenderer>().gameObject;
    }

    private void Update()
    {
        if (target.gameObject.activeInHierarchy)
        {
            if (!renderer.enabled) { renderer.enabled = true; }

            transform.position = target.position + offset;
            transform.localScale = spriteObject.transform.localScale; 
        }
        else
        {
            if (renderer.enabled) { renderer.enabled = false; }
        }
    }
}
