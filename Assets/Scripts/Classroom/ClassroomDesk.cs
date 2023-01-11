using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassroomDesk : MonoBehaviour
{
    public bool isDeskFilled;
    private Vector3 playerCameraOffset = new Vector3(0, -0.46f, 0);
    public Transform board;
    public Transform chair;
    public Transform student;
    public Vector3 chairInitialPos;
    private Vector3 chairSittingPos = new Vector3(0, 0, 0.425f);
    private Vector3 characterSittingPos = new Vector3(0, 0.252f, -0.119f);

    void Awake()
    {
        chair = this.transform.GetChild(0);
        chairInitialPos = chair.localPosition;
    }
    public void Initialize(Transform _board, Transform _student)
    {
        board = _board;
        student = _student;
        if (student != null)
        {
            isDeskFilled = true;
            chair.localPosition = chairSittingPos;
            student.parent = chair;
            student.transform.localPosition = characterSittingPos;
        }
    }
    public bool TryToSit()
    {
        if (!isDeskFilled)
        {
            isDeskFilled = true;
            chair.localPosition = chairSittingPos;
            return true;
        }
        return false;
    }
    public void ResetChair()
    {
        chair.localPosition = chairInitialPos;
        isDeskFilled = false;
    }
    public Vector3 SetPlayerPosition()
    {
        return chair.position + playerCameraOffset;
    }
}
