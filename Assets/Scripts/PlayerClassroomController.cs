using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerClassroomController : MonoBehaviour
{
    public ClassroomDesk currentClassroomDesk;
    public CharacterController characterController;
    public GameObject standupButton;
    public LayerMask deskLayer;
    public Camera cam;
    public Material centerDotMaterial;
    public MeshRenderer reticlePointerMeshRenderer;
    public bool isVRMode;
    public bool isCharacterSit;
    public Color defaultLayerColor;
    public Color hitLayerColor;
    private float playerCameraUpOffset = -0.22f;


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
                            currentClassroomDesk.ResetDesk();
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
                    }
                }
            }
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
            currentClassroomDesk.ResetDesk();
            characterController.enabled = true;
            isCharacterSit = false;
        }
    }
}
