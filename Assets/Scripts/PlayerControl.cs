using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 10.0f;
    public int maxJumps = 2;
    private int jumpCount;

    private Rigidbody2D rb;

    public float boostedJumpForce = 15f;

    public GameObject explosionEffect;
    public float explosionRadius = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = maxJumps;
    }

    void Update()
    {

        if (gameObject.name == "TnT Box Player"
                && Input.GetKeyDown(KeyCode.UpArrow) 
                && jumpCount > 0) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount--;
        } else if (gameObject.name == "Detonator Player"
                && Input.GetKeyDown(KeyCode.W) 
                && jumpCount > 0) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount--;
        }
    }

    private void FixedUpdate()
    {
        float moveHorizontal = 0f;

        if (gameObject.name == "TnT Box Player") {
            if (Input.GetKey(KeyCode.RightArrow))
                moveHorizontal = 1f;
            else if (Input.GetKey(KeyCode.LeftArrow))
                moveHorizontal = -1f;
        } else if (gameObject.name == "Detonator Player") {
            if (Input.GetKey(KeyCode.D))
                moveHorizontal = 1f;
            else if (Input.GetKey(KeyCode.A))
                moveHorizontal = -1f;
        }

        rb.linearVelocity = new Vector2(moveHorizontal * speed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumpCount = maxJumps; // You can double jump from anywhere

        ContactPoint2D contact = collision.contacts[0];
        bool landedOnTop = contact.normal.y > 0.5f;

        if (landedOnTop)
        {
            // Jump on enemy --> Destroy them
            if (collision.gameObject.CompareTag("Small Enemy"))
            {
                Destroy(collision.gameObject);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }

            // Jump on player --> Activate power
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerControl otherPlayer = collision.gameObject.GetComponent<PlayerControl>();

                // Extra high jump
                if (gameObject.name == "TnT Box Player" && collision.gameObject.name == "Detonator Player")
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                    rb.AddForce(Vector2.up * boostedJumpForce, ForceMode2D.Impulse);

                    otherPlayer.rb.linearVelocity = new Vector2(otherPlayer.rb.linearVelocity.x, 0);
                    otherPlayer.rb.AddForce(Vector2.up * boostedJumpForce, ForceMode2D.Impulse);
                }

                // Detonate explosion
                if (gameObject.name == "Detonator Player" && collision.gameObject.name == "TnT Box Player")
                {
                    if (explosionEffect != null) 
                        Instantiate(explosionEffect, transform.position, Quaternion.identity);

                    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
                    foreach (var hit in hitColliders)
                    {
                        if (hit.CompareTag("Enemy"))
                        {
                            Destroy(hit.gameObject);
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
