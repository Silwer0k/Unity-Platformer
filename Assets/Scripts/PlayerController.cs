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
        //Функция, которая сглаживает перемещение персонажа
        rb2d.velocity = Vector2.SmoothDamp(rb2d.velocity, targetVelocity, ref _zeroVelocity, smoothingKoef);
    }

    void JumpPlayer(bool jump)
    {
        //Илюша, это тернарный оператор. Как if только в одну строчку. Слева от ? условие проверки стоит ли персонаж на земле или нет, 
        //справа от ? - что получим если условие будет true, после : - что получим, если false
        //В итоге в _isGrounded будет либо true, либо false 
        _isGrounded = (Physics2D.OverlapCircle(groundChecker.position, _groundCheckerRadius, groundLayers) != null) ? true : false;
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
