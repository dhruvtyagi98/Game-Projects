using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bird : MonoBehaviour
{
    private Rigidbody2D birdrigidbody2D;
    private const float JumpAmount = 100f;
    private static Bird Instance;
    private State state;

    private enum State
    {
        WaitingToStart,
        Playing,
        Dead,
    }
    public static Bird GetInstance()
    {
        return Instance;
    }

    public event EventHandler onDied;
    public event EventHandler onStartedPlaying;
    private void Awake()
    {
        Instance = this;
        birdrigidbody2D = GetComponent<Rigidbody2D>();
        birdrigidbody2D.bodyType = RigidbodyType2D.Static;
        state = State.WaitingToStart;

    }
    private void Update()
    {
        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    state = State.Playing;
                    birdrigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    if (onStartedPlaying != null) onStartedPlaying(this, EventArgs.Empty);
                }
                break;

            case State.Playing:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    Jump();
                }
                break;

            case State.Dead:
                break;
        }
    }

    private void Jump()
    {
        birdrigidbody2D.velocity = Vector2.up * JumpAmount;
        SoundManager.PlaySound(SoundManager.Sound.birdJump);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        birdrigidbody2D.bodyType = RigidbodyType2D.Static;
        if (onDied != null) onDied(this, EventArgs.Empty);
    }
}
