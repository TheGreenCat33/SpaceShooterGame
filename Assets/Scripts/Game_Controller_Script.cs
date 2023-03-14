using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Game_Controller_Script : MonoBehaviour
{
    Enemy_Script[] enemies;
    Enemy_Script randomEnemy;

    // shooting variables
    public float shootInt = 3f;
    public float shootSpeed = 2f;
    public GameObject badBulletPrefab;
    private float shootTimer;

    //movement
    public float moveInt;
    public float moveDist = 0.1f;
    public float hLimit = 4;
    private float moveDirect = 1;
    private float moveTimer;
    private float rightMostEnemyPos = 0;
    private float leftMostEnemyPos = 0;

    //difficulty
    public float maxMoveInterval = 0.42857639f;
    public float minMoveInterval = 0.05f;
    private int maxEnemies;

    //ui
    public int score;
    private int lives;
    public int wave;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI waveText;


    // Start is called before the first frame update
    void Start()
    {
        //set score/wave/lives from previous level or 0 if first time
        if (PlayerPrefs.HasKey("score"))
        {
            score = PlayerPrefs.GetInt("score", score);
            wave = PlayerPrefs.GetInt("wave", wave);
            lives = PlayerPrefs.GetInt("lives", lives);
        }
        else
        {
            score = 0;
            wave = 1;
            lives = 3;
        }

        update_Ui();
        moveTimer = maxMoveInterval;
        enemies = GetComponentsInChildren<Enemy_Script>();
        shootTimer = shootInt;
        maxEnemies = enemies.Length;
    }

    public void player_die()
    {
        lives--;
        update_Ui();
        if (lives < 1)
        {
            clear_data(); 
            SceneManager.LoadScene("GameOver");
        }
    }

    public void addScore(int points)
    {
        score += points;
        update_Ui();
    }

    private void update_Ui()
    {
        scoreText.text = "Score\n" + score.ToString();
        waveText.text = "Wave\n" + wave.ToString();
        livesText.text = "Lives\n" + lives.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GetComponentsInChildren<Enemy_Script>();
        if(enemies.Length >= 1)
        {
            //move code
            moveTimer -= Time.deltaTime;
            if (moveTimer <= 0)
            {
                int currentEnemies = enemies.Length;
                float difSetting = 1f - (float)currentEnemies / maxEnemies;
                moveInt = maxMoveInterval - (maxMoveInterval - minMoveInterval) * difSetting;
                //side to side
                moveTimer = moveInt;
                transform.position = new Vector3(transform.position.x + moveDist * moveDirect, transform.position.y, 0);
            }
            if (moveDirect == 1)
            {
                foreach (Enemy_Script enemy in enemies)
                {
                    if (enemy.transform.position.x > rightMostEnemyPos)
                    {
                        rightMostEnemyPos = enemy.transform.position.x;
                    }
                }
                if (rightMostEnemyPos >= hLimit)
                {
                    moveDirect *= -1;
                    transform.position = new Vector3(transform.position.x, transform.position.y - moveDist, transform.position.z);
                    rightMostEnemyPos = 0;
                }
            }
            else
            {
                foreach (Enemy_Script enemy in enemies)
                {
                    if (enemy.transform.position.x < leftMostEnemyPos)
                    {
                        leftMostEnemyPos = enemy.transform.position.x;
                    }
                }
                if (leftMostEnemyPos < -hLimit)
                {
                    moveDirect *= -1;
                    transform.position = new Vector3(transform.position.x, transform.position.y - moveDist, transform.position.z);
                    leftMostEnemyPos = 0;
                }
            }

            //pew pew code
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0 && enemies.Length > 0)
            {
                shootInt = Random.Range(2, 6);
                shootTimer = shootInt;
                shootSpeed = Random.Range(2, 8);
                randomEnemy = enemies[Random.Range(0, enemies.Length)];
                GameObject enemyBullet = Instantiate(badBulletPrefab);
                enemyBullet.transform.position = randomEnemy.transform.position;
                enemyBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -shootSpeed);
                Destroy(enemyBullet, 6f);
            }
        }
        else
        {
            wave++;
            save_data();
            SceneManager.LoadScene("game"); 
        }
    }

    public void save_data()
    {
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.SetInt("wave", wave);
        PlayerPrefs.SetInt("lives", lives);
    }

    public void clear_data()
    {
        PlayerPrefs.DeleteKey("score");
        PlayerPrefs.DeleteKey("wave");
        PlayerPrefs.DeleteKey("lives");
    }
}
