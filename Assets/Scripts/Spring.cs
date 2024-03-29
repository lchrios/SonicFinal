﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{

    public Sonic sonic;
    public Transform referencia;
    public AudioClip springSound;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<AudioSource>().clip = this.springSound;
        this.sonic = GameObject.FindGameObjectWithTag("Player").GetComponent<Sonic>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (sonic.minY.transform.position.y > referencia.transform.position.y)
        {
            if (collision.transform.name.Equals("Sonic"))
            {
                collision.transform.GetComponent<Rigidbody2D>().AddForce(25 * transform.up, ForceMode2D.Impulse);
                this.GetComponent<AudioSource>().Play();
                sonic.isGrounded = false;
                
            }
        }
    }
}
