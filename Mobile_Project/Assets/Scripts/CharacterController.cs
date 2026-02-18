using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
    private Rigidbody2D rb2d;

    //Liste du transorm des �l�ment qui composent le personnage pour pouvoir le retourner
    [SerializeField] private List<Transform> spriteList;


    [Header("MOVE")] private bool startMoving = false;
    [SerializeField] private float runSpeed;
    [SerializeField] private float climbSpeed;
    [SerializeField] private UnityEvent onWalk;
    [SerializeField] private UnityEvent onClimb;
    private int horizontal = 1; //Permet de savoir si on va � gauche ou � droite pour l'instant

    [Header("GROUND CHECK")] [SerializeField]
    private LayerMask groundLayer;

    [SerializeField] private Vector2 groundCheck;
    [SerializeField] private float groundCastDistance;

    [Header("FRONT CHECK")] [SerializeField]
    private LayerMask wallLayer;

    [SerializeField] private LayerMask actionLayer;
    [SerializeField] private Vector2 frontCheck;
    [SerializeField] private float wallCastDistance;

    [Header("WATER CHECK")]
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private Vector2 waterCheck;

    private GameObject selectedObject;
    private bool isClimbing;

    public bool StartMoving
    {
        get => startMoving;
        set => startMoving = value;
    }

    void Start()
    {
        TryGetComponent(out rb2d);
    }

    Vector3 _lastPosition;
    [SerializeField] private float _distance = .1f;
    [SerializeField] private bool _isEnnemie;

    float _timer = 5f;

    void FixedUpdate()
    {
        if (StartMoving && LevelClearedPanel.Instance is null)
        {
            if(((Vector3.Distance(_lastPosition, transform.position) < _distance) || IsUnderWater()) && !_isEnnemie)
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


            //Si le perso peut monter alors il monte
            if (isClimbing)
            {
                onClimb?.Invoke();
                ClimbMove();
            }

            //Si le perso est au sol et qu'il n'y a pas de lierre devant alors le perso bouge
            if (IsGrounded() && !IsIvyInFront())
            {
                onWalk?.Invoke();
                Move();
            }

            //Si on est au sol et qu'il il y a une lierre :
            //- Il peut monter
            //- On enl�ve la simulation du RigidBody
            else if (IsGrounded() && IsIvyInFront())
            {
                isClimbing = true;
                rb2d.simulated = false;
            }

            _lastPosition = transform.position;
        }
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

        Vector3 direction = horizontal * Vector2.right;


        transform.position += direction * runSpeed * Time.deltaTime;
    }


    //MoveTowards le haut d'un objet Ivy � une vitesse modifiable
    private void ClimbMove()
    {
        Vector2 target = TargetUp(); //La position vers laquelle on va
        transform.position = Vector2.MoveTowards(
            transform.position,
            target,
            climbSpeed * Time.fixedDeltaTime
        );
    }

    //Permet de trouver le haut d'un objet de layer actionLayer � l'aide d'un Raycast
    private Vector2 TargetUp()
    {
        Vector2 targetPos = transform.position;

        //Origine de l'endroit d'o� le Raycast est lanc� (le centre du Boxcast pour le sol dans IsGrounded())
        Vector2 raycastOrigin = new Vector2(transform.position.x, transform.position.y + groundCastDistance);

        //- Lance un raycast depuis raycastOrigin
        //- En direction de la droite
        //- De longueur : addition de la longueur en x de la Boxcast frontCheck et de la distance du cast
        //- Doit toucher un objet de layer dans actionLayer
        RaycastHit2D hit = Physics2D.Raycast(
            raycastOrigin,
            transform.right,
            frontCheck.x + wallCastDistance,
            actionLayer
        );

        //Si le Raycast hit un objet du bon layer :
        //- selectedObject devient le l'objet qui est hit
        //- On cr�e le point que vers lequel le perso monte
        //- On le retourne
        if (hit.collider != null)
        {
            selectedObject = hit.collider.gameObject;
            float targetY = selectedObject.transform.position.y + selectedObject.transform.localScale.y;
            targetPos = new Vector2(transform.position.x, targetY);
            return targetPos;
        }
        //Si le perso ne touche rien, alors il peut bouger
        else
        {
            rb2d.simulated = true;
        }

        return targetPos;
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

    private bool IsWallInFront() //Check en horizontal
    {
        if (Physics2D.BoxCast(transform.position + transform.right * wallCastDistance, frontCheck, 0, transform.right,
                0, wallLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsIvyInFront() //Check en horizontal (m�me information que pour le check du mur)
    {
        if (Physics2D.BoxCast(transform.position + transform.right * wallCastDistance, frontCheck, 0, transform.right,
                0, actionLayer))
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


    //Permet de visualiser les BoxCasts et les Raycasts utilis�s
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + transform.up * groundCastDistance, groundCheck);
        Gizmos.DrawWireCube(transform.position + transform.right * wallCastDistance, frontCheck);
        Gizmos.DrawWireCube(transform.position, waterCheck);

        Vector3 rayOrigin = new Vector3(
            transform.position.x,
            transform.position.y + groundCastDistance,
            transform.position.z
        );
        Vector3 rayDirection = transform.right * (frontCheck.x + wallCastDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(rayOrigin, rayOrigin + rayDirection);
    }
}