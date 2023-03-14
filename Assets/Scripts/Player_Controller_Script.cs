using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller_Script : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletPartPreFab;
    public bool fired;
    public float bulletSpeed;
    public float moveSpeed;
    public Rigidbody2D rig;
    public float limit;

    private float cooldown;
    private float cooldownDuration;

    private Vector3 startPos;
    private Vector3 hidePos;
    public GameObject expPreFab;
    public GameObject exppartPreFab;
    public float hidetime;
    public float hideDur;
    private bool dead;

    public float rapidTime;
    public float rapidDur;
    public Game_Controller_Script controller;
    public bool rapid;
    public float rapidStart;
    public float rapidRandom;




    // Start is called before the first frame update
    void Start()
    {
        dead = false;
        hideDur = 3f;
        hidetime = hideDur;

        rapidDur = 5f;
        rapidTime = rapidDur;
        rapidRandom = Random.Range(30, 60);
        rapidStart = rapidRandom;

        startPos = transform.position;
        hidePos = startPos + Vector3.up * 1000;

        fired = false;
        rig = GetComponent<Rigidbody2D>();
        cooldownDuration = 1f;
        controller = FindObjectOfType<Game_Controller_Script>();
        rapid = false;
    }

    private void FixedUpdate()
    {
        rig.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, 0);

        if (transform.position.x > limit)
        {
            transform.position = new Vector3(limit, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -limit)
        {
            transform.position = new Vector3(-limit, transform.position.y, transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        rapidStart -= Time.deltaTime;
        if (rapidStart <= 0)
        {
            rapidStart = rapidRandom;
            rapid = true;
        }

        if (dead)
        {
            hidetime -= Time.deltaTime;
            if(hidetime <= 0)
            {
                dead = false;
                hidetime = hideDur;
                transform.position = startPos;
            }
        }

        cooldown -= Time.deltaTime;
        if(Input.GetAxis("Jump")== 1f)
        {
            if(cooldown <= 0 && fired == false)
            {
                if(rapid == true)
                {
                    rapidTime -= Time.deltaTime;

                    GameObject bulletR = Instantiate(bulletPrefab);
                    bulletR.transform.SetParent(transform.parent);
                    bulletR.transform.position = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
                    bulletR.GetComponent<Rigidbody2D>().velocity = new Vector2(
                        0,
                        bulletSpeed
                        );

                    Destroy(bulletR, 2f);
                    if(rapidTime <= 0)
                    {
                        rapid = false;
                        rapidTime = rapidDur;
                    }
                }
                else
                {
                    cooldown = cooldownDuration;
                    fired = true;
                    GameObject bullet = Instantiate(bulletPrefab);
                    bullet.transform.SetParent(transform.parent);
                    bullet.transform.position = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
                    bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(
                        0,
                        bulletSpeed
                        );

                    Destroy(bullet, 2f);
                }  
            }
        }
        else
        {
            fired = false;
        }

    }

    private void die()
    {
        GameObject exp = Instantiate(expPreFab);
        exp.transform.position = transform.position;
        GameObject exp_p = Instantiate(exppartPreFab);
        exp_p.transform.position = transform.position;

        dead = true;
        transform.position = hidePos;

        Destroy(exp, 1f);
        Destroy(exp_p, 1f);

        GameObject.FindGameObjectWithTag("controller").GetComponent<Game_Controller_Script>().player_die();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        die();
    }
}
