using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
	public override void OnStopClient()
	{
		base.OnStopClient();

		print("Stopped!");

		var ipField = GameObject.Find("Host Address Field");
		if (ipField != null)
		{
			ipField.GetComponent<UnityEngine.UI.InputField>().text = networkAddress;

			print(networkAddress);
		}
	}
}
