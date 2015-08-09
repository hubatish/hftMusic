using HappyFunTimes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovement : MessageActor
{
    public float maxSpeed = 10;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float jumpForce = 700f;

    private float m_direction = 0.0f;
    private bool m_jumpPressed = false;      // true if currently held down
    private bool m_jumpJustPressed = false;  // true if pressed just now
    private float m_groundRadius = 0.2f;
    private bool m_grounded = false;
    private bool m_facingRight = true;
    private Animator m_animator;
    private Rigidbody2D m_rigidbody2d;

    public override void OnMove(MessageMove data)
    {
        m_direction = data.dir;
    }

    public override void OnJump(MessageJump data)
    {
        m_jumpJustPressed = data.jump && !m_jumpPressed;
        m_jumpPressed = data.jump;
    }

    public override void Init()
    {
        if (m_animator == null)
        {
            //Only call this once
            m_animator = GetComponent<Animator>();
            m_rigidbody2d = GetComponent<Rigidbody2D>();
            MoveToRandomSpawnPoint();
        }
    }

    void Update()
    {
        // If we're on the ground AND we pressed jump (or space)
        if (m_grounded && (m_jumpJustPressed || Input.GetKeyDown("space")))
        {
            m_grounded = false;
            m_animator.SetBool("Ground", m_grounded);
            m_rigidbody2d.AddForce(new Vector2(0, jumpForce));
        }
        m_jumpJustPressed = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check if the center under us is touching the ground and
        // pass that info to the Animator
        m_grounded = Physics2D.OverlapCircle(groundCheck.position, m_groundRadius, whatIsGround);
        m_animator.SetBool("Ground", m_grounded);

        // Pass our vertical speed to the animator
        m_animator.SetFloat("vSpeed", m_rigidbody2d.velocity.y);

        // Get left/right input (get both phone and local input)
        float move = m_direction + Input.GetAxis("Horizontal");

        // Pass that to the animator
        m_animator.SetFloat("Speed", Mathf.Abs(move));

        // and move us
        m_rigidbody2d.velocity = new Vector2(move * maxSpeed, m_rigidbody2d.velocity.y);
        if (move > 0 && !m_facingRight)
        {
            Flip();
        }
        else if (move < 0 && m_facingRight)
        {
            Flip();
        }

        if (transform.position.y < LevelSettings.settings.bottomOfLevel.position.y)
        {
            MoveToRandomSpawnPoint();
        }
    }

    void MoveToRandomSpawnPoint()
    {
        // Pick a random spawn point
        int ndx = UnityEngine.Random.Range(0, LevelSettings.settings.spawnPoints.Length - 1);
        transform.localPosition = LevelSettings.settings.spawnPoints[ndx].localPosition;
    }

    void Flip()
    {
        m_facingRight = !m_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}

