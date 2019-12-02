using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss0 : MonoBehaviour
{
    public GameObject player,
                      nextStage,
                      bomb,
                      spawnRef;
    public BossStats stats;
    public bool dying, 
                attacking,
                canDrop;
    public int maxBombsPerShift;
    

    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player");
        this.stats = new BossStats(10, 0.0f, 10.0f, 1.2f, 2.5f, true);
        this.dying = false;
        this.attacking = false;
        this.canDrop = true;
        this.maxBombsPerShift = 9;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector2.Distance(this.transform.position, player.transform.position);
        moveToPlayer();
        // Modo ataque 
        if (attacking)
        {
            //Attacking
            if (canDrop)
            {
                dropBomb();
            }
        }

    }

    void dropBomb()
    {
        this.canDrop = false;
        Instantiate(bomb, spawnRef.transform.position, Quaternion.identity);
        StartCoroutine(recharge());
    }

    IEnumerator recharge()
    {
        yield return new WaitForSeconds(this.stats.attackWait);
        maxBombsPerShift--;
        if (maxBombsPerShift == 0)
        {
            this.attacking = false;
        }
        canDrop = true;
    }


    // Su muerte
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !this.attacking)
        {
            damage();
        }
    }

    void damage()
    {
        this.stats.health--;
        if(this.stats.health == 0)
        {
            this.dying = true;
            Destroy(gameObject, this.stats.stunTime);
        } else
        {
            attacking = true;
            maxBombsPerShift = 9;
            StartCoroutine(moveToPlayer());
        }
    }

    string moveToPlayer()
    {
        float dist = player.transform.position.x - transform.position.x;
        if (dist >= -0.5f && dist <= 0.5f && attacking)
        {
            transform.Translate(Vector3.right * this.stats.movSpeed * Time.deltaTime * (attacking ? 1.0f : 0.0f));
        }
        return "";
    }
}
