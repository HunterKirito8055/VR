using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassroomButtons : MonoBehaviour
{
    public Text text;
    public Button button;

    public void Initialize(ClassroomManager classroomManager,int strength)
    {
        text.text = strength.ToString() + "%";
        button.onClick.AddListener(() => classroomManager.SetClassroomStrength(strength));
    } 
    public void Initialize(AuditoriumManager auditoriumManager, int strength)
    {
        text.text = strength.ToString() + "%";
        button.onClick.AddListener(() => auditoriumManager.SetClassroomStrength(strength));
    }
}
