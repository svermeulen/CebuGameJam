using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerPlatformerController : PhysicsObject
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        GameRegistry.Instance.AddPlayer(this);

        var targets = GameRegistry.Instance.TargetGroup.m_Targets.ToList();

        targets.Add(new Cinemachine.CinemachineTargetGroup.Target()
            {
                target = this.transform,
                weight = 1.0f,
                radius = 0.0f,
            });

        GameRegistry.Instance.TargetGroup.m_Targets = targets.ToArray();
    }

    public void OnTriggerEnter2D(Collider2D theCollider)
    {
        if (theCollider.CompareTag("Killzone"))
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy();

        GameController.Instance.OnPlayerDied(this);
    }

    void Destroy()
    {
        GameRegistry.Instance.RemovePlayer(this);
        GameObject.Destroy(this.gameObject);
    }

    public void Exit()
    {
        Destroy();

        GameController.Instance.OnPlayerExited(this);
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
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

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }
}
