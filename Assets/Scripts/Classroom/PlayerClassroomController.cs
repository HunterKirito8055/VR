using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClassroomController : MonoBehaviour
{
    public ClassroomDesk currentClassroomDesk;
    public CharacterController characterController;
    public GameObject standupButton;
    public TouchPanel touchPanel;
    public LayerMask deskLayer;
    public Camera cam;
    public Material centerDotMaterial;
    public MeshRenderer reticlePointerMeshRenderer;
    public bool isCharacterSit;
    public Color defaultLayerColor;
    public Color hitLayerColor;
    private float playerCameraUpOffset = -0.22f;
    public float lastClickTime;
    public float clickDelay;
    public int touchCount;
    public Transform canvasTransform;



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastClickTime = Time.time;
            touchCount++;
        }
        if (Time.time <= lastClickTime + clickDelay)
        {
            if (touchCount == 2)
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 30f, deskLayer))
                {
                    ClassroomDesk classRoomDesk = hit.collider.gameObject.GetComponentInParent<ClassroomDesk>();
                    if (classRoomDesk != null && !isCharacterSit)
                    {
                        if (classRoomDesk.TryToSit())
                        {
                            if (currentClassroomDesk != null)
                            {
                                currentClassroomDesk.ResetChair();
                            }
                            classRoomDesk.Sit();
                            characterController.enabled = false;
                            isCharacterSit = true;
                            standupButton.SetActive(true);
                            transform.position = classRoomDesk.SetPlayerPosition();
                            transform.position = new Vector3(transform.position.x, playerCameraUpOffset, transform.position.z);
                            currentClassroomDesk = classRoomDesk;
                            SetCameraPositionOnDesk(classRoomDesk.board.position);
                        }
                    }
                }
                touchCount = 0;
            }
        }
        else
        {
            touchCount = 0;
        }
    }

    public void StandUpFromChair()
    {
        if (currentClassroomDesk != null)
        {
            currentClassroomDesk.ResetChair();
            characterController.enabled = true;
            isCharacterSit = false;
        }
    }
    public void SetCameraPositionOnDesk(Vector3 _board)
    {
        Vector3 diff = (_board - transform.position);
        Quaternion lookAt = Quaternion.LookRotation(diff);
        canvasTransform.parent = cam.transform;
        touchPanel.Xaxis = lookAt.eulerAngles.x - 360;
        touchPanel.Yaxis = lookAt.eulerAngles.y;
        canvasTransform.parent = this.transform;
    }
}
