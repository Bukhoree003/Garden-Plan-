using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using NativeCameraNamespace;
using TMPro;
using System;

public class CameraCapture : MonoBehaviour
{
    public static CameraCapture Instance;
    [SerializeField] bool resetOnReOpenCamera = false;
    public RawImage rawImage;  // RawImage สำหรับแสดงผลภาพจากกล้อง
    public Texture defaultTexture;  // รูปภาพที่จะแสดงเมื่อกล้องไม่ทำงาน
    public WebCamTexture webCamTexture;  // ตัวแปรสำหรับกล้อง
    public Button captureButton;  // ปุ่มถ่ายภาพ
    public Image capturedImageDisplay;  // Image สำหรับแสดงภาพที่ถ่ายได้
    public GameObject camPanel;
    Sprite s;
    [SerializeField] TextMeshProUGUI camSizeText;
    [SerializeField] RectTransform canvas;
    [SerializeField] GameObject alert;
    [SerializeField] TextMeshProUGUI textAlert;
    [SerializeField] Toggle toggleDeleteCamera;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        alert.SetActive(false);
        textAlert.text = "";
        OpenCamera();
        // ตรวจสอบว่าอุปกรณ์มีเว็บแคมหรือไม่
        if (WebCamTexture.devices.Length > 0)
        {
            string deviceName = "";
            foreach (WebCamDevice item in WebCamTexture.devices)
            {
                if (item.name != "LSVCam" && item.name != "VTubeStudioCam" && item.name != "OBS Virtual Camera")
                {
                    Debug.Log($"device name: {item.name}");
                    deviceName = item.name;
                    
                    break;
                }
            }
            if (deviceName != "")
            {
                // สร้าง WebCamTexture ด้วยกล้องตัวแรก
                webCamTexture = new WebCamTexture(deviceName);
                rawImage.texture = webCamTexture;  // แสดงภาพกล้องใน RawImage
                rawImage.material.mainTexture = webCamTexture;

                // เริ่มการแสดงผลภาพจากกล้อง
                webCamTexture.Play();
            }else
            {
                alert.SetActive(true);
                textAlert.text = "No camera device detected!";
            }

        }
        else
        {
            Debug.LogError("No camera detected!");
            rawImage.texture = defaultTexture;  // ถ้าไม่พบกล้อง แสดงภาพ default แทน
        }

        // ผูกฟังก์ชันถ่ายภาพกับปุ่ม
        captureButton.onClick.AddListener(CapturePhoto);
        toggleDeleteCamera.onValueChanged.AddListener(ToggleDeleteObjects);
        //StartCoroutine(GetCameraSize());
    }

    private void ToggleDeleteObjects(bool arg0)
    {
        resetOnReOpenCamera = arg0;
    }

    private IEnumerator GetCameraSize()
    {
        // รอจนกว่ากล้องจะเริ่มทำงาน
        while (webCamTexture.width <= 16 || webCamTexture.height <= 16)
        {
            yield return null; // รอเฟรมถัดไป
        }

        // ตอนนี้คุณสามารถดึงขนาดของ WebCamTexture ได้
        Debug.Log("WebCamTexture Width: " + webCamTexture.width);
        Debug.Log("WebCamTexture Height: " + webCamTexture.height);
        camSizeText.text= "WebCamTexture Width: " + webCamTexture.width + "\nWebCamTexture Height: " + webCamTexture.height;
        // สามารถนำขนาดนี้ไปปรับขนาด UI หรืออะไรก็ได้ที่ต้องการ
        rawImage.rectTransform.sizeDelta = new Vector2(webCamTexture.width, webCamTexture.height);
        // ขนาดของ WebCamTexture
        float cameraWidth = webCamTexture.width;
        float cameraHeight = webCamTexture.height;

        // ขนาดของหน้าจอ
        float screenWidth = canvas.rect.width;
        float screenHeight = canvas.rect.height;
        float rate = Screen.width / cameraWidth;
        cameraWidth *= rate;
        cameraHeight *= rate;
        rawImage.rectTransform.sizeDelta = new Vector2(cameraWidth, cameraHeight);

        //// คำนวณสัดส่วนของกล้องและหน้าจอ
        //float cameraAspectRatio = cameraWidth / cameraHeight;
        //float screenAspectRatio = screenWidth / screenHeight;

        //// ปรับขนาดของ RawImage ตามสัดส่วน
        //if (cameraAspectRatio > screenAspectRatio)
        //{
        //    // ถ้าสัดส่วนกล้องกว้างกว่าสัดส่วนหน้าจอ
        //    float newHeight = screenWidth / cameraAspectRatio;
        //    rawImage.rectTransform.sizeDelta = new Vector2(screenWidth, newHeight);
        //}
        //else
        //{
        //    // ถ้าสัดส่วนกล้องสูงกว่าสัดส่วนหน้าจอ
        //    float newWidth = screenHeight * cameraAspectRatio;
        //    rawImage.rectTransform.sizeDelta = new Vector2(newWidth, screenHeight);
        //}

        // จัดตำแหน่งให้อยู่กลางหน้าจอ
        rawImage.rectTransform.anchoredPosition = Vector2.zero;
    }
    // ฟังก์ชันสำหรับถ่ายภาพ
    void CapturePhoto()
    {
        if (webCamTexture != null)
        {
            // ถ่ายภาพจากกล้องในขณะที่มันกำลังแสดงผล
            Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
            photo.SetPixels(webCamTexture.GetPixels());
            photo.Apply();

            // นำภาพที่ถ่ายได้ไปแสดงใน Image ที่กำหนด
            s = Sprite.Create(photo, new Rect(0, 0, photo.width, photo.height), new Vector2(0.5f, 0.5f));
            capturedImageDisplay.sprite = s;
            Debug.Log("Photo Captured!");
            camPanel.gameObject.SetActive(false);
            if (webCamTexture != null)
                webCamTexture.Pause();
        }
    }
    public void OpenCamera()
    {
        if (resetOnReOpenCamera)
        {
            foreach (Transform item in BuildingManager.Instance.container)
            {
                Destroy(item.gameObject);
            }
        }
        camPanel.gameObject.SetActive(true);
        if (webCamTexture != null)
            webCamTexture.Play();
    }
    // หยุดการทำงานของกล้องเมื่อออกจากแอป
    void OnDisable()
    {
        if (webCamTexture != null)
        {
            webCamTexture.Stop();
        }
    }
}
