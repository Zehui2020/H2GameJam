using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Used to check whether the device being played on is WebGL or Mobile
/// </summary>
public class GamePlatformChecker : MonoBehaviour
{

    public enum DeviceType
    { 
        Mobile,
        WebGL,
        Other,
    }

    public DeviceType deviceType;
    public static GamePlatformChecker gamePlatformInstance;

    private void Awake()
    {
        if (gamePlatformInstance == null)
        {
            gamePlatformInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (gamePlatformInstance != this)
        {
            Destroy(gameObject); 
        }

        SetUp();
    }

    private void SetUp()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("Running on Android");
            deviceType = DeviceType.Mobile;
        }
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.Log("Running on WebGL");
            deviceType = DeviceType.WebGL;
        }
        else
        {
            Debug.Log("Running on another platform");
            deviceType = DeviceType.WebGL;
        }
    }


}
