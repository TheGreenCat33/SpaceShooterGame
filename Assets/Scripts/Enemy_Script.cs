using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Script : MonoBehaviour
{
    public GameObject expPreFab;
    public GameObject exppartPreFab;
    public int points;


    private void die()
    {
        //add points
        GetComponentInParent<Game_Controller_Script>().addScore(points);
        //boom boom
        GameObject exp = Instantiate(expPreFab);
        exp.transform.position = transform.position;
        GameObject exp_p = Instantiate(exppartPreFab);
        exp_p.transform.position = transform.position;
        Destroy(exp, 1f);
        Destroy(exp_p, 1f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "bullets")
        { 
            Destroy(collision.gameObject);
            die();
        }
    }
}
