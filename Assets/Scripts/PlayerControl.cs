using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour
{
	public float runSpeed;
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

		_anim.SetBool("On Ground", isGrounded);

		if (Input.GetKey(KeyCode.RightArrow))
		{
			if (transform.localScale.x > 0f)
				transform.localScale = new Vector3(-1, 1, 1);

			_anim.SetBool("Running", true);

			_rigid.AddForce(new Vector2(runSpeed, 0));
		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			if (transform.localScale.x < 0f)
				transform.localScale = new Vector3(1, 1, 1);
		
			_anim.SetBool("Running", true);
		
			_rigid.AddForce(new Vector2(-runSpeed, 0));
		}
		else
		{
			_anim.SetBool("Running", false);
		}

		_rigid.velocity = _rigid.velocity.x > 10 ? new Vector2(10, _rigid.velocity.y) : _rigid.velocity;
		_rigid.velocity = _rigid.velocity.x < -10 ? new Vector2(-10, _rigid.velocity.y) : _rigid.velocity;

		if (isGrounded && Input.GetKeyDown(KeyCode.UpArrow))
		{
			_rigid.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);

			_anim.SetTrigger("Jump");
		}
	}
}
