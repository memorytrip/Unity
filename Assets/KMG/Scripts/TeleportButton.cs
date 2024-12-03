using Common.Network;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class TeleportButton : MonoBehaviour
{
	[SerializeField] private Transform destination;
    private Button button;

	private void Start() {
		button = GetComponent<Button>();
		button.onClick.AddListener(Teleport);
	}

	private void Teleport() {
		//NetworkTransform networkTransform = Connection.StateAuthInstance.currenctCharacter.GetComponent<NetworkTransform>();
		//networkTransform.Teleport(destination.position);

		PlayerMovement player = Connection.StateAuthInstance.currenctCharacter.GetComponent<PlayerMovement>();
		player.RequestTeleport(destination.position);
	}
}
