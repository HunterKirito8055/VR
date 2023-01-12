using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuditoriumManager : MonoBehaviour
{
    public Transform auditoriumChairsParent;
    public Transform board;
    public List<AuditoriumChair> auditoriumChairsList = new List<AuditoriumChair>();
    public List<GameObject> studentsPrefabList = new List<GameObject>();
    public bool isHalfAuditoriumRowFill;

    public int noOfStudents;

    public int[] auditoriumRoomData;
    public ClassroomButtons[] auditoriumRoomButtons;
    private void Start()
    {
        for (int i = 0; i < auditoriumRoomData.Length; i++)
        {
            auditoriumRoomButtons[i].Initialize(this, auditoriumRoomData[i]);
        }
    }
    public void SetClassroomStrength(int _classroomStrength)
    {
        AuditoriumChair[] _auditoriumChairsList = auditoriumChairsParent.GetComponentsInChildren<AuditoriumChair>();
        for (int i = 0; i < _auditoriumChairsList.Length; i++)
        {
            auditoriumChairsList.Add(_auditoriumChairsList[i]);
        }

        noOfStudents = Mathf.CeilToInt((float)auditoriumChairsList.Count * (_classroomStrength / 100f));

        ShuffleList(auditoriumChairsList);

        int studentsCount = 0;
        foreach (var auditoriumChair in auditoriumChairsList)
        {
            if (noOfStudents > studentsCount)
            {
                var student = Instantiate(studentsPrefabList[Random.Range(0, studentsPrefabList.Count)], transform);
                auditoriumChair.Initialize(board, student.transform);
                studentsCount++;
            }
            else
            {
                auditoriumChair.Initialize(board, null);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (var classRoomDesk in auditoriumChairsList)
        //{
        //    classRoomDesk.Initialize(board, null);
        //}
    }
    private void ShuffleList(List<AuditoriumChair> auditoriumList)
    {
        // Loop array
        for (int i = auditoriumList.Count - 1; i > 0; i--)
        {
            // Randomize a number between 0 and i (so that the range decreases each time)
            int rnd = Random.Range(0, i);

            // Save the value of the current i, otherwise it'll overwrite when we swap the values
            AuditoriumChair temp = auditoriumList[i];

            // Swap the new and old values
            auditoriumList[i] = auditoriumList[rnd];
            auditoriumList[rnd] = temp;
        }
    }
}
