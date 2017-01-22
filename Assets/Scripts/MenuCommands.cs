using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuCommands : MonoBehaviour
{
	public InputField hostAddressField;
	public GameObject runningMan;
	Text _ipText;


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

		_ipText.text = hostAddressField.text.Trim();
		_ipText.text = _ipText.text == "" ? "Local" : _ipText.text;

		AttachRunner(hostAddressField.transform);
	}

	public void Host()
	{
		if (NetworkManager.singleton.IsClientConnected())
			NetworkManager.singleton.StopClient();

		NetworkManager.singleton.networkPort = 7777;

		_ipText.text = Network.player.ipAddress + " (Host)";

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

		_ipText = GameObject.Find("Host IP").GetComponent<Text>();

		if (_ipText != null && _ipText.text.Trim() != "" && !_ipText.text.Contains("Local") && !_ipText.text.Contains("Host"))
		{
			hostAddressField.text = _ipText.text;
		}
			
	}
}
