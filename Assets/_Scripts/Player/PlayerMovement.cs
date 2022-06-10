using System;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float rayLength;
    public float fallMultiplier;
    public float lowJumpMultiplier;

    public GameObject colliderAttackRight;
    public GameObject colliderAttackLeft;
    public LayerMask groundMask;
    public Transform groundCheck;
    
    [SerializeField]
    private float horizontal, vertical;
    
    [SerializeField]
    private bool isJumping;
    [SerializeField]
    private bool isOnGround;
    [SerializeField] 
    private bool isAttacking;
    private bool isSpecialAttack;
    
    private Rigidbody2D rb;
    private Animator anim;
    private Ray2D ray;
    private RaycastHit2D hit;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        InputPlayer();
        GroundDetection();
        JumpPressed();
        Animating();
        Flip();
        AttackPressed();
    }

    private void FixedUpdate()
    {
        Movement();
        Jump();
        BetterJumping();
        Attack();
    }

    private void InputPlayer()
    {
        if (!isAttacking)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }
    }

    void Animating()
    {
        if (horizontal != 0)
            anim.SetBool("IsMoving", true);
        else 
            anim.SetBool("IsMoving", false);
        
        anim.SetBool("IsJumping", !isOnGround);
        anim.SetFloat("VelocityY", rb.velocity.y);
    }

    void Flip()
    {
        if (horizontal > 0)
            spriteRenderer.flipX = false;
        else if (horizontal < 0)
            spriteRenderer.flipX = true;
    }

    void Movement()
    {
        if (!isAttacking)
        {
            Vector2 direction = new Vector2(horizontal, vertical);

            rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.zero;
            horizontal = 0;
        }
        
    }

    void GroundDetection()
    {
        isOnGround = Physics2D.Raycast(groundCheck.position, Vector2.down, 
            rayLength, groundMask);
        Debug.DrawRay(groundCheck.position, Vector2.down * rayLength, Color.red);
    }

    void JumpPressed()
    {
        if (Input.GetKey(KeyCode.Space) && isOnGround)
            isJumping = true;
    }

    void Jump()
    {
        if (isJumping)
        {
            isJumping = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += Vector2.up * jumpForce;
        }
    }

    void BetterJumping()
    {
        if (rb.velocity.y < 0)
            rb.velocity += Vector2.up * (Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            rb.velocity += Vector2.up * (Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
    }

    void AttackPressed()
    {
        if (Input.GetMouseButtonDown(0) && isOnGround)
            isAttacking = true;

        if (Input.GetMouseButtonDown(1) && isOnGround)
            isSpecialAttack = true;
    }

    void Attack()
    {
        if ((!isAttacking || isSpecialAttack) && Input.GetMouseButtonDown(0) && isOnGround)
        {
            isAttacking = true;
            rb.velocity = Vector2.zero;
            
            anim.SetTrigger("Attack");
            
            if (isSpecialAttack) 
            {
                anim.SetInteger("AttackSelector", 2);
            }
            else
            {
                int n = Random.Range(0, 2);
                anim.SetInteger("AttackSelector", n); 
            }
        }
    }

    void EnableAttackCollider()
    {
        if (!spriteRenderer.flipX)
            colliderAttackRight.SetActive(true);
        else
            colliderAttackLeft.SetActive(true);
        
        Invoke("DisableAttackCollider", 0.1f);
    }

    void DisableAttackCollider()
    {
        colliderAttackRight.SetActive(false);
        colliderAttackLeft.SetActive(false);
    }

    void AttackToFalse()
    {
        isAttacking = false;
        isSpecialAttack = false;
    } 

}
