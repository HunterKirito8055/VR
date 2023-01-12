using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVRAuditoriumController : MonoBehaviour
{
    public AuditoriumChair currentChair;
    public GameObject standupButton;
    public LayerMask deskLayer;
    public Camera cam;
    public bool isCharacterSit;
    public Transform canvasTransform;
    public Image gazeImg;
    public float gvrTimer;
    public float totalTime = 2;
    private Vector3 playerCameraUpOffset = new Vector3(0, 3f, 0);
    public Vector3 playerdefaultoffset = new Vector3(0, 2.09f, 0);
    public Vector3 playergetupOffset = new Vector3(0, 2.09f, 0);

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, 30f, deskLayer) && !isCharacterSit)
        {
            AuditoriumChair auditoriumChair = hit.collider.gameObject.GetComponentInParent<AuditoriumChair>();
            if (auditoriumChair != null && !isCharacterSit)
            {
                if (auditoriumChair.TryToSit())
                {
                    gvrTimer += Time.deltaTime;
                    gazeImg.fillAmount = gvrTimer / totalTime;
                    if (gazeImg.fillAmount >= 1)
                    {
                        auditoriumChair.Sit();
                        if (currentChair != null)
                        {
                            currentChair.ResetChair();
                        }
                        isCharacterSit = true;
                        standupButton.SetActive(true);
                        transform.position = auditoriumChair.SetPlayerPosition();
                        transform.position = new Vector3(transform.position.x, transform.position.y - playerCameraUpOffset.y, transform.position.z);
                        currentChair = auditoriumChair;
                        SetCameraPositionOnDesk(auditoriumChair.board.position);
                        ResetGaze();
                    }
                }
                else
                {
                    ResetGaze();
                }
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("VRFloor"))
            {
                gvrTimer += Time.deltaTime;
                gazeImg.fillAmount = gvrTimer / totalTime;
                if (gazeImg.fillAmount >= 1)
                {
                    transform.position = new Vector3(hit.point.x, hit.point.y - playerdefaultoffset.y, hit.point.z);
                    ResetGaze();
                }
            }
        }
        else
        {
            ResetGaze();
        }
    }
    public void SetCameraPositionOnDesk(Vector3 _board)
    {
        Vector3 diff = (_board - transform.position);
        Quaternion lookAt = Quaternion.LookRotation(diff);
        canvasTransform.parent = cam.transform;
        transform.rotation = Quaternion.Euler(0, lookAt.eulerAngles.y - cam.transform.localEulerAngles.y, 0);
        canvasTransform.parent = this.transform;
    }
    public void ResetGaze()
    {
        gazeImg.fillAmount = 0;
        gvrTimer = 0;
    }
    public void StandUpFromChair()
    {
        if (currentChair != null)
        {
            currentChair.ResetChair();
            transform.position = new Vector3(transform.position.x, transform.position.y + playergetupOffset.y, transform.position.z + 1);
            isCharacterSit = false;
        }
    }
}
