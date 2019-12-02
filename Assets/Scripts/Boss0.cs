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
        this.attacking = true;
        this.canDrop = true;
        this.maxBombsPerShift = 9;
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

    public IEnumerator damage()
    {
        this.stats.health--;
        Debug.Log(this.stats.health);
        if(this.stats.health == 0)
        {
            Debug.Log("muriendo");
            Destroy(gameObject, this.stats.stunTime);
            yield return new WaitForSeconds(this.stats.stunTime);
            Instantiate(nextStage, this.transform);
        } else
        {
            attacking = true;
            maxBombsPerShift = 9;
            //StartCoroutine(moveToPlayer());
        }
    }

    void moveToPlayer()
    {
        float dist = player.transform.position.x - transform.position.x;
        //Debug.Log(dist);
        if (Mathf.Abs(dist) <= 0.5f && attacking)
        {
            Debug.Log("YIPIYYA");
            transform.Translate(this.stats.movSpeed * Time.deltaTime, 0, 0);
        }
    }
}
