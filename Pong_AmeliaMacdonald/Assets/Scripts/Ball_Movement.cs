using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Movement : MonoBehaviour
{

    public float yEdge;
    public float xBounds;

    private Vector2 velocity;

    private Rigidbody2D rb2d;
    private SpriteRenderer BallSprite;

    public AudioClip collisionSound;
    public AudioClip deathSound;


    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        BallSprite = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();
        BallSprite.enabled = false;
    }


    private void FixedUpdate()
    {
        if (GameManager.Instance.State == "Serve")
            BallSprite.enabled = true;

        if (GameManager.Instance.State == "Play")
        {
            rb2d.MovePosition(rb2d.position + velocity * Time.fixedDeltaTime);

            if (Mathf.Abs(rb2d.position.y) >= yEdge)
                WallCollision();

            if (Mathf.Abs(rb2d.position.x) >= xBounds)
                Death();
        }

    }


    private void Death()
    {
        GameManager.Instance.PlaySound(deathSound);
        GameManager.Instance.UpdateScore(rb2d.position.x > 0 ? 1 : 2);
        Reset();
    }


    private void WallCollision()
    {
        velocity.y *= -1;

        rb2d.MovePosition(new Vector2(
            rb2d.position.x,
            rb2d.position.y > 0 ? yEdge - 0.01f : -yEdge + 0.01f
        ));

        GameManager.Instance.PlaySound(collisionSound);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            velocity.x *= -1;

            velocity.x = IncrementSpeed(velocity.x);
            velocity.y = IncrementSpeed(velocity.y);

            GameManager.Instance.PlaySound(collisionSound);
        }
    }


    private float IncrementSpeed(float axis)
    {
        /* Long form:
        
        if (axis > 0)
            return axis + GameManager.Instance.ballSpeedIncrement;
        else
            return axis - GameManager.Instance.ballSpeedIncrement;

        */

        axis += axis > 0 ? GameManager.Instance.ballSpeedIncrement : -GameManager.Instance.ballSpeedIncrement;
        return axis;
    }


    private void Reset()
    {
        if (GameManager.Instance.State != "GameOver")
        {
            GameManager.Instance.State = "Serve";
            GameManager.Instance.messagesGUI.enabled = true;
        }

        if (GameManager.Instance.State == "GameOver")
            BallSprite.enabled = false;

            transform.position = new Vector3(0, 0, 0);

        velocity = new Vector2(
            GameManager.Instance.initBallSpeed * (Random.Range(0.0f, 1.0f) > 0.5f ? 1 : -1),
            GameManager.Instance.initBallSpeed * (Random.Range(0.0f, 1.0f) > 0.5f ? 1 : -1)
        );
    }
}