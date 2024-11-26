using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GUI
{
    public class PhotoListVideoItem: PhotoListItem
    {
        [SerializeField] private VideoPlayer videoPlayer;

        protected override void ShowImage()
        {
            Utility.EnablePanel(imagePanel);
            rawImage.texture = videoPlayer.texture;
        }
    }
}