using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerVRLiftController : MonoBehaviour
{
    public Camera cam;
    public LayerMask liftButtonLayer;
    public LayerMask reticlePointLayers;
    public Transform elevatorParent;
    public Transform defaultParent;
    public Transform thisTransform;
    public bool isLiftEnter;
    public bool isLiftStarted;

    public Material centerDotMaterial;
    public MeshRenderer reticlePointerMeshRenderer;
    public Color hitLayerColor;
    public Color defaultLayerColor;

    private void Start()
    {
        centerDotMaterial = reticlePointerMeshRenderer.material;
        centerDotMaterial.color = defaultLayerColor;
    }
    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, 20f, liftButtonLayer))
        {
            if (hit.collider != null)
            {
                ElevatorButton elevatorButton = hit.collider.gameObject.GetComponent<ElevatorButton>();
                elevHallFrameController callElevatorButton = hit.collider.gameObject.GetComponentInParent<elevHallFrameController>();
                if (elevatorButton && !isElevatorCall)
                {
                    gvrTimer += Time.deltaTime;
                    gazeImg.fillAmount = gvrTimer / totalTime;
                    if (gazeImg.fillAmount >= 1)
                    {
                        isElevatorCall = true;
                        StartLift();
                        elevatorButton.CallElevator();
                        ResetGaze();
                    }
                }
                else if (callElevatorButton && !isElevatorCall)
                {
                    gvrTimer += Time.deltaTime;
                    gazeImg.fillAmount = gvrTimer / totalTime;
                    if (gazeImg.fillAmount >= 1)
                    {
                        isElevatorCall = true;
                        callElevatorButton.CallElevator();
                        ResetGaze();
                    }
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("VRFloor"))
                {
                    if (isLiftEnter && isLiftStarted)
                    {
                        return;
                    }
                    gvrTimer += Time.deltaTime;
                    gazeImg.fillAmount = gvrTimer / totalTime;
                    if (gazeImg.fillAmount >= 1)
                    {
                        transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                        ResetGaze();
                    }
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("VRLiftFloor") && !isElevatorCall)
                {
                    if (isLiftEnter && isLiftStarted)
                    {
                        return;
                    }
                    gvrTimer += Time.deltaTime;
                    gazeImg.fillAmount = gvrTimer / totalTime;
                    if (gazeImg.fillAmount >= 1)
                    {
                        transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                        ResetGaze();
                    }
                }
                else
                {
                    ResetGaze();
                }
            }
        }
        else
        {
            ResetGaze();
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
        isLiftStarted = true;
    }
    public void LiftDoorOpened()
    {
        isLiftStarted = false;
        isElevatorCall = false;
    }
    public void LiftDoorClosed()
    {
        isElevatorCall = true;
    }
    public void OnLiftSizePositionChange(Vector3 pos)
    {
        transform.position = pos;
    }
    public void RaycastLiftSceneLayers()
    {
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

    public Image gazeImg;
    public float gvrTimer;
    public float totalTime = 2;
    public bool isElevatorCall;
    public void ResetGaze()
    {
        gazeImg.fillAmount = 0;
        gvrTimer = 0;
    }
}
