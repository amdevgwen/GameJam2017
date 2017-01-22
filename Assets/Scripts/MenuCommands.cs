using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuCommands : MonoBehaviour
{
	NetworkManager _netMan;
	public InputField hostAddressField;
	public GameObject runningMan;

	public void Quit()
	{
		Application.Quit();
	}

	public void SetIPAndJoin()
	{
		_netMan.networkPort = 7777;
		if (hostAddressField != null && hostAddressField.text != null && hostAddressField.text.Trim() != "")
		{
			_netMan.networkAddress = hostAddressField.text.Trim();
		}

		_netMan.StartClient();

		Text ipText = GameObject.Find("Host IP").GetComponent<Text>();
		ipText.text = hostAddressField.text.Trim();
		ipText.text = ipText.text == "" ? "Local" : ipText.text;

		AttachRunner(hostAddressField.transform);
	}

	public void Host()
	{
		_netMan.networkPort = 7777;

		Text ipText = GameObject.Find("Host IP").GetComponent<Text>();
		ipText.text = Network.player.ipAddress + " (Host)";

		_netMan.StartHost();		//Is hosting and live
	}

	public void AttachRunner(Transform adjacentButton)
	{
		runningMan.transform.position = adjacentButton.position + new Vector3(3.5f, 0, 0);
	}

	void Start()
	{
		_netMan = FindObjectOfType<NetworkManager>();

		_netMan.maxConnections = 16;
	}
}
