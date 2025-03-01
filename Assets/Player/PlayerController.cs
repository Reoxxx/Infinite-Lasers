using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;

    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float airSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    private float dashTimeCounter = 0;
    [SerializeField] float dashCooldown;
    private float dashCooldownCounter = 0;

    private float moveHorizontal;
    private float moveVertical;
    private bool isJumping;
    private bool onGround;
    private bool isDashing;
    private bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void FixedUpdate()
    {
        GetInput();
        Dash();          
        Move();
        Jump();
    }

    // Update is called once per frame
    void Update()
    {
        LookForward();
        AnimationController();
    }

    void GetInput()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space) && onGround)
        {
            isJumping = true;
        }

        if (dashCooldownCounter < dashCooldown)
        {
            dashCooldownCounter += Time.deltaTime;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift) && !isDashing)
            {
                isDashing = true;
            }
        }

    }

    void Move()
    {
        if (!canMove) return;

        if (onGround)
        {
            rb.linearVelocity = new Vector3(moveHorizontal * speed, rb.linearVelocity.y, moveVertical * speed); ;
        }
        else
        {
            rb.linearVelocity = new Vector3(moveHorizontal * airSpeed, rb.linearVelocity.y, moveVertical * airSpeed); ;
        }
    }

    void Jump()
    {
        if (!isJumping) return;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpSpeed, rb.linearVelocity.z);
        isJumping = false;
    }

    void Dash()
    {
        if(!isDashing) return;

        canMove = false;
        isJumping = false;

        if (dashTimeCounter < dashTime)
        {
            rb.linearVelocity = transform.forward * dashSpeed;
            dashTimeCounter += Time.deltaTime;
        }
        else
        {
            isDashing = false;
            canMove = true;
            dashTimeCounter = 0;
            dashCooldownCounter = 0;
        }
    }

    private void AnimationController()
    {
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (!onGround)
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }

        if (isDashing)
        {
            animator.SetBool("isDashing", true);
        }
        else
        {
            animator.SetBool("isDashing", false);
        }
    }

    private void LookForward()
    {
        if (moveHorizontal != 0 || moveVertical != 0 && !isDashing)
            transform.forward = new Vector3(moveHorizontal, 0, moveVertical);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }
    }
}
