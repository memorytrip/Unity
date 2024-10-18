using UnityEngine;

public class PhotoHide : MonoBehaviour
{
    private Vector2 randomPosition;
    private Vector2[] photoPosition;//사진이랑 위치를 dictionary로 만들까?
    
    private Vector2 GetRandomPosition()
    {
        return randomPosition = new Vector2(Random.Range(-10, 11), Random.Range(-10, 11));
    }

    public void HidePhoto(string[] photoArray)
    {
        for (int i = 0; i < photoArray.Length; i++)
        {
            photoPosition[i] = GetRandomPosition();
        }
    }

    public void FindPhoto()
    {
    }
}
