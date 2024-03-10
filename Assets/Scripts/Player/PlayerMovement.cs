using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Para")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Coyote Time")]
    // Thời gian mà player treo trên không trung sau khi nhảy
    [SerializeField] private float coyoteTime;
    //  Thời gian mà player vượt qua kể từ lúc chạy khỏi rìa
    private float coyoteCounter;

    [Header("Multi Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX;
    [SerializeField] private float wallJumpY;

    [Header("Player Layer")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCol;
    private float wallJumpCooldown;
    private float horizontalInput;

    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // lật người chơi khi di chuyển sang trái - phải
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        } 
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1,1,1);
        }

        // Set tham số cho animator 
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        // Có thể điều chỉnh độ cao nhảy
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
        }

        if (onWall())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (isGrounded())
            {
                coyoteCounter = coyoteTime; // Reset coyoteCounter khi ở mặt đất
                jumpCounter = extraJumps;
            }
            else
                coyoteCounter -= Time.deltaTime; // Bắt đầu giảm coyoteCounter khi không ở trên mặt đất
        }
    }

    private void Jump()
    {
        if (coyoteCounter <= 0 && !onWall() && jumpCounter <= 0) return;
        
        SoundManager.instance.PlaySound(jumpSound);
        
        if(onWall())
            WallJump();
        else
        {
            // Nếu không ở trên mặt đất và coyoteCounter > 0 thì nhảy bình thường
            if (coyoteCounter > 0)
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            else
            {
                // Nếu có thêm bước nhảy-extra jumps thì nhảy và giảm jumpCounter
                if (jumpPower > 0)
                {
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                    jumpCounter--;
                }
            }
        }

        // Đặt lại về 0 để tránh nhảy đôi
        coyoteCounter = 0;
    }
    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    
    public bool canAtk()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }

}
