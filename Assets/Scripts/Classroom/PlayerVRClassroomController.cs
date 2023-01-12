using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVRClassroomController : MonoBehaviour
{
    public ClassroomDesk currentClassroomDesk;
    public GameObject standupButton;
    public LayerMask deskLayer;
    public Camera cam;
    public Material centerDotMaterial;
    public MeshRenderer reticlePointerMeshRenderer;
    public bool isCharacterSit;
    public Color defaultLayerColor;
    public Color hitLayerColor;
    public Transform canvasTransform;
    public Image gazeImg;
    public float gvrTimer;
    public float totalTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        centerDotMaterial = reticlePointerMeshRenderer.material;
        centerDotMaterial.color = defaultLayerColor;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, 30f, deskLayer) && !isCharacterSit)
        {
            ClassroomDesk classRoomDesk = hit.collider.gameObject.GetComponentInParent<ClassroomDesk>();
            if (classRoomDesk != null && !isCharacterSit)
            {
                if (classRoomDesk.TryToSit())
                {
                    gvrTimer += Time.deltaTime;
                    gazeImg.fillAmount = gvrTimer / totalTime;
                    if (gazeImg.fillAmount >= 1)
                    {
                        classRoomDesk.Sit();
                        if (currentClassroomDesk != null)
                        {
                            currentClassroomDesk.ResetChair();
                        }
                        isCharacterSit = true;
                        standupButton.SetActive(true);
                        transform.position = classRoomDesk.SetPlayerPosition();
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                        currentClassroomDesk = classRoomDesk;
                        SetCameraPositionOnDesk(classRoomDesk.board.position);
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
                    transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                    ResetGaze();
                }
            }
        }
        else
        {
            ResetGaze();
        }
        // RaycastClassroomDesk();
    }
    public void StandUpFromChair()
    {
        if (currentClassroomDesk != null)
        {
            currentClassroomDesk.ResetChair();
            transform.position = new Vector3(transform.position.x, 0, transform.position.z - 1);
            isCharacterSit = false;
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
}
