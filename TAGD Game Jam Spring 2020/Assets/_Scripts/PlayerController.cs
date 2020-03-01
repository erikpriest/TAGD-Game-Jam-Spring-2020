using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    // Variable declaration
    private Rigidbody2D m_rb2d;
    private Animator m_anim;
    private GroundCheck m_groundCheck;


    [Header("Movement Attributes")]
    private bool m_grounded;
    [SerializeField] private float m_gravity = 2f;
    [SerializeField] private float m_speed = 4f;
    [SerializeField] private float m_jumpHeight = 1f;
    [SerializeField][Range(0,1)] private float m_airSpeedReduction = 0.5f;
    [SerializeField] private float m_runningSpeed = 5f;
    [SerializeField] private float m_fallSpeedBoost = 1.5f;
    private Vector2 m_moveDirection = Vector2.zero;
    private bool m_running = false;
    private float m_horizontalAxis = 0f;
    private bool m_jumped;

    //public event Action<InputAction.CallbackContext> onActionTriggered;


    // Animator Hashes
    protected readonly int m_HashHorizontalVelocity = Animator.StringToHash("HorizontalVelocity");
    protected readonly int m_HashJumped = Animator.StringToHash("Jumped");
    protected readonly int m_HashGrounded = Animator.StringToHash("Grounded");
    protected readonly int m_HashVerticalVelocity = Animator.StringToHash("VerticalVelocity");

    // Start is called before the first frame update
    void Start()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
    }


    private void FixedUpdate()
    {
        // Seperate the difference in the vector of displacement vector
        // movement vector is the displacement vector

        // X value of walking on ground
        // y value if not in air
        m_moveDirection.x = m_horizontalAxis * m_speed * Time.deltaTime;

        //If character is grounded, and is falling then set move direction to zero.
        if(m_grounded && m_moveDirection.y < 0)
        {
            m_moveDirection.y = 0;
        }

        // Values if character is running
        if (m_running)
        {
            m_moveDirection.x = m_horizontalAxis * m_runningSpeed * Time.deltaTime;
        }

        //If the character jumped
        if (m_jumped)
        {
            m_moveDirection.y = m_jumpHeight * Time.deltaTime;
            m_jumped = false;
        }

        if(!m_grounded)
        {
            //X value if in air
            // y value if in air
            m_moveDirection.x = m_horizontalAxis * m_speed * m_airSpeedReduction * Time.deltaTime;           
            // Condition: If move direction is negative then increase gravity by a factor set in inspector
            if(m_moveDirection.y < 0)
            {
                m_moveDirection.y -= m_gravity * m_fallSpeedBoost * Time.deltaTime;
            }
            else
            {
                m_moveDirection.y -= m_gravity * Time.deltaTime;
            }

        }

        

        //Debug.Ray of movement vectors for better understanding
        //X Direction
        Debug.DrawRay(m_rb2d.position, new Vector2(m_moveDirection.x, 0), Color.red);
        //Y Direction
        Debug.DrawRay(m_rb2d.position, new Vector2(0, m_moveDirection.y), Color.blue);

        // send the vector into the move funciton to move the player if the the direction is none zero
        Move(m_moveDirection);

    }

    private void Move(Vector2 directionVector)
    {
        //Update animator values for displacement vector
        // Put in the value of the movement into movePosition of rigidbody
        //Apply time. delta time and such
        m_anim.SetFloat(m_HashVerticalVelocity, directionVector.normalized.y);

        m_rb2d.MovePosition(m_rb2d.position + directionVector);
    }



    private void Grounded()
    {
        m_grounded = true;
        m_anim.SetBool(m_HashGrounded, m_grounded);
    }

    private void NotGrounded()
    {
        m_grounded = false;
        m_anim.SetBool(m_HashGrounded, m_grounded);
    }


    #region // Input related methods
    void OnMovement(InputValue value)
    {
        m_horizontalAxis = value.Get<float>();
        m_anim.SetFloat(m_HashHorizontalVelocity, m_horizontalAxis);
    }

    void OnRun(InputValue value)
    {
        m_running = (value.isPressed) && (m_grounded) ? true : false;
    }

    void OnJump()
    {
        if(m_grounded)
        {
            m_jumped = true;
            m_anim.SetTrigger(m_HashJumped);
        }
    }
    #endregion
}
