using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal; 
    private float speed = 10;
    private float jumpingPower  = 16f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        Flip();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) 
        {
            isFacingRight = !isFacingRight; 
            Vector3 localScale = transform.localScale;
            localScale.x  *= -1f;
            transform.localScale = localScale;
        }
    }
}
