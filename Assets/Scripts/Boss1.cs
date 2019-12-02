using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    public BossStats stats;
    public Rigidbody2D rb;
    public Animator anim;
    public GameObject player,
                      nextStage,
                      spawnRef;
    public bool attacking,
                canExplode,
                canDash;
    public int dashesLeft;
    // Start is called before the first frame update
    void Start()
    {
        this.stats = new BossStats(6, 5.0f, 10.0f, 3.4f, 1.1f, true);
        this.canDash = true;
        this.rb = GetComponent<Rigidbody2D>();
        this.canExplode = true;
        this.canDash = true;
        this.dashesLeft = 4;
        this.anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("*se muere*");
            Instantiate(nextStage, spawnRef.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (attacking)
        {
            Vector2 heading = player.transform.position - this.transform.position;
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("*se muere*");
                Instantiate(nextStage, spawnRef.transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            /*if (canDash)
            {
                dash(heading);
            }

            if (canExplode)
            {
                explode();
            }*/
        }
        
    }

    void dash(Vector2 heading)
    {
        dashesLeft--;
        rb.AddForce(heading * 25);
        Vector2.MoveTowards(transform.position, player.transform.position, this.stats.movSpeed * Time.deltaTime);
        StartCoroutine(rechargeDash());
        if (dashesLeft == 0)
        {
            anim.SetBool("triggered", false);
            attacking = false;
        }
    }

    IEnumerator rechargeDash()
    {
        yield return new WaitForSeconds(this.stats.attackWait * Time.deltaTime);
        this.canDash = true;
    }

    void explode()
    {
        canExplode = false;
        if (Vector3.Distance(transform.position, player.transform.position) <= 4.5f)
        {
            player.GetComponent<Sonic>().damage();
        }
        StartCoroutine(rechargeExplosion());
    }

    IEnumerator rechargeExplosion()
    {
        yield return new WaitForSeconds(Random.Range(4.0f, 8.0f));
        this.canExplode = true;
    }

    IEnumerator recharge()
    {
        yield return new WaitForSeconds(this.stats.stunTime);
        this.canExplode = true;
        this.dashesLeft = 4;
        this.attacking = true;
        anim.SetBool("triggered", true);
    }

    // Su muerte
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !this.attacking)
        {
            damage();
        }
    }

    IEnumerator damage()
    {
        this.stats.health--;
        if (this.stats.health == 0)
        {
            Destroy(gameObject, this.stats.stunTime);
            yield return new WaitForSeconds(this.stats.stunTime);
            Instantiate(nextStage, this.transform);
        }
        else
        {
            StartCoroutine(recharge());
        }
    }
}
