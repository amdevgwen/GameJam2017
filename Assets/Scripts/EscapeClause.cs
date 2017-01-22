using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EscapeClause : MonoBehaviour
{

	void Update()
	{
		if (Input.GetButtonDown("Cancel"))
		{
			NetworkManager.singleton.StopClient();

			if (!NetworkManager.singleton.isNetworkActive)
				NetworkManager.singleton.StopServer();
		}
	}
}
