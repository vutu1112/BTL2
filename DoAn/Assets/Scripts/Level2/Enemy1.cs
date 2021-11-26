using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public int target = 0;
    public Transform exit;
    public Transform[] waypoints;
    public float navigation;
    Transform enemy;
    [SerializeField]
    int health;
    [SerializeField]
    int rewardAmount;
    bool isDead = false;
    Collider2D enemyCollider;
    float navigationTime = 0;
    Animator anim;
    public bool IsDead
    {
        get
        {

            return isDead;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent <Transform>();
        enemyCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        Manager1.intance1.RegisterEnemy(this);

    }

    // Update is called once per frame
    void Update()
    {
        if (waypoints != null && isDead==false)
        {
            navigationTime += Time.deltaTime;
            if (navigationTime > navigation)
            {
                if (target < waypoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, waypoints[target].position, navigationTime);

                }
                else
                {
                    enemy.position= Vector2.MoveTowards(enemy.position, exit.position, navigationTime);
                }
                navigationTime = 0;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "checkpoint")
        {
            target += 1;
        }
        else if (collision.tag == "Finish")
        {
            Manager1.intance1.RoundEscaped += 1;
            Manager1.intance1.TotalEscaped += 1;
            Manager1.intance1.UnregisterEnemy(this);
            Manager1.intance1.IsWaveOver();
            //Destroy(gameObject);
        }
        else if (collision.tag == "Projectile")
        {
            Destroy(collision.gameObject);
            Projectiles1 newP = collision.gameObject.GetComponent<Projectiles1>();

            EnemyHit(newP.AttackDamage);
        }
    }
    public void EnemyHit(int hitpoints)
    {

        if (health - hitpoints > 0)
        {
            health -= hitpoints;
            //Manager1.intance1.AudioSource1.PlayOneShot(SoundManager1.Instance1.Hit1);
            anim.Play("Hurt");
        }
        else
        {
            anim.SetTrigger("didDie");
            Die();
        }
    }
    public void Die()
    {
        isDead = true;
        enemyCollider.enabled = false;
        Manager1.intance1.TotalKilled += 1;
        //Manager1.intance1.AudioSource1.PlayOneShot(SoundManager1.Instance1.Death1);
        Manager1.intance1.addMoney(rewardAmount);
        Manager1.intance1.IsWaveOver();
    }
}
