using UnityEngine;
using UnityEditor;

namespace SavingBrainrot
{
    public class ScreenshotUtility
    {
        [MenuItem("Tools/Take Screenshot")]
        public static void CaptureScreenshot()
        {
            string filename = $"Screenshot_{System.DateTime.Now:yyyyMMdd-HHmmss}.png";
            string path = System.IO.Path.Combine(Application.dataPath, filename);
            ScreenCapture.CaptureScreenshot(path);
            Debug.Log($"Скриншот успешно сохранён: {path}");
        }
    }
}