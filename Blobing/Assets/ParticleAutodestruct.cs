using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutodestruct : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destruct", 0.5f);
    }

    void Destruct()
    {
        Destroy(gameObject);
    }
}
