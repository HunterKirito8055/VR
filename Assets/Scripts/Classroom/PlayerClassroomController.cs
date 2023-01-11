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
    public bool isVRMode;
    public bool isCharacterSit;
    public Color defaultLayerColor;
    public Color hitLayerColor;
    private float playerCameraUpOffset = -0.22f;
    public float lastClickTime;
    public float clickDelay;
    public int touchCount;
    public Transform canvasTransform;


    // Start is called before the first frame update
    void Start()
    {
        if (isVRMode)
        {
            centerDotMaterial = reticlePointerMeshRenderer.material;
            centerDotMaterial.color = defaultLayerColor;
        }
    }

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
            if (touchCount == 2 || (isVRMode && touchCount == 1))
            {
                Ray ray = !isVRMode ? cam.ScreenPointToRay(Input.mousePosition) : cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                if (Physics.Raycast(ray, out RaycastHit hit, 30f, deskLayer))
                {
                    ClassroomDesk classRoomDesk = hit.collider.gameObject.GetComponent<ClassroomDesk>();
                    if (classRoomDesk != null && !isCharacterSit)
                    {
                        if (classRoomDesk.TryToSit())
                        {
                            if (currentClassroomDesk != null)
                            {
                                currentClassroomDesk.ResetChair();
                            }
                            if (!isVRMode)
                            {
                                characterController.enabled = false;
                                isCharacterSit = true;
                                standupButton.SetActive(true);
                            }
                            transform.position = classRoomDesk.SetPlayerPosition();
                            transform.position = new Vector3(transform.position.x, (isVRMode ? transform.position.y : playerCameraUpOffset), transform.position.z);
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
        RaycastClassroomDesk();
    }

    private void RaycastClassroomDesk()
    {
        if (!isVRMode)
        {
            return;
        }
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, 20f, deskLayer))
        {
            centerDotMaterial.color = hitLayerColor;
        }
        else
        {
            centerDotMaterial.color = defaultLayerColor;
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
        if (!isVRMode)
        {
            touchPanel.Xaxis = lookAt.eulerAngles.x - 360;
            touchPanel.Yaxis = lookAt.eulerAngles.y;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, lookAt.eulerAngles.y - cam.transform.localEulerAngles.y, 0);
        }
        canvasTransform.parent = this.transform;
    }

}
