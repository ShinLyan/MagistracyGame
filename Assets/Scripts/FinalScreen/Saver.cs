using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;

public class FileSaver : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void DownloadFile(string filename, string fileData);

    public void SaveFile(byte[] data, string filename)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        string base64Data = System.Convert.ToBase64String(data);
        DownloadFile(filename, base64Data);
#elif UNITY_EDITOR
        string path = Path.Combine(Application.persistentDataPath, filename);
        File.WriteAllBytes(path, data);
#else
        Debug.LogWarning("WebGL only. Используйте другую реализацию для Standalone.");
#endif
    }
}
