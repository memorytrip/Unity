using UnityEngine;

public static class ParameterNameCache
{
    public const string HasPaused = "HasPaused";
    public const string IsNearYggdrasil = "IsNearYggdrasil";
    public const string LetterFoundCount = "LetterFoundCount";
    public const string PlayStarted = "PlayStarted";
    public const string ResultReady = "ResultReady";
    public const string IsWalking = "isWalking";

    public static readonly WaitForSeconds Wait = new(3f);
    public static readonly WaitForSeconds WaitLonger = new(8f); // 임시
}
