using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{

	public override void OnClientDisconnect(NetworkConnection conn)
	{
		base.OnClientDisconnect(conn);
		print("Client Disconnect");

		var ipField = GameObject.Find("Host Address Field");
		if (ipField != null)
		{
			ipField.GetComponent<UnityEngine.UI.InputField>().text = conn.address;
			print("IP saved");
		}
	}

}
