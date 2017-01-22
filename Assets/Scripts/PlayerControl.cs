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
            if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Crew Dead"))
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                    Debug.Log("Can't Move - Player is Dead");
                return;
            }
            //Ground anim
            _anim.SetBool("On Ground", isGrounded);

            float direction = 0;

            //Movement
            if (Input.GetKey(KeyCode.RightArrow))
            {
                //Right facing
                if (transform.localScale.x > 0f)
                    transform.localScale = new Vector3(-1, 1, 1);

                //Moving
                _anim.SetBool("Running", true);

                //Dir
                direction = 1;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                //Left facing
                if (transform.localScale.x < 0f)
                    transform.localScale = new Vector3(1, 1, 1);

                //Moving
                _anim.SetBool("Running", true);

                //Dir
                direction = -1;
            }
            else
            {
                //Not moving
                _anim.SetBool("Running", false);
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

                _anim.SetTrigger("Jump");
            }
        }
        //Everybody else now
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Water")
		{
			_rigid.gravityScale = 0.1f;
			if (Mathf.Abs(_rigid.velocity.y) > 1)
				_rigid.velocity /= 10;

			_anim.SetBool("In Water", true);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Water")
		{
			_rigid.gravityScale = 2f;

			_anim.SetBool("In Water", false);
		}
	}


}
