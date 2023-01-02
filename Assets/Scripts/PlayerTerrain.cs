using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerTerrain : MonoBehaviour
{
    private CharacterController characterController;
    public FixedJoystick joystick;
    public Vector3 move;
    public float speed = 5;
    public float gravity = 20;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
       move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
      // move = new Vector3(joystick.input.x, 0, joystick.input.y);
        move = transform.TransformDirection(move);
        move *= speed * Time.deltaTime;
        move.y -= gravity * Time.deltaTime;
        characterController.Move(move);
    }
}
