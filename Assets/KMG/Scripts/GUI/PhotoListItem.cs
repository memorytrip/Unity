using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class PhotoListItem: MonoBehaviour
    {
        [SerializeField] private Image image;

        public void SetThumbnail(Sprite sprite)
        {
            image.sprite = sprite;
        }
    }
}