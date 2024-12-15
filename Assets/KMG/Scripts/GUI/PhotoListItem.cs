using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class PhotoListItem: MonoBehaviour
    {
        [SerializeField] protected Image image;
        [SerializeField] protected Button button;
        
        [HideInInspector] public Button closeButton;
        [HideInInspector] public CanvasGroup imagePanel;
        [HideInInspector] public RawImage rawImage;

        [HideInInspector] public InMyRoomPhotoList photoList;

        private void Awake()
        {
            button.onClick.AddListener(ShowImage);
        }

        public void SetThumbnail(Sprite sprite)
        {
            image.sprite = sprite;
        }

        protected virtual void ShowImage()
        {
            Utility.EnablePanel(imagePanel);
            rawImage.texture = image.sprite.texture;
            
            photoList.downloadButton.onClick.RemoveAllListeners();
            photoList.downloadButton.onClick.AddListener(Download);
        }

        protected virtual void Download()
        {
            // string path = Application.persistentDataPath + "/media";
            // if (!Directory.Exists(path))
            // {
            //     Directory.CreateDirectory(path);
            // }
            
            
            byte[] texturePNGBytes = image.sprite.texture.EncodeToPNG();
            string filename = Guid.NewGuid().ToString() + ".png";
            
            Debug.Log($"Download Picture: {filename}");
            // File.WriteAllBytes($"{path}/{filename}", texturePNGBytes);
            NativeGallery.SaveImageToGallery(texturePNGBytes, "memorytrip", filename);
            PopupManager.Instance.ShowMessage("저장되었습니다.");
        }
    }
}