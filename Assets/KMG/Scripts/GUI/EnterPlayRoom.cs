using System;
using System.Collections;
using System.Linq;
using Common;
using Cysharp.Threading.Tasks;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GUI
{
    public class EnterPlayRoom: MonoBehaviour
    {
        [Header("Select create or join")]
        [SerializeField] private CanvasGroup selectCreateOrJoinPanel;
        [SerializeField] private Button createRoomButton;
        [SerializeField] private Button inputRoomButton;

        [Header("Input room Name")] 
        [SerializeField] private CanvasGroup inputRoomPanel;
        [SerializeField] private TMP_InputField roomNameField;
        [SerializeField] private Button joinRoomButton;

        [Header("Fade Controller")] 
        [SerializeField] private FadeController fader;

        private static readonly char[] RoomNameCharacter = new[]
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        private void Awake()
        {
            InitPanel();

            createRoomButton.onClick.AddListener(CreateRoom);
            inputRoomButton.onClick.AddListener(SwitchPanel);
            joinRoomButton.onClick.AddListener(JoinRoom);
        }

        private void InitPanel()
        {
            selectCreateOrJoinPanel.interactable = selectCreateOrJoinPanel.blocksRaycasts = true;
            selectCreateOrJoinPanel.alpha = 1;
            inputRoomPanel.interactable = inputRoomPanel.blocksRaycasts = false;
            inputRoomPanel.alpha = 0;
        }

        private void CreateRoom()
        {
            SceneManager.Instance.MoveRoom(RandRoomName()).Forget();
        }

        private void SwitchPanel()
        {
            selectCreateOrJoinPanel.interactable = selectCreateOrJoinPanel.blocksRaycasts = false;
            selectCreateOrJoinPanel.alpha = 0;
            inputRoomPanel.interactable = inputRoomPanel.blocksRaycasts = true;
            inputRoomPanel.alpha = 1;
        }

        public void ClosePanel()
        {
            selectCreateOrJoinPanel.interactable = selectCreateOrJoinPanel.blocksRaycasts = false;
            selectCreateOrJoinPanel.alpha = 0;
            inputRoomPanel.interactable = inputRoomPanel.blocksRaycasts = false;
            inputRoomPanel.alpha = 0;
        }

        private void JoinRoom()
        {
            string roomName = roomNameField.text;
            if (!IsRoomNameValid(roomName))
                throw new ArgumentException("Invalid RoomName");
            SceneManager.Instance.MoveRoom(roomName).Forget();
        }

        private string RandRoomName()
        {
            char[] str = new char[4];
            for (int i = 0; i < 4; i++)
                str[i] = RoomNameCharacter[Random.Range(0, RoomNameCharacter.Length)];
            return new string(str);
        }

        private bool IsRoomNameValid(string roomName)
        {
            if (roomName.Length != 4) return false;
            for (int i = 0; i < 4; i++)
                if (!RoomNameCharacter.Contains(roomName[i]))
                    return false;
            return true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            if (!other.GetComponent<NetworkObject>().HasStateAuthority)
                return;
            fader.StartFadeIn(0.5f);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            if (!other.GetComponent<NetworkObject>().HasStateAuthority)
                return;
            StartCoroutine(FadeOutProcess());
        }

        private IEnumerator FadeOutProcess()
        {
            yield return fader.FadeOut(0.5f);
            InitPanel();
        }
    }
}