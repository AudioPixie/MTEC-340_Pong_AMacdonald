using UnityEngine;

public class Paddle_Movement : MonoBehaviour
{

    public float movementSpeed;

    public float yEdge;

    public KeyCode up;
    public KeyCode down;

    private SpriteRenderer PaddleSprite;

    private void Awake()
    {
        PaddleSprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        PaddleSprite.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.Instance.State != "Start")
            PaddleSprite.enabled = true;

        if (GameManager.Instance.State == "GameOver")
            PaddleSprite.enabled = false;

        if (GameManager.Instance.State != "Pause")
        {
            if (Input.GetKey(up) && transform.position.y <= yEdge)
                transform.position += new Vector3(0, movementSpeed * Time.deltaTime, 0);

            else if (Input.GetKey(down) && transform.position.y >= -yEdge)
                transform.position -= new Vector3(0, movementSpeed * Time.deltaTime, 0);
        }
    }
}