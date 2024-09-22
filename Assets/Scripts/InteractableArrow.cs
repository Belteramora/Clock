using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InteractableArrow : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform rectTransform;

    private float previousRotation;

    public bool canDrag;

    public UnityEvent<float> onChangeRotation;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        previousRotation = rectTransform.localEulerAngles.z;
        CalculateRotation(eventData.position);

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        CalculateRotation(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        CalculateRotation(eventData.position);
    }

    public void CalculateRotation(Vector2 mousePos)
    {
        mousePos = new Vector2(mousePos.x - Screen.width / 2 + rectTransform.anchoredPosition.x, mousePos.y - Screen.height / 2 + rectTransform.anchoredPosition.y);


        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        rectTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        if (rectTransform.localEulerAngles.z > 350 && previousRotation < 10)
            previousRotation += 360;
        else if (rectTransform.localEulerAngles.z < 10 && previousRotation > 350)
            previousRotation -= 360;


        Debug.Log("localEulerAngles: " + rectTransform.localEulerAngles.z + " prevRot: " + previousRotation);
        var delta = rectTransform.localEulerAngles.z - previousRotation;

        previousRotation = rectTransform.localEulerAngles.z;

        onChangeRotation?.Invoke(delta);
    }
}
