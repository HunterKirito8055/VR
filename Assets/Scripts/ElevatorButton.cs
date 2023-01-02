using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : MonoBehaviour
{
    public elevControl elevcontrol;
    public int floor;

    public void CallElevator()
    {
        elevcontrol.SetElevatorFloor(floor);
    }
}
