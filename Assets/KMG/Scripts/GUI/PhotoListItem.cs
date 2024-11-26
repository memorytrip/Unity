using System;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class PhotoListItem: MonoBehaviour
    {
        [SerializeField] protected Image image;
        [SerializeField] protected Button button;

        [HideInInspector] public CanvasGroup imagePanel;
        [HideInInspector] public RawImage rawImage;

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
        }
    }
}