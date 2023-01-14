using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ToggleGameMode : MonoBehaviour
{
    public void EnableVR(bool _enable)
    {
        StartCoroutine(IEnableVRMode(_enable));
    }
    IEnumerator IEnableVRMode(bool _enable)
    {
        //GameManager.Instance.IsVRMode = _enable;
        yield return null;

#if !UNITY_EDITOR
        // https://docs.unity3d.com/ScriptReference/XR.XRSettings.LoadDeviceByName.html
        // Device names are lowercase, as returned by `XRSettings.supportedDevices`.
        string desiredDevice = _enable ? "cardboard" : "";

        // Some VR Devices do not support reloading when already active, see
        if (String.Compare(XRSettings.loadedDeviceName, desiredDevice, true) != 0)
        {
            XRSettings.LoadDeviceByName(desiredDevice);

            // Must wait one frame after calling `XRSettings.LoadDeviceByName()`.
            yield return null;
        }

        // Now it's ok to enable VR mode.
        XRSettings.enabled = _enable;


#endif
    }
    IEnumerator IDisableVRMode()
    {
        //GameManager.Instance.IsVRMode = _enable;
        yield return null;

        // https://docs.unity3d.com/ScriptReference/XR.XRSettings.LoadDeviceByName.html
        // Device names are lowercase, as returned by `XRSettings.supportedDevices`.
        string desiredDevice = "";

        // Some VR Devices do not support reloading when already active, see
        if (String.Compare(XRSettings.loadedDeviceName, desiredDevice, true) != 0)
        {
            XRSettings.LoadDeviceByName(desiredDevice);

            // Must wait one frame after calling `XRSettings.LoadDeviceByName()`.
            yield return null;
        }

        // Now it's ok to enable VR mode.
        XRSettings.enabled = false;

        load.ReloadCurrentScene();

    }
    public LoadSceneManager load;
    public void DisableVR(bool _enable)
    {
        StartCoroutine(IDisableVRMode());
    }

}
