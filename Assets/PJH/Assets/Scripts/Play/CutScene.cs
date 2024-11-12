using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CutScene : MonoBehaviour
{
    public List<Vector3> path = new List<Vector3>();
    
    private void Start()
    {
        StartCoroutine(CutSceneEffect(path));
    }
    private IEnumerator CutSceneEffect(List<Vector3> path)
    {
        transform.DOPath(path.ToArray(), 3f, PathType.CatmullRom);
        yield return new WaitForSeconds(3f);
        transform.gameObject.SetActive(false);
    }
}


