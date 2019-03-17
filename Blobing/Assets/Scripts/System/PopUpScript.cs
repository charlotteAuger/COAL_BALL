using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private GameObject popupGameObject;
    [SerializeField] private Vector3 offset;

    [SerializeField] private TextMeshProUGUI popupText;

    private void Awake()
    {
        target.GetComponent<PointGiver>().popup = this;
    }

    private void Update()
    {
        if (target.gameObject.activeInHierarchy)
        {
            if (!popupGameObject.activeInHierarchy) { popupGameObject.SetActive(true); }

            transform.position = target.position + offset;
        }
        else
        {
            if (popupGameObject.activeInHierarchy) { popupGameObject.SetActive(false); }
        }
    }


    public IEnumerator DisplayPointChange(int value, bool sign)
    {
        popupText.text = sign ? value.ToString() : (-value).ToString();
        popupText.enabled = true;

        yield return new WaitForSeconds(0.5f);

        popupText.enabled = false;
    }

}
