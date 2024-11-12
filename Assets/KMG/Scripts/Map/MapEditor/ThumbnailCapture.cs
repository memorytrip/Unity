using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Windows;

namespace Map.Editor
{
    public class ThumbnailCapture: MonoBehaviour
    {
        // [SerializeField] private Camera cam;
        [SerializeField] private RenderTexture renderTexture;
        
        public async UniTask<string> CaptureToBase64()
        {
            var data = (await Capture()).EncodeToPNG();
            string b64 = Convert.ToBase64String(data);
            return b64;
        }
        
        public async UniTaskVoid CaptureToLocal()
        {
            var data = (await Capture()).EncodeToPNG();
            string filePath = Application.persistentDataPath + "/Maps/asdf.png";
            File.WriteAllBytes(filePath, data);
        }

        public async UniTask<Texture2D> Capture()
        {
            await UniTask.Yield();

            Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false, true);
            RenderTexture.active = renderTexture;
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

            await UniTask.Yield();

            return tex;
        }
    }
}