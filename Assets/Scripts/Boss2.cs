using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss2 : MonoBehaviour
{
    public BossStats stats;
    public GameObject player,
                      bullet;
    public bool attacking,
                canShoot,
                canExplode;
    public int shotsLeft;
    // Start is called before the first frame update
    void Start()
    {
        this.stats = new BossStats(5, 2.0f, 3.0f, 1.5f, 0.7f, false);
        this.attacking = true;
        this.canShoot = true;
        this.shotsLeft = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(8);
        }
        if (attacking)
        {
            if (canShoot)
            {
                shoot();
            }
            if (canExplode)
            {
                explode();
            }
        }

        
    }

    void shoot()
    {
        canShoot = false;
        Instantiate(bullet, this.transform, true);
        StartCoroutine(rechargeShot());
    }

    IEnumerator rechargeShot()
    {
        yield return new WaitForSeconds(this.stats.attackWait * Time.deltaTime);
        canShoot = true;
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

    void damage()
    {
        this.stats.health--;
        if (this.stats.health == 0)
        {
            SceneManager.LoadScene(8);
        }
        this.canExplode = true;
        this.canShoot = true;
        this.shotsLeft = 20;
        this.attacking = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            damage();
        }
    }
}
