using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAuditoriumController : MonoBehaviour
{
    public AuditoriumChair currentChair;
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
    private Vector3 playerCameraUpOffset = new Vector3(0, 0f, 0);
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
    public float lastClickTime;
    public float clickDelay;
    public int touchCount;
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
                    AuditoriumChair auditoriumChair = hit.collider.gameObject.GetComponentInParent<AuditoriumChair>();
                    if (auditoriumChair != null && !isCharacterSit)
                    {
                        if (auditoriumChair.TryToSit())
                        {
                            if (currentChair != null)
                            {
                                currentChair.ResetChair();
                            }
                            if (!isVRMode)
                            {
                                characterController.enabled = false;
                                isCharacterSit = true;
                                standupButton.SetActive(true);
                            }
                            transform.position = auditoriumChair.SetPlayerPosition();
                            transform.position = new Vector3(transform.position.x, (isVRMode ? transform.position.y : transform.position.y + playerCameraUpOffset.y), transform.position.z);
                            currentChair = auditoriumChair;
                            SetCameraPositionOnDesk(auditoriumChair.board.position);
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
    public void SetCameraPositionOnDesk(Vector3 _board)
    {
        Vector3 diff = (_board - transform.position);
        Quaternion lookAt = Quaternion.LookRotation(diff);
        canvasTransform.parent = cam.transform;

        if (!isVRMode)
        {
            touchPanel.Xaxis = lookAt.eulerAngles.x / 360;
         
            touchPanel.Yaxis = lookAt.eulerAngles.y;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, lookAt.eulerAngles.y - cam.transform.localEulerAngles.y, 0);
        }
        canvasTransform.parent = this.transform;

    }

    #region VR
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
    #endregion

    #region Normal
    public void StandUpFromChair()
    {
        if (currentChair != null)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 1);
            currentChair.ResetChair();
            characterController.enabled = true;
            isCharacterSit = false;
        }
    }
    #endregion
}