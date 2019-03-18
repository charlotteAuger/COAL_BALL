using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private Color playerColor;
    [SerializeField] private Color aiColor;

    private Coroutine popUpCo;

    private void Awake()
    {
        target.GetComponent<PointGiver>().popup = this;
    }

    public void StartDisplay(int value, bool sign, bool isPlayer)
    {
        if (popUpCo == null)
        {
            popUpCo = StartCoroutine(DisplayPointChange(value, sign, isPlayer));
        }
        else
        {
            StopCoroutine(popUpCo);
            popUpCo = StartCoroutine(DisplayPointChange(value, sign, isPlayer));
        }
    }

    public IEnumerator DisplayPointChange(int value, bool sign, bool isPlayer)
    {
        transform.position = target.position + offset;
        popupText.text = sign ? "+"+value.ToString() : (-value).ToString();
        popupText.color = isPlayer ? playerColor : aiColor;
        yield return null;
        popupText.enabled = true;

        yield return new WaitForSeconds(0.5f);

        popupText.enabled = false;
        popUpCo = null;
    }

    public void ClearPopup()
    {
        if (popUpCo != null)
        {
            StopCoroutine(popUpCo);
            popupText.enabled = false;

        }
    }

}
