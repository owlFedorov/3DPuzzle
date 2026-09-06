using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class TransparentScreenshot : MonoBehaviour
{
    public Camera renderCam; // Перетащите сюда вашу настроенную камеру

    // Папка относительно корня проекта (где лежит папка Assets)
    private const string RelativePath = "Assets/Textures/Items";

    void Update()
    {
        if (Keyboard.current.f12Key.IsPressed())
        {
            StartCoroutine(Capture());
        }
    }

    private System.Collections.IEnumerator Capture()
    {
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;

        RenderTexture rt = new RenderTexture(width, height, 32);
        renderCam.targetTexture = rt;
        renderCam.Render();

        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.ARGB32, false, true);
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();

        // Сбрасываем настройки камеры
        renderCam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();

        // Формируем абсолютный путь к папке внутри проекта
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        string absoluteFolderPath = Path.Combine(projectRoot, RelativePath);

        // Создаем папку, если ее нет
        if (!Directory.Exists(absoluteFolderPath))
        {
            Directory.CreateDirectory(absoluteFolderPath);
        }

        string fileName = "item_" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";
        string fullFilePath = Path.Combine(absoluteFolderPath, fileName);

        File.WriteAllBytes(fullFilePath, bytes);

        Debug.Log("Скриншот сохранен в проект: " + fullFilePath);

        // Принудительно импортируем текстуру, чтобы она появилась в окне Project
#if UNITY_EDITOR
        AssetDatabase.ImportAsset(RelativePath + "/" + fileName);
#endif

        Destroy(screenShot);
    }
}