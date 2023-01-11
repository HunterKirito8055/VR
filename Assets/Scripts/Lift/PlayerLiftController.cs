using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLiftController : MonoBehaviour
{
    public Camera cam;
    public CharacterController characterController;
    public LayerMask liftButtonLayer;
    public LayerMask uiLayer;
    public Transform elevatorParent;
    public Transform defaultParent;
    public Transform thisTransform;
    public bool isLiftEnter;
    public bool isLiftStarted;

    public Material centerDotMaterial;
    public MeshRenderer reticlePointerMeshRenderer;
    public Color hitLayerColor;
    public Color defaultLayerColor;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray =  cam.ScreenPointToRay(Input.mousePosition);
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
                }
            }
            LiftScaleButtonsInteraction();
        }
    }

    #region OnTriggerMethods
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
    #endregion

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
    public void LiftScaleButtonsInteraction()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 20f, uiLayer))
        {
            if (hit.collider.gameObject.GetComponent<Button>() != null)
            {
                hit.collider.gameObject.GetComponent<Button>().onClick.Invoke();
            }
        }
    }
    public void OnLiftSizePositionChange(Vector3 pos)
    {
        characterController.enabled = false;
        transform.position = pos;
        characterController.enabled = true;
    }
}
[System.Serializable]
public struct SizeDimensions
{
    public string lift;
    public float elevatorForwardPosition;
    public float elevatorScaleSize;
    public float reflectionProbePos;
}