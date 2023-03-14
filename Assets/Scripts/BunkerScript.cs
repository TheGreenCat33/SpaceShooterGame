using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunkerScript : MonoBehaviour
{

    public int health;
    public Vector3 startPos;
    public Vector3 hidePos;
    public GameObject expPreFab;
    public GameObject exppartPreFab;

    // Start is called before the first frame update
    void Start()
    {
        health = 10;
        startPos = transform.position;
        hidePos = startPos + Vector3.up * 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "bullets")
        {
            Destroy(collision.gameObject);
        }
        if (collision.tag == "enemy bullets")
        {
            Destroy(collision.gameObject);
        }

        if (health >= 1)
        {
            health--;
            transform.position = startPos;
        }
        else
        {
            GameObject exp = Instantiate(expPreFab);
            exp.transform.position = transform.position;
            GameObject exp_p = Instantiate(exppartPreFab);
            exp_p.transform.position = transform.position;
            Destroy(exp, 1f);
            Destroy(exp_p, 1f);
            transform.position = hidePos;
        }
    }
}
