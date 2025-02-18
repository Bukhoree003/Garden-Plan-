using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class CameraPermissionChecker : MonoBehaviour
{
    void Start()
    {
#if UNITY_ANDROID
        // เช็คว่าแอปมีสิทธิ์เข้าถึงกล้องหรือไม่
        if (!HasCameraPermission())
        {
            // ขอสิทธิ์จากผู้ใช้
            RequestCameraPermission();
        }
        else
        {
            Debug.Log("Camera permission already granted.");
            StartCamera();
        }
#endif
    }

#if UNITY_ANDROID
    bool HasCameraPermission()
    {
        return Permission.HasUserAuthorizedPermission(Permission.Camera);
    }

    void RequestCameraPermission()
    {
        Permission.RequestUserPermission(Permission.Camera);
    }
#endif

    void StartCamera()
    {
        // ใส่โค้ดสำหรับเริ่มการใช้กล้องที่นี่
        Debug.Log("Starting camera...");
    }
}
