using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Death : NetworkBehaviour
{
	public float timeDrowning = 0.0f;

	[SyncVar]
	public bool isDead;
	[SyncVar]
	public bool isRespawn;

	public float timeDead = 0.0f;

	private const float timeToDrown = 4.0f;
	private const float timeToRespawn = 5.0f;

	private const float distanceFromTarget = 5.0f;
	private const float launchForce = 40f;
	private const float launchangle = Mathf.PI * 1 / 4;
	//based from <1,0>

	private Animator _anim;
	private Rigidbody2D _body;
	private PlayerAudio _playerAudio;

	public GameObject Target;

	private void Awake()
	{
		_anim = GetComponent<Animator>();
		_body = GetComponent<Rigidbody2D>();
		_playerAudio = GetComponent<PlayerAudio>();
		//find something!
		if (Target == null)
		{
			Target = GameObject.FindGameObjectWithTag("Target");
		}
	}

	private void Start()
	{
		isDead = _anim.GetCurrentAnimatorStateInfo(0).IsName("Crew Dead");
		isRespawn = false;
	}

	private void Update()
	{
		//Server checks if the person is drowning
		//if they drown enough start counting if they are dead
		if (isServer)
		{
			//counting should be server ONLY
			if (isDead)
			{
				//ayyy do nothing for now!
				//Code below is for death timeout

				//timeDead += Time.deltaTime;
				//if (timeDead > timeToRespawn)
				//{
				//    timeDead = 0.0f;
				//    isDead = false;
				//    RpcRespawn();
				//    //res the idiot
				//}
			}
			else
			{
				//clearly not dead
				timeDead = 0.0f;
				//man overboard!
				if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Crew Drowning"))
				{
					timeDrowning += Time.deltaTime;
					if (timeDrowning > timeToDrown)
					{
						timeDrowning = 0.0f;
						isDead = true;
					}
				}
				else
				{
					timeDrowning = 0.0f;
				}
			}
		}

		//Check if user is attempt to be revived
		if (isLocalPlayer && isDead && Input.GetKeyDown(KeyCode.R))
		{
			Respawn();
		}

		RunState();
	}

	[Command]
	private void CmdSyncState(bool dead, bool respawn)
	{
		isDead = dead;
		isRespawn = respawn;
	}

	private void RunState()
	{
		_anim.SetBool("Dead", isDead);
		_anim.SetBool("Respawn", isRespawn);
	}

	private void Respawn()
	{
		if (isLocalPlayer)
		{
			if (Target == null)
			{
				transform.position = new Vector3(transform.position.x, 3, 0);
			}
			else
			{
				Vector3 targetPos = Target.transform.position;
				Vector3 spawnPos = new Vector3(Mathf.Cos(launchangle), Mathf.Sin(launchangle), 0) * distanceFromTarget
				                               + targetPos;
				spawnPos.z = -10;
				transform.position = spawnPos;

				float invertAngle = launchangle + Mathf.PI;

				Vector3 force = new Vector3(Mathf.Cos(invertAngle), Mathf.Sin(invertAngle)) * launchForce;
				_body.AddForce(force, ForceMode2D.Impulse);
			}

			isDead = false;
			isRespawn = true;
			_playerAudio.PlaySpawnSound();

			CmdSyncState(isDead, isRespawn);
		}
	}
}
