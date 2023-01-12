using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ClassroomManager : MonoBehaviour
{
    public Transform board;
    public List<ClassroomDesk> classroomDesksList = new List<ClassroomDesk>();
    public List<GameObject> studentsPrefabList = new List<GameObject>();
    public bool isHalfClassroomFill;

    public int noOfStudents;

    public int[] classRoomData;
    public ClassroomButtons[] classRoomButtons;
    private void Start()
    {
        for (int i = 0; i < classRoomData.Length; i++)
        {
            classRoomButtons[i].Initialize(this, classRoomData[i]);
        }
    }
    public void SetClassroomStrength(int _classroomStrength)
    {
        noOfStudents = Mathf.CeilToInt((float)classroomDesksList.Count * (_classroomStrength / 100f));
        ShuffleList(classroomDesksList);

        int studentsCount = 0;
        foreach (var classRoomDesk in classroomDesksList)
        {
            if (noOfStudents > studentsCount)
            {
                var student = Instantiate(studentsPrefabList[Random.Range(0, studentsPrefabList.Count)], transform);
                classRoomDesk.Initialize(board, student.transform);
                studentsCount++;
            }
            else
            {
                classRoomDesk.Initialize(board, null);
            }
        }
    }

    private void ShuffleList(List<ClassroomDesk> _classroomDesksList)
    {
        // Loop array
        for (int i = _classroomDesksList.Count - 1; i > 0; i--)
        {
            // Randomize a number between 0 and i (so that the range decreases each time)
            int rnd = Random.Range(0, i);

            // Save the value of the current i, otherwise it'll overwrite when we swap the values
            ClassroomDesk temp = _classroomDesksList[i];

            // Swap the new and old values
            _classroomDesksList[i] = _classroomDesksList[rnd];
            _classroomDesksList[rnd] = temp;
        }
    }
}
