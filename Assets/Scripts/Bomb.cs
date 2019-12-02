using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Destroy(gameObject, 15.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void explode()
    {
        anim.SetBool("touch", true);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (Vector2.Distance(transform.position, player.transform.position) <= 4.5f)
        {
            player.GetComponent<Sonic>().damage();
        }
        Destroy(gameObject, 0.5f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        explode();
    }
}
