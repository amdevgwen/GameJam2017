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

			_netMan.StartClient();

			Text ipText = GameObject.Find("Host IP").GetComponent<Text>();
			ipText.text = hostAddressField.text.Trim();

			AttachRunner(hostAddressField.transform);
		}
	}

	public void JoinLocal()
	{
		print("Listening as client? : " + _netDisc.StartAsClient().ToString());
	}

	public void Host()
	{
		if (_netDisc.isClient)
		{
			_netDisc.StopBroadcast();
			print(_netDisc.isClient + " " + _netDisc.isServer);
		}

		_netMan.networkPort = 7777;

		Text ipText = GameObject.Find("Host IP").GetComponent<Text>();
		ipText.text = Network.player.ipAddress + " (Host)";

		_netDisc.StartAsServer();
//		_netMan.StartHost();
	}

	public void AttachRunner(Transform adjacentButton)
	{
		runningMan.transform.position = adjacentButton.position + new Vector3(3.5f, 0, 0);
	}

	void Start()
	{
		_netMan = FindObjectOfType<NetworkManager>();
		_netDisc = FindObjectOfType<NetworkDiscovery>();

		print("Local discovery initialized? : " + _netDisc.Initialize().ToString());

		_netMan.maxConnections = 16;

		#if UNITY_EDITOR
		_netDisc.runInEditMode = true;
		#endif
	}
}
