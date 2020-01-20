using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _horizontalMove = 0f;
    private Vector2 _zeroVelocity = Vector2.zero;
    private bool _jump = false;
    private float _groundCheckerRadius = 0.1f;
    private bool _isGrounded;

    public Rigidbody2D rb2d;
    public float speed = 60f;
    public float smoothingKoef = .03f;
    public float jumpForce = 200f;

    public LayerMask groundLayers;
    public Transform groundChecker;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _horizontalMove = -speed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _horizontalMove = speed;
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _jump = true;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer(_horizontalMove * Time.fixedDeltaTime);
        JumpPlayer(_jump);

        _horizontalMove = 0f;
        _jump = false;
    }

    void MovePlayer(float moveValue)
    {
        Vector2 targetVelocity = new Vector2(moveValue, rb2d.velocity.y);
        rb2d.velocity = Vector2.SmoothDamp(rb2d.velocity, targetVelocity, ref _zeroVelocity, smoothingKoef);
    }

    void JumpPlayer(bool jump)
    {
        _isGrounded = (Physics2D.OverlapCircle(groundChecker.position, _groundCheckerRadius, groundLayers) != null) ? true : false;
        if (jump && _isGrounded)
            rb2d.AddForce(new Vector2(0f, jumpForce));
    }
}
