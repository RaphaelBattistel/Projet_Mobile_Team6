using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private SpriteRenderer sprite;

    [Header("MOVE")]
    [SerializeField] private float runSpeed;
    [SerializeField] private float climbSpeed;
    private int horizontal = 1;
    
    [Header("GROUND CHECK")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheck;
    [SerializeField] private float groundCastDistance;

    [Header("FRONT CHECK")]
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask actionLayer;
    [SerializeField] private Vector2 frontCheck;
    [SerializeField] private float wallCastDistance;

    private GameObject selectedObject;
    private bool isClimbing;

    void Start()
    {
        TryGetComponent(out rb2d);
        TryGetComponent(out sprite);
    }

    void FixedUpdate()
    {
        if (isClimbing)
        {
            ClimbMove();
            return;
        }

        if (IsGrounded() && !IsActionInFront())
            Move();

        else if (IsGrounded() && IsActionInFront())
        {
            isClimbing = true;
            rb2d.simulated = false;
        }
    }

    private void Move()
    {
        if (IsWallInFront())
        {
            if (sprite.flipX)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
            wallCastDistance *= -1;
            horizontal *= -1;
        }

        Vector3 direction = horizontal * Vector2.right;


        transform.position += direction * runSpeed * Time.deltaTime;
    }

    private bool IsGrounded()
    {
        if (Physics2D.BoxCast(transform.position, groundCheck, 0, transform.up, groundCastDistance, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsWallInFront()
    {
        if (Physics2D.BoxCast(transform.position, frontCheck, 0, transform.right, wallCastDistance, wallLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsActionInFront()
    {
        if (Physics2D.BoxCast(transform.position, frontCheck, 0, transform.right, wallCastDistance, actionLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + transform.up * groundCastDistance, groundCheck);
        Gizmos.DrawWireCube(transform.position + transform.right * wallCastDistance, frontCheck);

    }

    private bool CanClimb()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + wallCastDistance, transform.position.y), transform.right, frontCheck.x, actionLayer);
        if (hit.collider != null)
        {
            selectedObject = hit.collider.gameObject;
            return true;
        }
        else
        {
            return false;
        }

    }

    private void ClimbMove()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + wallCastDistance, transform.position.y), transform.right, frontCheck.x, actionLayer);
        selectedObject = hit.collider.gameObject;
        float targetY = 
            selectedObject.transform.position.y
            + selectedObject.transform.localScale.y / 2
            + transform.localScale.y / 2;

        Vector2 targetPos = new Vector2(transform.position.x, targetY);

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPos,
            climbSpeed * Time.fixedDeltaTime
        );

        if (Mathf.Abs(transform.position.y - targetY) < 0.01f)
        {
            isClimbing = false;
            rb2d.simulated = true;
        }
    }


}
