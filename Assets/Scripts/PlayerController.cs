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
    //В какую сторону повернут персонаж: false - влево, true - вправо
    private bool _rightTurning;

    public Rigidbody2D rb2d;
    public Animator animator;
    [Range(0, 100f)] public float speed = 60f;
    //Коэффициент сглаживания передвижения персонажа, без него как то не живо выглядит
    [Range(0, 0.3f)] public float smoothingKoef = .03f;
    [Range(50f, 400f)] public float jumpForce = 200f;
    [Range(50f, 400f)] public float doubleJumpForce = 100f;
    [Range(0, 3f)] public float shiftMultiplier = 2f;
    //Можно ли управлять персонажем в воздухе
    public bool canAirControl;

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
            animator.SetBool("isShifting", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift) || (_horizontalMove == 0f))
            animator.SetBool("isShifting", false);
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics2D.OverlapCircle(groundChecker.position, _groundCheckerRadius, groundLayers);

        if(_isGrounded || (!_isGrounded && canAirControl))
            MovePlayer(_horizontalMove * Time.fixedDeltaTime);
        if(_isGrounded)
        {
            JumpPlayer(_jump);
            _jump = false;
        }
    }

    void MovePlayer(float moveValue)
    {
        if (moveValue < 0 && _rightTurning)
            ChangeTurning();
        else if (moveValue > 0 && !_rightTurning)
            ChangeTurning();
        animator.SetFloat("Speed", Mathf.Abs(_horizontalMove));
        Vector2 targetVelocity = new Vector2(moveValue, rb2d.velocity.y);
        //Функция, которая сглаживает перемещение персонажа
        rb2d.velocity = Vector2.SmoothDamp(rb2d.velocity, targetVelocity, ref _zeroVelocity, smoothingKoef);
        //сохраняем инерцию полета, убираем как только приземлились
        if (_isGrounded)
        {
            _horizontalMove = 0f;
        }
    }

    void JumpPlayer(bool jump)
    {
        if(jump)
        {
            _isGrounded = false;
            rb2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Force);
        }
    }

    void ChangeTurning()
    {
        _rightTurning = !_rightTurning;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
