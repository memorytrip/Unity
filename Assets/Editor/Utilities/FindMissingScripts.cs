#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

// 씬 안의 Missing Component!를 찾아주는 스크립트
// Tools > Missing Scripts > Find
public class FindMissingScripts
{
    private const string MissingScriptsMenuFolder = "Tools/Missing Scripts/";

    [MenuItem(MissingScriptsMenuFolder + "Find")]
    static void FindMissingScriptsMenuItem()
    {
        foreach (var gameObj in GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            if (gameObj.GetComponentsInChildren<Component>().Any(component => component == null))
            {
                Debug.LogError($"GameObject contains missing script: {gameObj.name}", gameObj);
            }
            else
            {
                Debug.Log($"No missing scripts found!");
            }
        }
    }
}
#endif