using System;
using System.Collections.Generic;
using Common;
using Cysharp.Threading.Tasks;
using Map;
using UnityEngine;
using UnityEngine.UI;

public class Dummy_MapSelectTest : MonoBehaviour
{
    private List<MapInfo> mapInfoList;
    private List<RawImage> mapThumbnails;

    [SerializeField] private RectTransform mapListUI;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async UniTaskVoid Start()
    {
        mapInfoList = await MapManager.Instance.LoadMapList(new User());
        mapThumbnails = new List<RawImage>();
        foreach (var mapInfo in mapInfoList)
        {
            mapThumbnails.Add(GetThumbnail(mapInfo));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private RawImage GetThumbnail(MapInfo mapInfo)
    {
        GameObject imageObj = new GameObject("MapThumbnail");
        imageObj.transform.parent = mapListUI;
        imageObj.transform.localScale = Vector3.one;
        
        byte[] thumbnailData = Convert.FromBase64String(mapInfo.thumbnail);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(thumbnailData);
        
        RawImage image = imageObj.AddComponent<RawImage>();
        image.texture = texture;
        return image;
    }
}
