using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _horizontalMove = 0f;
    private Vector2 _zeroVelocity = Vector2.zero;
    private bool _jump;
    private float _groundCheckerRadius = 0.1f;
    private bool _isGrounded;
    private bool _doubleJump;

    public Rigidbody2D rb2d;
    [Range(0, 100f)] public float speed = 60f;
    //Коэффициент сглаживания передвижения персонажа, без него как то не живо выглядит
    [Range(0, 0.3f)] public float smoothingKoef = .03f;
    [Range(50f, 400f)] public float jumpForce = 200f;
    [Range(50f, 400f)] public float doubleJumpForce = 100f;
    [Range(0, 3f)] public float shiftMultiplier = 2f;

    //Слои, определяющие поверхность земли
    public LayerMask groundLayers;
    //Пустой объект, координатами и _groundCheckerRadius определяем стоит ли персонаж на земле или нет
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
        if ( (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && _isGrounded)
        {
            _horizontalMove *= shiftMultiplier;
        }
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics2D.OverlapCircle(groundChecker.position, _groundCheckerRadius, groundLayers);

        MovePlayer(_horizontalMove * Time.fixedDeltaTime);
        JumpPlayer(_jump);

        _horizontalMove = 0f;
        _jump = false;
    }

    void MovePlayer(float moveValue)
    {
        Vector2 targetVelocity = new Vector2(moveValue, rb2d.velocity.y);
        //Функция, которая сглаживает перемещение персонажа
        rb2d.velocity = Vector2.SmoothDamp(rb2d.velocity, targetVelocity, ref _zeroVelocity, smoothingKoef);
    }

    void JumpPlayer(bool jump)
    {
        if (jump)
        {
            if(_isGrounded)
            {
                rb2d.AddForce(new Vector2(0f, jumpForce));
                _doubleJump = true;
            }
            else if (_doubleJump)
            {
                rb2d.velocity = Vector2.zero;
                rb2d.AddForce(new Vector2(0f, doubleJumpForce));
                _doubleJump = false;
            }
        }
    }
}
