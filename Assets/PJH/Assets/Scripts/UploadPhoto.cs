using System.Collections;
using UnityEngine;
using System.IO;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class UploadPhoto : MonoBehaviour
{
    public RawImage img;
    private float photoCapacity = 5000000000000f;

    public void OnClickImageLoad()
    {
        NativeGallery.GetImageFromGallery((file) => //갤러리를 열었음
        {
            FileInfo selected = new FileInfo(file); //selected -> 선택한 이미지
            
            //용량 제한
            if (selected.Length > photoCapacity)   //byte
            {
                Debug.Log("용량 제한");
                return;
            }
            
            //불러 오기
            if (!string.IsNullOrEmpty(file))
            {
                StartCoroutine(LoadImage(file)); 
            }
        });
    }

    IEnumerator LoadImage(string path)
    {
        yield return null;

        byte[] fileData = File.ReadAllBytes(path);
        string filename = Path.GetFileName(path).Split('.')[0];
        string savePath = Application.persistentDataPath + "/Image";
        Debug.Log("경로:" + savePath);

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        
        File.WriteAllBytes(savePath + filename + ".png", fileData);
        var temp = File.ReadAllBytes(savePath + filename + ".png");

        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(temp);

        img.texture = tex;
        img.SetNativeSize();
        ImageSizeSetting(img, 495f, 495f);
    }

    void ImageSizeSetting(RawImage img, float x, float y)
    {
        var imageX = img.rectTransform.sizeDelta.x;
        var imageY = img.rectTransform.sizeDelta.y;
        
        float targetAspect = x / y;   // 목표 비율

        if (targetAspect  > imageX / imageY) //세로가 더김 
        {
            float newHeight = imageX / targetAspect;  // 비율에 맞춰 새 세로 길이 계산
            float yOffset = (imageY - newHeight) / 2f; // 잘라낼 세로 영역 계산 (중앙 기준)

            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);

            // 텍스처 자르기
            img.uvRect = new Rect(0, yOffset / imageY, 1, newHeight / imageY);
        }
        else if (targetAspect  < imageX / imageY)
        {
            float newWidth = imageY * targetAspect;  // 비율에 맞춰 새 가로 길이 계산
            float xOffset = (imageX - newWidth) / 2f; // 잘라낼 가로 영역 계산 (중앙 기준)

            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);

            // 텍스처 자르기
            img.uvRect = new Rect(xOffset / imageX, 0, newWidth / imageX, 1); 
        }
        else
        {
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);
        }
    }
}
