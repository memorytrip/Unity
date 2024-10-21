using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using Fusion;
using TMPro;
using Unity.VisualScripting;

public class PhotoManager : NetworkBehaviour, IListener
{
    private float yValue = 1f;
    private Dictionary<int, Vector3> photoPositions;
    public GameObject photoPrefab;
    private GameObject hitObject;
    public TMP_Text process;
    public TMP_Text letter;
    private int findedPhoto = 0;

    void Start()
    {
        EventManager.Instance.AddListener(EventType.eRaycasting, this);
        photoPositions = new Dictionary<int, Vector3>();
        HidePhoto(photoPositions, 8);
    }
    
    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-10, 11), yValue, Random.Range(-10, 11));
    }

    public void HidePhoto(Dictionary<int, Vector3> photoDict, int numberOfPhotos)
    {
        for (int i = 1; i <= numberOfPhotos; i++)
        {
            int photoName = i;
            photoDict[photoName] = GetRandomPosition(); 
            GameObject photoObject = Instantiate(photoPrefab, photoDict[photoName], Quaternion.identity);
            PhotoData photodata = photoObject.GetComponent<PhotoData>();
            if (photodata != null)
            {
                photodata.SetPhotoName(photoName);
            }
        }
    }


    public void OnEvent(EventType eventType, Component sender, object param = null)
    {
        switch (eventType)
        {
            case EventType.eRaycasting:
                if (param is PlayerInteraction.RaycastInfo raycastInfo)
                {
                    if (raycastInfo.isHit)
                    {
                        hitObject = raycastInfo.hitInfo.collider.gameObject;
                        
                    }
                }
                
                break;
                
        }
        
    }
    
    private void PhotoTransparency(GameObject obj , float alpha)
    {
        if (HasStateAuthority)
        {
            
        }
    }
    
    public void DestroyPhoto()
    {
        UpdateProcess();
        if (hitObject != null)
        {
            // hitObject에서 PhotoData를 다시 가져오기
            PhotoData photodata = hitObject.GetComponent<PhotoData>();

            if (photodata != null)
            {
                if (photodata.GetPhotoName() % 2 == 0)
                {
                    StartCoroutine(FadeIO());
                }
                Debug.Log("삭제될 사진: " + photodata.GetPhotoName());
            }
            else
            {
                Debug.Log("PhotoData를 찾지 못했습니다.");
            }

            // 오브젝트 삭제
            Destroy(hitObject);
        }
        else
        {
            Debug.Log("삭제할 오브젝트가 없습니다.");
        }
    }

    private IEnumerator FadeIO()
    {
        letter.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        letter.gameObject.SetActive(false);
    }

    private void UpdateProcess()
    {
        findedPhoto++;
        process.text = findedPhoto + " / 8";
    }
}

