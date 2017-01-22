using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour
{
    public float runPower;
    public float maxRunSpeed;
    public float jumpPower;

    private Rigidbody2D _rigid;
    private Animator _anim;

    [SyncVar]
    public bool isGrounded;
    [SyncVar]
    public bool isRunning;
    [SyncVar]
    public bool isJumping;
    [SyncVar]
    public bool inWater;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f);
    }

    public override void OnStartClient()
    {
        Debug.Log("Calling On Start Client");
        base.OnStartClient();
    }

    // the Update loop contains a very simple example of moving the character around and controlling the animation
    void Update()
    {
        //Take Player movement
        if (isLocalPlayer)
        {
            //prevent action
            //if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Crew Dead"))
            //{
            //    if (Input.GetKeyDown(KeyCode.RightArrow))
            //        Debug.Log("Can't Move - Player is Dead");
            //    return;
            //}
            //Ground anim
            //_anim.SetBool("On Ground", isGrounded);

            float direction = 0;

            //Movement
            if (Input.GetKey(KeyCode.RightArrow))
            {
                //Right facing
                if (transform.localScale.x > 0f)
                    transform.localScale = new Vector3(-1, 1, 1);

                //Moving
                isRunning = true;//_anim.SetBool("Running", true);

                //Dir
                direction = 1;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                //Left facing
                if (transform.localScale.x < 0f)
                    transform.localScale = new Vector3(1, 1, 1);

                //Moving
                isRunning = true;// _anim.SetBool("Running", true);

                //Dir
                direction = -1;
            }
            else
            {
                //Not moving
                isRunning = false; // _anim.SetBool("Running", false);
            }

            //Dampen air movement
            if (!isGrounded)
                direction /= 2;

            //Movement
            _rigid.AddForce(new Vector2(runPower * direction, 0));

            //Clamp horizontal velocity
            _rigid.velocity = _rigid.velocity.x > maxRunSpeed ? new Vector2(10, _rigid.velocity.y) : _rigid.velocity;
            _rigid.velocity = _rigid.velocity.x < -maxRunSpeed ? new Vector2(-10, _rigid.velocity.y) : _rigid.velocity;

            //Jumping
            if (isGrounded && Input.GetKeyDown(KeyCode.UpArrow))
            {
                _rigid.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);

                isJumping = true;// _anim.SetTrigger("Jump");
            }

            CmdSyncState(isGrounded, isRunning, isJumping, inWater);
        }
        //Everybody else now
        RunState(); //Run state regardless

    }

    [Command]
    private void CmdSyncState(bool grounded, bool running, bool jumping, bool water)
    {
        if (isServer)
        {
            isGrounded = grounded;
            isRunning = running;
            isJumping = jumping;
            inWater = water;
        }
    }


    private void RunState()
    {
        _anim.SetBool("On Ground", isGrounded);
        _anim.SetBool("Running", isRunning);
        _anim.SetBool("In Water", inWater);
        if (isJumping)
            _anim.SetTrigger("Jump");

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (isLocalPlayer)
        {
            if (other.tag == "Water")
            {
                _rigid.gravityScale = 0.1f;
                if (Mathf.Abs(_rigid.velocity.y) > 1)
                    _rigid.velocity /= 10;

                inWater = true;//_anim.SetBool("In Water", true);
            }
            CmdSyncState(isGrounded, isRunning, isJumping, inWater);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (isLocalPlayer)
        {
            if (other.tag == "Water")
            {
                _rigid.gravityScale = 2f;

                inWater = false;//_anim.SetBool("In Water", false);
            }
            CmdSyncState(isGrounded, isRunning, isJumping, inWater);
        }
    }


}
