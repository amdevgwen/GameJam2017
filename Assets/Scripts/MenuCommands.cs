using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuCommands : MonoBehaviour
{
	public InputField hostAddressField;
	public GameObject runningMan;

	public void Quit()
	{
		Application.Quit();
	}

	public void SetIPAndJoin()
	{
		NetworkManager.singleton.networkPort = 7777;
		if (hostAddressField != null && hostAddressField.text != null && hostAddressField.text.Trim() != "")
		{
			NetworkManager.singleton.networkAddress = hostAddressField.text.Trim();
		}

		NetworkManager.singleton.StartClient();

		Text ipText = GameObject.Find("Host IP").GetComponent<Text>();
		ipText.text = hostAddressField.text.Trim();
		ipText.text = ipText.text == "" ? "Local" : ipText.text;

		AttachRunner(hostAddressField.transform);
	}

	public void Host()
	{
		if (NetworkManager.singleton.IsClientConnected())
			NetworkManager.singleton.StopClient();

		NetworkManager.singleton.networkPort = 7777;

		Text ipText = GameObject.Find("Host IP").GetComponent<Text>();
		ipText.text = Network.player.ipAddress + " (Host)";

		NetworkManager.singleton.StartHost();		//Is hosting and live
	}

	public void AttachRunner(Transform adjacentButton)
	{
		runningMan.transform.position = adjacentButton.position + new Vector3(3.5f, 0, 0);
	}

	void Start()
	{
		NetworkManager.singleton = FindObjectOfType<NetworkManager>();

		NetworkManager.singleton.maxConnections = 16;
	}
}
