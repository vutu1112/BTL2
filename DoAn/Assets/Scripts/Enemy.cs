using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    Transform exit;
    [SerializeField]
    Transform[] wayPoints;
    [SerializeField]
    float navigation;
    [SerializeField]
    int health;
    [SerializeField]
    int rewardAmount;

    int target = 0;
    Transform enemy;
    Collider2D enemyCollider;
    Animator anim;
    float navigationTime = 0;
    bool isDead = false;

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
        enemy = GetComponent<Transform>();
        enemyCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        Manager.Instance.RegisterEnemy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (wayPoints != null && isDead==false)
        {
            navigationTime += Time.deltaTime;
            if (navigationTime>navigation)
            {
                if (target<wayPoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, wayPoints[target].position, navigationTime);
                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exit.position, navigationTime);
                }
                navigationTime = 0;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "checkpoint")
        {
            Debug.Log("Vao");
            target += 1;
        }
        else if (collision.tag == "Finish")
        {
            Manager.Instance.RoundEscaped += 1;
            Manager.Instance.TotalEscaped += 1;
            Manager.Instance.UnregisterEnemy(this);
            Manager.Instance.IsWaveOver();
        }
        else if (collision.tag == "Projectile")
        {
            Destroy(collision.gameObject);
            Projectile newP = collision.gameObject.GetComponent<Projectile>();
            EnemyHit(newP.AttackDamage);
            
        }
    }

    public void EnemyHit(int hitpoints)
    {
        
        if (health - hitpoints > 0)
        {
            health -= hitpoints;
            Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit);
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
        Manager.Instance.TotalKilled += 1;
        Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Death);
        Manager.Instance.addMoney(rewardAmount);
        Manager.Instance.IsWaveOver();
    }
}
