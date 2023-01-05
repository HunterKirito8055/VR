using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLiftController : MonoBehaviour
{
    public Camera cam;
    public CharacterController characterController;
    public elevControl elevcontrol;
    public LayerMask liftButtonLayer;
    public LayerMask vrFloorLayer;
    public LayerMask reticlePointLayers;
    public Transform elevatorParent;
    public Transform defaultParent;
    public Transform thisTransform;
    public bool isLiftEnter;
    public bool isLiftStarted;
    public bool isVRMode;

    public Material centerDotMaterial;
    public MeshRenderer reticlePointerMeshRenderer;
    public Color hitLayerColor;
    public Color defaultLayerColor;

    private void Start()
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
            if (Physics.Raycast(ray, out RaycastHit hit, 20f, liftButtonLayer))
            {
                if (hit.collider != null)
                {
                    ElevatorButton elevatorButton = hit.collider.gameObject.GetComponent<ElevatorButton>();
                    elevHallFrameController callElevatorButton = hit.collider.gameObject.GetComponentInParent<elevHallFrameController>();

                    if (elevatorButton)
                    {
                        StartLift();
                        elevatorButton.CallElevator();
                    }
                    else if (callElevatorButton)
                    {
                        callElevatorButton.CallElevator();
                    }
                    else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("VRFloor"))
                    {
                        if (isLiftEnter && isLiftStarted)
                        {
                            return;
                        }
                        transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                    }
                }
            }
        }
        RaycastLiftSceneLayers();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Elevator")
        {
            isLiftEnter = true;
            thisTransform.parent = elevatorParent;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Elevator")
        {
            isLiftEnter = false;
            thisTransform.parent = defaultParent;
        }
    }
    public void StartLift()
    {
        characterController.enabled = false;
        isLiftStarted = true;
    }

    public void LiftDoorOpened()
    {
        isLiftStarted = false;
        if (isLiftEnter)
        {
            characterController.enabled = true;
        }
    }
    public void RaycastLiftSceneLayers()
    {
        if (!isVRMode)
        {
            return;
        }
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, 20f, reticlePointLayers))
        {
            if (isLiftEnter && hit.collider.gameObject.layer == LayerMask.NameToLayer("ElevatorButton"))
            {
                centerDotMaterial.color = defaultLayerColor;
            }
            else
            {
                centerDotMaterial.color = hitLayerColor;
            }
        }
        else
        {
            centerDotMaterial.color = defaultLayerColor;
        }
    }

}
