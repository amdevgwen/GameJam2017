using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour
{
	public float runPower;
	public float maxRunSpeed;
	public float jumpPower;
	public bool isGrounded;

	private Rigidbody2D _rigid;
	private Animator _anim;

	void Awake()
	{
		_anim = GetComponent<Animator>();
		_rigid = GetComponent<Rigidbody2D>();
	}

	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update()
	{
		//do not update if not local
		if (!isLocalPlayer)
			return;

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
		_rigid.velocity = _rigid.velocity.x > maxRunSpeed ? new Vector2(maxRunSpeed, _rigid.velocity.y) : _rigid.velocity;
		_rigid.velocity = _rigid.velocity.x < -maxRunSpeed ? new Vector2(-maxRunSpeed, _rigid.velocity.y) : _rigid.velocity;

		//Jumping
		if (isGrounded && Input.GetKeyDown(KeyCode.UpArrow))
		{
			_rigid.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);

			_anim.SetTrigger("Jump");
		}
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
