using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Transitionmanual : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject manual;

    private void Start()
    {
        manual.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        manual.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        manual.SetActive(false);
    }
}
