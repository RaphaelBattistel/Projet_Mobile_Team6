using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
    private Rigidbody2D rb2D;

    [Header("MOVE")]
    [SerializeField] private bool startMoving = false;
    [SerializeField] private float runSpeed;
    [SerializeField] private float slowingSpeed;
    [SerializeField] private float mudScaleSpeed;
    [SerializeField] private UnityEvent onWalk;

    [Header("GROUND CHECK")] [SerializeField]
    private LayerMask groundLayer;

    [SerializeField] private LayerMask _mudlayer;

    [SerializeField] private Vector2 groundCheck;
    [SerializeField] private float groundCastDistance;

    [Header("FRONT CHECK")] [SerializeField]
    private LayerMask wallLayer;

    [SerializeField] private Vector2 frontCheck;
    [SerializeField] private Vector3 wallCastoffset;

    [Header("SLOPE CHECK")] [SerializeField]
    private Vector2 slopeCheck;

    [SerializeField] private Vector3 slopeCastoffset;

    [Header("WATER CHECK")] [SerializeField]
    private LayerMask waterLayer;

    [SerializeField] private Vector2 waterCheck;

    [SerializeField] private Animator animator;

    public bool StartMoving
    {
        get => startMoving;
        set => startMoving = value;
    }

    void Start()
    {
        TryGetComponent(out rb2D);
    }

    [SerializeField] private bool _isEnnemie;

    float _timer = 5f;

    void FixedUpdate()
    {
        if (StartMoving && LevelClearedPanel.Instance is null)
        {
            if ((rb2D.linearVelocity.magnitude < .1f || IsUnderWater()) && !_isEnnemie)
            {
                _timer -= Time.fixedDeltaTime;
                if (_timer <= 0 && LossPanel.Instance is null)
                {
                    GameManager.Instance.HandlePlayerLoss();
                }
            }
            else
            {
                _timer = 5f;
            }

            if (IsGrounded() && !IsWallInFront())
            {
                onWalk?.Invoke();
                Move();
            }
            else
            {
                rb2D.linearVelocity = Vector2.Lerp(rb2D.linearVelocity, Vector2.zero, slowingSpeed * Time.fixedDeltaTime);
            }
        }
        else
        {
            rb2D.linearVelocity = Vector2.Lerp(rb2D.linearVelocity, Vector2.zero, slowingSpeed * Time.fixedDeltaTime);
        }
        animator.SetFloat("Speed", Mathf.Abs(rb2D.linearVelocity.x));
        animator.SetBool("IsGrounded", IsGrounded());
    }


    //Mouvement horizontal du personnage
    private void Move()
    {
        //Si un mur ou obstacle est devant, alors le perso va de l'autre c�t� et les casts se sont du c�t� oppos�
        //if (IsWallInFront())
        //{
        //    //Tentative pour retourner le personnage 
        //
        //    //foreach (Transform sprite in spriteList)
        //    //{
        //    //    sprite.position *= new Vector3(1, 1, -1); 
        //    //    sprite.eulerAngles += new Vector3(0, 180, 0);
        //    //}
        //    
        //    wallCastDistance *= -1;
        //    horizontal *= -1;
        //}
        float speedBonus = 0f;

        if (IsSlope() && !IsMuded())
        {
            speedBonus = runSpeed / 2f;
        }

        float speed = IsMuded() ? mudScaleSpeed * runSpeed : runSpeed;

        rb2D.linearVelocity = new Vector2(speed + speedBonus, rb2D.linearVelocity.y);
    }

    //Check si le personnage touche un certain layer avec des BoxCasts
    private bool IsGrounded() //Check en lat�ral
    {
        if (Physics2D.BoxCast(transform.position + transform.up * groundCastDistance, groundCheck, 0, transform.up, 0,
                groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Check si le personnage touche un certain layer avec des BoxCasts
    private bool IsMuded() //Check en lat�ral
    {
        if (Physics2D.BoxCast(transform.position + transform.up * groundCastDistance, groundCheck, 0, transform.up, 0,
                _mudlayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsWallInFront() //Check en horizontal
    {
        if (Physics2D.BoxCast(transform.position + wallCastoffset, frontCheck, 0, transform.right,
                0, wallLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsUnderWater()
    {
        if (Physics2D.BoxCast(transform.position, waterCheck, 0, transform.up, 0, waterLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsSlope()
    {
        if (Physics2D.BoxCast(transform.position + slopeCastoffset, slopeCheck, 0, transform.up, 0, wallLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Permet de visualiser les BoxCasts et les Raycasts utilis�s
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + transform.up * groundCastDistance, groundCheck);
        Gizmos.DrawWireCube(transform.position + slopeCastoffset, slopeCheck);
        Gizmos.DrawWireCube(transform.position + wallCastoffset, frontCheck);
        Gizmos.DrawWireCube(transform.position, waterCheck);
    }
}