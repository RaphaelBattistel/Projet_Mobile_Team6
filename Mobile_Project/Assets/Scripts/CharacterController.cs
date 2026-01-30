using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private SpriteRenderer sprite;

    [Header("MOVE")]
    [SerializeField] private float speed;
    private int horizontal = 1;
    
    [Header("GROUND CHECK")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheck;
    [SerializeField] private float groundCastDistance;

    [Header("WALL CHECK")]
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Vector2 wallCheck;
    [SerializeField] private float wallCastDistance;



    void Start()
    {
        TryGetComponent(out rb2d);
        TryGetComponent(out sprite);
    }

    void Update()
    {
        if (IsGrounded())
        {
            Move();
        }
    }

    private void Move()
    {
        if (WallInFront())
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


        transform.position += direction * speed * Time.deltaTime;
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

    private bool WallInFront()
    {
        if (Physics2D.BoxCast(transform.position, wallCheck, 0, transform.right, wallCastDistance, wallLayer))
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
        Gizmos.DrawWireCube(transform.position + transform.right * wallCastDistance, wallCheck);

    }





}
