using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tails : MonoBehaviour
{

    private float speedV;
    private float speed;
    private SpriteRenderer sr;
    private Animator anim;
    private Rigidbody2D rb;

    public LayerMask groundLayer;
    public bool wallSliding;
    public bool facingRight;
    public Transform wallCheckpoint;
    public Transform wallCheckpointL;
    public bool wallCheck;
    public LayerMask stickyLayer;

    // Start is called before the first frame update
    void Start()
    {
        this.speedV = 7f;
        this.sr = GetComponent<SpriteRenderer>();
        this.anim = GetComponent<Animator>();
        this.rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        detectMovement();
        flipCharacter();
        detectJump();
        jumpingAnim();
        idk();
    }

    private void detectMovement()
    {
        float h = Input.GetAxis("Horizontal");
        this.speed = h * this.speedV * Time.deltaTime;
        transform.Translate(new Vector3(speed, 0, 0));
        if (isOnGround() == true)
        {
            this.anim.SetFloat("speed", Mathf.Abs(speed));
        }
    }

    private void flipCharacter()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.sr.flipX = false;
            this.facingRight = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.sr.flipX = true;
            this.facingRight = false;
        }
    }

    public bool isOnGround()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1f;

        Debug.DrawRay(position, direction, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    private void detectJump()
    {
        if (((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) && isOnGround() == true) && !wallSliding)
        {
            this.rb.velocity = Vector2.up * 6;
        }
    }

    private void jumpingAnim()
    {
        if (isOnGround() == true)
        {
            this.anim.SetBool("jumping", false);
        }
        else if (isOnGround() == false)
        {
            this.anim.SetBool("jumping", true);
        }
    }

    private bool wallChecking()
    {
        if (Physics2D.OverlapCircle(wallCheckpoint.position, 0.1f, stickyLayer) || Physics2D.OverlapCircle(wallCheckpointL.position, 0.1f, stickyLayer))
        {
            return true;
        }
        return false;
    }

    private void idk()
    {
        if (isOnGround() == false)
        {
            this.wallCheck = (Physics2D.OverlapCircle(wallCheckpoint.position, 0.1f, stickyLayer));
            if (facingRight && Input.GetAxis("Horizontal") > 0.1f || !facingRight && Input.GetAxis("Horizontal") < 0.1f)
            {
                if (wallChecking())
                {
                    handleWallSliding();
                    print("This happened");
                }
            }
        }

        if (!wallChecking() || isOnGround() == true)
        {
            wallSliding = false;
        }
    }

    private void handleWallSliding()
    {
        this.rb.velocity = new Vector2(this.rb.velocity.x, -0.7f);

        wallSliding = true;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("This also happened");
            if (facingRight)
            {
                print("Add force here!");
                this.transform.Translate(-0.5f, 1.2f, 0, Space.World);
                //this.rb.AddForce(new Vector2(-5, 20) * Vector2.up, ForceMode2D.Impulse);
            }
            else
            {
                this.transform.Translate(0.5f, 1.2f, 0, Space.World);
                //this.rb.AddForce(new Vector2(5, 20) * 20, ForceMode2D.Impulse);
            }
        }
    }


}
