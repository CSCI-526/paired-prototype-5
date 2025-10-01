using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class OvalPlayerControl : MonoBehaviour
{
    //set the movement speed in the Inspector window
    public float speed = 5.0f;

    public float jumpForce = 10.0f;

    //double jump code
    public int maxJumps = 3; 
    private int jumpCount; 

    private Rigidbody2D rb;


    private void Start()
    {
        // get the Rigidbody2D component attached to this game object
        rb = GetComponent<Rigidbody2D>();
        jumpCount = maxJumps; // when the game starts, the jump count is full
    }
    

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow) && jumpCount > 0)
        {
            // 1. before applying force, first clear the current vertical velocity
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

            // 2. apply an upward instantaneous force
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            
            // 3. consume one jump count
            jumpCount--;
        }
    }

    // Handle physics-based movement.
    private void FixedUpdate()
    {
        // 1. get the input
        // create a float to store the horizontal movement direction (-1 for left, 1 for right, 0 for none)
        float moveHorizontal = 0f;

        // check if the right arrow key is pressed
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveHorizontal = 1f;
        }
        // check if the left arrow key is pressed
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveHorizontal = -1f;
        }

        // 2. apply the movement
        // modify the rigidbody's speed to implement the movement
        // X axis speed = direction * speed
        // Y axis speed = keep the current vertical speed (so that gravity can work!)
        rb.linearVelocity = new Vector2(moveHorizontal * speed, rb.linearVelocity.y);
    }

    //reset the jump count
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // when the player contacts the ground, reset the jump count
            jumpCount = maxJumps;
        }
    }
}