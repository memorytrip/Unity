using UnityEngine;
using Unity.Cinemachine;

public class CutsceneCamera : MonoBehaviour
{
    private CinemachineSplineDolly _splineDolly;

    private void Awake()
    {
        _splineDolly = GetComponent<CinemachineSplineDolly>();
    }
}
