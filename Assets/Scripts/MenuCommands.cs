using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuCommands : MonoBehaviour
{
	NetworkManager _netMan;
	NetworkDiscovery _netDisc;
	public InputField hostAddressField;

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

			_netMan.StartClient();

			Text ipText = GameObject.Find("Host IP").GetComponent<Text>();
			ipText.text = hostAddressField.text.Trim();
		}
	}

	public void JoinLocal()
	{
		print("Listening as client? : " + _netDisc.StartAsClient().ToString());
	}

	public void Host()
	{
		_netMan.networkPort = 7777;

		Text ipText = GameObject.Find("Host IP").GetComponent<Text>();
		ipText.text = Network.player.ipAddress + " (Host)";

		_netMan.StartHost();
	}

	void Start()
	{
		_netMan = FindObjectOfType<NetworkManager>();
		_netDisc = FindObjectOfType<NetworkDiscovery>();

		print("Local discovery initialized? : " + _netDisc.Initialize().ToString());
	}
}
