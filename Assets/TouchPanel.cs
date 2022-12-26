using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private bool isDragging = false;
    [SerializeField] private GameObject target;

    [SerializeField] private float rotationSensitivity = 5f;
    [SerializeField] private float smoothTime = 0.12f;
    private float Yaxis;
    private float Xaxis;

    private Vector2 TouchDist;
    private Vector2 PointerOld;
    private Vector3 currentVel;
    private Vector3 targetRotation;

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerOld = eventData.position;
        isDragging = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    private void Awake()
    {
        Yaxis = target.transform.eulerAngles.y;
     //   Xaxis = target.transform.eulerAngles.y;
    }
    private void Update()
    {
        if (isDragging)
        {
            TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
            PointerOld = Input.mousePosition;
        }
        else
        {
            TouchDist = new Vector2();
        }

        Yaxis += TouchDist.x * rotationSensitivity;
        // Xaxis -= TouchDist.y * rotationSensitivity;

        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(0, Yaxis), ref currentVel, smoothTime);
        target.transform.eulerAngles = targetRotation;
    }


}
