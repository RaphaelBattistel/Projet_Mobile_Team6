using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Rigidbody2D rb2d;

    [SerializeField] private List<Transform> spriteList;


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
    }

    void FixedUpdate()
    {
        if (isClimbing)
        {
            ClimbMove();
        }

        if (IsGrounded() && !IsIvyInFront())
        {
            Move();
        }

        else if (IsGrounded() && IsIvyInFront())
        {
            isClimbing = true;
            rb2d.simulated = false;
        }
    }

    private void Move()
    {
        if (IsWallInFront())
        {
            
            //foreach (Transform sprite in spriteList)
            //{
            //    sprite.position *= new Vector3(1, 1, -1); 
            //    sprite.eulerAngles += new Vector3(0, 180, 0);
            //}
            
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

    private bool IsIvyInFront()
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

        // Visualisation du Raycast de TargetUp
        Vector3 rayOrigin = new Vector3(
            transform.position.x, 
            transform.position.y + groundCastDistance,
            transform.position.z
        );
        Vector3 rayDirection = transform.right * (frontCheck.x + wallCastDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(rayOrigin, rayOrigin + rayDirection);
    }


    private void ClimbMove()
    {
        Vector2 target = TargetUp();
        transform.position = Vector2.MoveTowards(
            transform.position,
            target,
            climbSpeed * Time.fixedDeltaTime
        );
    }

    private Vector2 TargetUp()
    {
        Vector2 targetPos = transform.position;
        //RaycastHit2D hit = Physics2D.Raycast(
        //    new Vector2(transform.position.x + wallCastDistance, transform.position.y - transform.localScale.y/2),
        //    transform.right,
        //    frontCheck.x,
        //    actionLayer
        //);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(
            transform.position.x, transform.position.y + groundCastDistance),
            transform.right,
            frontCheck.x + wallCastDistance,
            actionLayer
        );
        //transform.position, groundCheck, 0, transform.up, groundCastDistance
        if (hit.collider != null)
        {
            selectedObject = hit.collider.gameObject;
            float targetY =
                selectedObject.transform.position.y
                + selectedObject.transform.localScale.y;
            targetPos = new Vector2(transform.position.x, targetY);
            return targetPos;
        }
        else
        {
            rb2d.simulated = true;
        }
        return targetPos;
    }


}
