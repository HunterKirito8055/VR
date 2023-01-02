using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private bool isDragging = false;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject player;

    [SerializeField] private float rotationSensitivity = 5f;
    [SerializeField] private float smoothTime = 0.12f;
    private float Yaxis;
    private float Xaxis;
    public int PointerId;
    public int xMinRotation;
    public int xMaxRotation;

    private Vector2 TouchDist;
    private Vector2 PointerOld;
    private Vector3 currentVel;
    private Vector3 targetRotation;

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerId = eventData.pointerId;
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
        Xaxis = target.transform.eulerAngles.x;
    }
    private void Update()
    {
        if (isDragging)
        {
            if (PointerId >= 0 && PointerId < Input.touches.Length)
            {
                TouchDist = Input.touches[PointerId].position - PointerOld;
                PointerOld = Input.touches[PointerId].position;
            }
            else
            {
                TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
                PointerOld = Input.mousePosition;
            }
        }
        else
        {
            TouchDist = new Vector2();
        }

        Yaxis += TouchDist.x * rotationSensitivity;
        Xaxis -= TouchDist.y * rotationSensitivity;
        Xaxis = Mathf.Clamp(Xaxis, xMinRotation, xMaxRotation);
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVel, smoothTime);

        target.transform.localRotation = Quaternion.Euler(targetRotation.x, 0, 0);
        player.transform.localRotation = Quaternion.Euler(0, targetRotation.y, 0);
    }

  
}
