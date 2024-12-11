using System.Collections;
using UnityEngine;
using System.IO;
using Cysharp.Threading.Tasks;
using Fusion;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class UploadPhoto : MonoBehaviour
{
    public RawImage img;
    public RawImage letterImg;
    public int photoId; //임시 : 원래는 response의 photoId
    private float photoCapacity = 5000000000000f;
    //private string File = "Application.temporaryCachePath";
    
    public void OnClickImageLoad()
    {
        NativeGallery.GetImageFromGallery((file) => //갤러리를 열었음
        {
            FileInfo selected = new FileInfo(file); //selected -> 선택한 이미지
            Debug.Log("파일 위치" + file);
            //용량 제한
            if (selected.Length > photoCapacity)   //byte
            {
                Debug.Log("용량 제한");
                return;
            }
            
            //불러 오기
            if (!string.IsNullOrEmpty(file))
            {
                LoadImage(file).Forget(); 
            }
        });
    }

    public async UniTask LoadImage(string path)
    {
        await UniTask.Yield();

        byte[] fileData = await File.ReadAllBytesAsync(path);
        string filename = Path.GetFileName(path).Split('.')[0];
        string savePath = Application.persistentDataPath + "/Image";
        Debug.Log("경로:" + savePath);

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        
        await File.WriteAllBytesAsync(savePath + filename + ".png", fileData);
        var temp = File.ReadAllBytesAsync(savePath + filename + ".png");

        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(await temp);

        img.texture = tex;
        letterImg.texture = tex;
        img.SetNativeSize();
        ImageSizeSetting(img, 432f, 288f);
        ImageSizeSetting(letterImg, 914f, 604f);
        StretchImageToFit(letterImg);
        StretchImageToFit(img);

        if (photoId == 0)
        {
            await PostLetterAndPhoto.Instance.PostPhotoProcess(tex);
        }
        else
        {
            await ModifyLetterAndPhoto.Instance.ModifyPhotoProcess(tex, photoId);
        }

        photoId = PhotoResponse.postResponse.photoId;
    }

    public static void ImageSizeSetting(RawImage img, float x, float y)
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

        img.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        img.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
    }
    
    private void StretchImageToFit(RawImage image)
    {
        // Set anchors to stretch in both directions to fit the parent
        img.rectTransform.anchorMin = Vector2.zero;
        img.rectTransform.anchorMax = Vector2.one;
        img.rectTransform.offsetMin = Vector2.zero;
        img.rectTransform.offsetMax = Vector2.zero;

        // Maintain aspect ratio by setting the scale mode
        img.rectTransform.localScale = Vector3.one;
        img.uvRect = new Rect(0, 0, 1, 1);
    }
    
}