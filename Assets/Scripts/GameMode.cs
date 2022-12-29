using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMode : MonoBehaviour
{
    public GameObject vrMode;
    public GameObject normalMode;
    private void Start()
    {
        vrMode.gameObject.SetActive(GameManager.Instance.IsVRMode);
        normalMode.gameObject.SetActive(!GameManager.Instance.IsVRMode);
    }
}
