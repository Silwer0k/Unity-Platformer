    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _horizontalMove = 0f;
    private Vector2 _zeroVelocity = Vector2.zero;
    private bool _jump;
    private float _groundCheckerRadius = 0.03f;
    private bool _isGrounded;
    private bool _doubleJump;
    //В какую сторону повернут персонаж: false - влево, true - вправо
    private bool _rightTurning=true;
    //Количество оставшихся допольнительных прыжков прыжков (изменяется по ходу выполнения кода)
    private int _jumpsLeft;
    private float _shiftSpeed;

    public Rigidbody2D rb2d;
    public Animator animator;
    [Range(0, 100f)] public float speed = 60f;
    //Коэффициент сглаживания передвижения персонажа, без него как то не живо выглядит
    [Range(0, 0.3f)] public float smoothingKoef = .03f;
    [Range(50f, 400f)] public float jumpForce = 200f;
    [Range(50f, 400f)] public float extraJumpForce = 100f;
    [Range(0, 3f)] public float shiftMultiplier = 2f;
    //Задаваемое количество прыжков. Задаем это число через Editor(не изменяется по ходу выполнения кода)
    public int jumpsCount;
    //Можно ли управлять персонажем в воздухе
    public bool canAirControl;

    //Слои, определяющие поверхность земли
    public LayerMask groundLayers;
    //Пустой объект, координатами и _groundCheckerRadius определяем стоит ли персонаж на земле или нет
    public Transform groundChecker;

    void Awake()
    {
        _jumpsLeft = jumpsCount;
    }

    // Update is called once per frame
    void Update()
    {
        bool leftIsHolding = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool rightIsHolding = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        if (leftIsHolding)
        {
            _horizontalMove = -speed;
            _shiftSpeed = _horizontalMove * shiftMultiplier;
        }
        else 
        if (rightIsHolding)
        {
            _horizontalMove = speed;
            _shiftSpeed = _horizontalMove * shiftMultiplier;
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _jump = true;
        }
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && (rightIsHolding || leftIsHolding))
        {
            _horizontalMove = _shiftSpeed;
            animator.SetBool("isShifting", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift) || (_horizontalMove == 0f))
            animator.SetBool("isShifting", false);
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics2D.OverlapCircle(groundChecker.position, _groundCheckerRadius, groundLayers);
        animator.SetBool("isGrounded", _isGrounded);
        animator.SetFloat("Speed", Mathf.Abs(_horizontalMove));

        if (_isGrounded || (!_isGrounded && canAirControl))
            MovePlayer(_horizontalMove * Time.fixedDeltaTime);
        
        JumpPlayer(_jump);
        _jump = false;
        if (_isGrounded)
        {
            _jumpsLeft = jumpsCount;
            //сохраняем инерцию полета, убираем как только приземлились
            _horizontalMove = 0f;
        }
    }

    void MovePlayer(float moveValue)
    {
        if (moveValue < 0 && _rightTurning)
            ChangeTurning();
        else if (moveValue > 0 && !_rightTurning)
            ChangeTurning();
        Vector2 targetVelocity = new Vector2(moveValue, rb2d.velocity.y);
        //Функция, которая сглаживает перемещение персонажа
        rb2d.velocity = Vector2.SmoothDamp(rb2d.velocity, targetVelocity, ref _zeroVelocity, smoothingKoef);
    }

    void JumpPlayer(bool jump)
    {
        if(jump && (_jumpsLeft > 0))
        {
            animator.SetTrigger("isJump");
            //если не можем управлять персонажем в воздухе, то гасим только Y-состовляющую velocity, чтобы прыжок был направлен в сторону движения.
            //если можем - убираем velocity полностью, чтобы при падении дабл джамп работал корректно (не просто гасилось бы падение, а производился прыжок)
            rb2d.velocity = (canAirControl) ? Vector2.zero : new Vector2(rb2d.velocity.x, 0f);
            if (_jumpsLeft < jumpsCount)
                rb2d.AddForce(new Vector2(0f, extraJumpForce), ForceMode2D.Force);
            else
                rb2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Force);
            _isGrounded = false;
            _jumpsLeft -= 1;
            
        }
    }

    void ChangeTurning()
    {
        _rightTurning = !_rightTurning;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
