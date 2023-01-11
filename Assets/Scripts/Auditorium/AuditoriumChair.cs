using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuditoriumChair : MonoBehaviour
{
    public bool isDeskFilled;
    public Transform board;
    public Transform chair;
    public Transform student;
    private Vector3 playerCameraOffset = new Vector3(0.47f, 0f, -0.3f);
    private Vector3 studentSittingPos = new Vector3(-0.091f, 0.00141f, 0.1164f);
    private Vector3 studentSittingRot = new Vector3(90, 90.00001f, -90.00001f);
    private Vector3 studentScaleSize = new Vector3(1.5f, 1.5f, 1.5f);
    private void Awake()
    {
        chair = transform;
    }
    public void Initialize(Transform _board, Transform _student)
    {
        board = _board;
        student = _student;
        if (student != null)
        {
            isDeskFilled = true;

            student.transform.localScale = studentScaleSize;
            student.parent = chair;
            student.transform.localPosition = studentSittingPos;
            student.transform.localEulerAngles = studentSittingRot;
        }
    }
    public bool TryToSit()
    {
        if (!isDeskFilled)
        {
            isDeskFilled = true;
            return true;
        }
        return false;
    }
    public Vector3 SetPlayerPosition()
    {
        return chair.position + playerCameraOffset;
    }
    public void ResetChair()
    {
        isDeskFilled = false;
    }
}
