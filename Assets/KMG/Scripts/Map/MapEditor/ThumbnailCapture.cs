using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Map.Editor
{
    public class ThumbnailCapture: MonoBehaviour
    {
        // [SerializeField] private Camera cam;
        [SerializeField] private RenderTexture renderTexture;

        public void Test()
        {
            CaptureToBase64().Forget();
        }
        
        public async UniTask<string> CaptureToBase64()
        {
            await UniTask.Yield();

            Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false, true);
            RenderTexture.active = renderTexture;
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

            await UniTask.Yield();

            var data = tex.EncodeToPNG();
            string b64 = Convert.ToBase64String(data);
            return b64;
        }
    }
}