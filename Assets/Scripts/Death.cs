using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Death : NetworkBehaviour
{
    public float timeDrowning = 0.0f;

    [SyncVar]
    public bool isDead;

    public float timeDead = 0.0f;

    private const float timeToDrown = 4.0f;
    private const float timeToRespawn = 5.0f;

    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    private void Start()
    {
        isDead = _anim.GetCurrentAnimatorStateInfo(0).IsName("Crew Dead");
    }

    private void Update()
    {
        if (isServer)
        {
            //counting should be server ONLY
            if (isDead)
            {
                timeDead += Time.deltaTime;
                if (timeDead > timeToRespawn)
                {
                    timeDead = 0.0f;
                    isDead = false;
                    RpcRespawn();
                    //res the idiot
                }
            }
            else
            {
                timeDead = 0.0f;
                //man overboard!
                if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Crew Drowning"))
                {
                    timeDrowning += Time.deltaTime;
                    if(timeDrowning > timeToDrown)
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

        //Server Code ended now for everybody else
        if (isDead)
        {
            _anim.SetBool("Dead", true);
        }
    }

    //something for something?
    [Command]
    private void CmdRespawn()
    {        

    }

    [ClientRpc]
    private void RpcDrown()
    {
        if (isLocalPlayer)
        {
            _anim.SetBool("Dead", true);
            //this ensures that the player is dead
        }
    }

    [ClientRpc]
    private void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            transform.position = new Vector3(transform.position.x, 3, 0);
        }
        _anim.SetBool("Respawn", true);
        _anim.SetBool("Dead", false);
    }
}
