using UnityEngine;
using System.IO;
using TMPro;
using System.Collections;

public class ScreenshotCapture : MonoBehaviour
{
    public int resolutionMultiplier = 1;  // ตัวคูณความละเอียด
    public GameObject textBG;
    [SerializeField] TextMeshProUGUI saveText;
    void Update()
    {
        // กดปุ่ม S เพื่อบันทึกรูปหน้าจอ
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    CaptureScreenshot();
        //}
    }

    public void CaptureScreenshot()
    {
        // ตั้งชื่อไฟล์รูปภาพตามวันที่และเวลา
        string fileName = $"Screenshot_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";

        // กำหนดพาธสำหรับบันทึกในโฟลเดอร์ Gallery ของ Android
        string folderPath = Path.Combine(Application.persistentDataPath, "Screenshots");

        // ตรวจสอบว่ามีโฟลเดอร์หรือไม่ ถ้าไม่มีให้สร้างใหม่
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // สร้างพาธเต็มของไฟล์
        string filePath = Path.Combine(folderPath, fileName);

        // บันทึกรูปหน้าจอ
        ScreenCapture.CaptureScreenshot(filePath, resolutionMultiplier);
        Debug.Log($"Screenshot saved to: {filePath}");
        textBG.gameObject.SetActive(true);
        saveText.text = $"Save image at: {filePath}";
        // รอให้รูปภาพบันทึกเสร็จและแจ้ง Android Media Scanner ให้รู้ว่ามีไฟล์รูปใหม่
        //StartCoroutine(NotifyGallery(filePath));
    }

    System.Collections.IEnumerator NotifyGallery(string filePath)
    {
        yield return new WaitForSeconds(0.5f); // รอให้ไฟล์บันทึกเสร็จ

        // ทำให้ Media Scanner ของ Android รู้จักไฟล์
#if UNITY_ANDROID
        using (AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");

            // สร้าง Intent เพื่อบอก Media Scanner ว่ามีไฟล์รูปใหม่
            AndroidJavaClass mediaScannerConnection = new AndroidJavaClass("android.media.MediaScannerConnection");
            mediaScannerConnection.CallStatic("scanFile", context, new string[] { filePath }, null, null);
        }
#endif
    }
    System.Collections.IEnumerator SaveToDownloads(string tempFilePath, string fileName)
    {
        yield return new WaitForSeconds(0.5f); // รอให้ไฟล์บันทึกเสร็จ

#if UNITY_ANDROID
        // เข้าถึงฟังก์ชัน Android เพื่อดึงพาธโฟลเดอร์ Downloads
        using (AndroidJavaClass environment = new AndroidJavaClass("android.os.Environment"))
        {
            string downloadsPath = environment.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", environment.GetStatic<string>("DIRECTORY_DOWNLOADS")).Call<string>("getAbsolutePath");

            // สร้างพาธไฟล์ในโฟลเดอร์ Downloads
            string filePath = Path.Combine(downloadsPath, fileName);

            // คัดลอกไฟล์จากพาธชั่วคราวไปที่โฟลเดอร์ Downloads
            File.Copy(tempFilePath, filePath, true);

            Debug.Log($"Screenshot saved to Downloads: {filePath}");

            // แจ้งให้ Media Scanner ของ Android ทราบว่ามีไฟล์ใหม่
            using (AndroidJavaClass mediaScannerConnection = new AndroidJavaClass("android.media.MediaScannerConnection"))
            {
                using (AndroidJavaObject currentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
                    mediaScannerConnection.CallStatic("scanFile", context, new string[] { filePath }, null, null);
                }
            }
        }
#endif
    }
    public void CaptureMyScreenshot()
    {
        StartCoroutine(TakeScreenshotAndSave());
    }

    private IEnumerator TakeScreenshotAndSave()
    {
        yield return new WaitForEndOfFrame();
        textBG.gameObject.SetActive(true);
        saveText.text = $"Save image at: {Application.productName}";
        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        // Save the screenshot to Gallery/Photos
        string name = string.Format("{0}_Capture{1}_{2}.png", Application.productName, "{0}", System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(ss, Application.productName + " Captures", name));
    }
}
