using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopPhysics : MonoBehaviour
{
    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        loopCalculus(GameObject.FindGameObjectWithTag("player").GetComponent<Sonic>().speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.D))
        {
            Destroy(this.gameObject.GetComponent<LoopPhysics>(), 0.1f);
        }
    }

    void loopCalculus(float speed)
    {
        float angle, angSpeed, time;


    }
}
