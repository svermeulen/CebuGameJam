using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerPlatformerController : MonoBehaviour
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    SpriteRenderer spriteRenderer;
    Animator animator;
    bool _isFrozen;
    bool _isDead;
    bool _isReverse;
    Vector2 _lastMove;

    public static PlayerPlatformerController MainPlayer
    {
        get; private set;
    }

    public float PlatformMove
    {
        get; set;
    }

    public bool IsGrounded
    {
        get { return grounded; }
    }

    public Vector2 LastMove
    {
        get { return _lastMove; }
    }

    void OnTriggerEnter2D(Collider2D theCollider)
    {
        if (theCollider.CompareTag("Killzone"))
        {
            Die(true);
        }
    }

    public void SetIsReverse(bool isReverse)
    {
        _isReverse = isReverse;
    }

    public void SetIsFrozen(bool isFrozen)
    {
        _isFrozen = isFrozen;

        if (isFrozen)
        {
            gameObject.layer = 9;
        }
        else
        {
            gameObject.layer = 8;
        }

        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    }

    void Awake()
    {
        if (MainPlayer == null)
        {
            MainPlayer = this;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        GameRegistry.Instance.AddPlayer(this);
        AddToTargetGroup();
    }

    public void AddToTargetGroup()
    {
        var targets = GameRegistry.Instance.TargetGroup.m_Targets.ToList();

        targets.Add(new Cinemachine.CinemachineTargetGroup.Target()
            {
                target = this.transform,
                weight = 1.0f,
                radius = 1.0f,
            });

        GameRegistry.Instance.TargetGroup.m_Targets = targets.ToArray();
    }

    public void RemoveFromTargetGroup()
    {
        GameRegistry.Instance.TargetGroup.m_Targets = GameRegistry.Instance.TargetGroup.m_Targets
            .Where(x => x.target != this.transform).ToArray();
    }

    public void Die(bool hideImmediately)
    {
        if (!_isDead)
        {
            _isDead = true;
            GameController.Instance.OnPlayerDied(this);

            if (hideImmediately)
            {
                SetIsFrozen(true);
                animator.enabled = false;
            }
            else
            {
                Invoke("HideAndStop", 0.5f);
            }
        }
    }

    void HideAndStop()
    {
        SetIsFrozen(true);
        animator.enabled = false;
        spriteRenderer.enabled = false;
    }

    public void Exit()
    {
        HideAndStop();
        GameRegistry.Instance.RemovePlayer(this);
        GameController.Instance.OnPlayerExited(this);
    }

    void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        if (!_isFrozen)
        {
            move.x = Input.GetAxis("Horizontal") * (_isReverse ? -1 : 1);

            if (Input.GetButtonDown("Jump") && grounded)
            {
                velocity.y = jumpTakeOffSpeed;

                if (MainPlayer == this)
                {
                    SoundManager.Instance.PlayJump();
                }
            }
            else if (Input.GetButtonUp("Jump"))
            {
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * 0.5f;
                }
            }

            if (move.x > 0.01f)
            {
                if (spriteRenderer.flipX == true)
                {
                    spriteRenderer.flipX = false;
                }
            }
            else if (move.x < -0.01f)
            {
                if (spriteRenderer.flipX == false)
                {
                    spriteRenderer.flipX = true;
                }
            }
        }

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(move.x) / maxSpeed);

        _lastMove = move;

        move.x += PlatformMove;

        targetVelocity = move * maxSpeed;

        PlatformMove = 0;
    }

    public float minGroundNormalY = .65f;
    public float gravityModifier = 1f;

    Vector2 targetVelocity;
    bool grounded;
    Vector2 groundNormal;
    Rigidbody2D rb2d;
    Vector2 velocity;
    ContactFilter2D contactFilter;
    RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    List<RaycastHit2D> hitBufferList = new List<RaycastHit2D> (16);

    const float minMoveDistance = 0.001f;
    const float shellRadius = 0.01f;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D> ();
    }

    void Start ()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask (gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    void Update ()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity ();
    }

    void FixedUpdate()
    {
        if (_isFrozen)
        {
            rb2d.velocity = Vector2.zero;
            return;
        }

        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        grounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2 (groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement (move, false);

        move = Vector2.up * deltaPosition.y;

        Movement (move, true);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast (move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear ();
            for (int i = 0; i < count; i++) {
                hitBufferList.Add (hitBuffer [i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList [i].normal;
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot (velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList [i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }


        }

        rb2d.position = rb2d.position + move.normalized * distance;
    }
}
