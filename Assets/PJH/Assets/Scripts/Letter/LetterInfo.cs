using Common;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class RecievedLetterData
{
    public int letterId;
    public string writerEmail;
    public string content;
}

public class LetterInfo : MonoBehaviour
{
    public static RecievedLetterData letterResponse { get; private set; }

    public static void SetLetterResponse(RecievedLetterData response)
    {
        letterResponse = response;
    }

    public static void Clear()
    {
        letterResponse = null;
    }
}
