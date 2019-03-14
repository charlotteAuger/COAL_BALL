using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    public bool isAvailable = true;

    public virtual void Spawn()
    {
        gameObject.SetActive(true);
        isAvailable = false;
    }

    public virtual void Disable()
    {
        gameObject.SetActive(false);
        isAvailable = true;
    }

}
