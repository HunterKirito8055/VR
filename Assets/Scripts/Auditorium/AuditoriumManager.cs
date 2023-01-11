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
    public (int, int) halfAuditoriumRandomStrength;
    public (int, int) fullAuditoriumRandomStrength;
    private void Start()
    {
        AuditoriumChair[] _auditoriumChairsList = auditoriumChairsParent.GetComponentsInChildren<AuditoriumChair>();
        for (int i = 0; i < _auditoriumChairsList.Length; i++)
        {
            auditoriumChairsList.Add(_auditoriumChairsList[i]);
        }
        halfAuditoriumRandomStrength = (auditoriumChairsList.Count / 2, (auditoriumChairsList.Count / 2) + 3);
        fullAuditoriumRandomStrength = (auditoriumChairsList.Count - 3, auditoriumChairsList.Count - 5);

        noOfStudents = isHalfAuditoriumRowFill ? Random.Range(halfAuditoriumRandomStrength.Item1, halfAuditoriumRandomStrength.Item2) :
         Random.Range(fullAuditoriumRandomStrength.Item1, fullAuditoriumRandomStrength.Item2);

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
        foreach (var classRoomDesk in auditoriumChairsList)
        {
            classRoomDesk.Initialize(board, null);
        }
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
