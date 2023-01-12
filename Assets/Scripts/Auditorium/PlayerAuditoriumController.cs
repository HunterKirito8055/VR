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
    public bool isCharacterSit;
    private Vector3 playerCameraUpOffset = new Vector3(0, -0.72f, 0);
    public Transform canvasTransform;

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
            if (touchCount == 2)
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 30f, deskLayer))
                {
                    AuditoriumChair auditoriumChair = hit.collider.gameObject.GetComponentInParent<AuditoriumChair>();
                    if (auditoriumChair != null && !isCharacterSit)
                    {
                        if (auditoriumChair.TryToSit())
                        {
                            auditoriumChair.Sit();
                            if (currentChair != null)
                            {
                                currentChair.ResetChair();
                            }
                            characterController.enabled = false;
                            isCharacterSit = true;
                            standupButton.SetActive(true);
                            transform.position = auditoriumChair.SetPlayerPosition();
                            transform.position = new Vector3(transform.position.x, transform.position.y + playerCameraUpOffset.y, transform.position.z);
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

    }
    public void SetCameraPositionOnDesk(Vector3 _board)
    {
        Vector3 diff = (_board - transform.position);
        Quaternion lookAt = Quaternion.LookRotation(diff);
        canvasTransform.parent = cam.transform;

        touchPanel.Xaxis = lookAt.eulerAngles.x / 360;
        touchPanel.Yaxis = lookAt.eulerAngles.y;
        canvasTransform.parent = this.transform;
    }

    public void StandUpFromChair()
    {
        if (currentChair != null)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.5f, transform.localPosition.z + 1);
            currentChair.ResetChair();
            characterController.enabled = true;
            isCharacterSit = false;
        }
    }
}