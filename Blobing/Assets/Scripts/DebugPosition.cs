using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DebugPosition : MonoBehaviour
{
    public TextMeshProUGUI debug;  

    void Update () {
        debug.text = "Position : " + transform.position;
	}
}
