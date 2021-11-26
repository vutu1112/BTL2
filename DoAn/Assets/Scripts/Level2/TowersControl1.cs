using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowersControl1 : MonoBehaviour
{
    [SerializeField]
    float timeBetweenAttacks;
    [SerializeField]
    float attackRadius;
    [SerializeField]
    Projectiles1 projecttile;
    Enemy1 targetEnemy = null;
    float attackCounter;
    bool isAttacking = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackCounter -= Time.deltaTime;
        if (targetEnemy == null || targetEnemy.IsDead)
        {
            Enemy1 nearestEnemy = GetNearesEnemy();
            if (nearestEnemy != null && Vector2.Distance(transform.localPosition, nearestEnemy.transform.localPosition) <= attackRadius)
            {
                targetEnemy = nearestEnemy;
            }

        }
        else
        {
            if (attackCounter <= 0)
            {
                isAttacking = true;

                attackCounter = timeBetweenAttacks;
            }
            else
            {
                isAttacking = false;
            }
            if (Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRadius)
            {
                targetEnemy = null;
            }
        }
        
    }
    public void FixedUpdate()
    {
        if (isAttacking == true)
        {
            Attack();
        }
    }
    public void Attack()
    {
        isAttacking = false;
        Projectiles1 newProjecttile1 = Instantiate(projecttile) as Projectiles1;
        newProjecttile1.transform.localPosition = transform.localPosition;
        //if (newProjecttile1.PType == projecttiletype.arrow)
        //{
        //    Manager1.intance1.AudioSource1.PlayOneShot(SoundManager1.Instance1.Arrow1);
        //}
        //else if (newProjecttile1.PType == projecttiletype.fireball)
        //{
        //    Manager1.intance1.AudioSource1.PlayOneShot(SoundManager1.Instance1.FireBall1);
        //}
        //else if (newProjecttile1.PType == projecttiletype.rock)
        //{
        //    Manager1.intance1.AudioSource1.PlayOneShot(SoundManager1.Instance1.Rock1);
        //}
        if (targetEnemy == null)
        {
            Destroy(newProjecttile1);
        }
        else
        {
            StartCoroutine(MoveProjecttile(newProjecttile1));
        }
    }
    IEnumerator MoveProjecttile(Projectiles1 projecttile)
    {
        while (GetTargetDistance(targetEnemy) > 0.20f && projecttile != null && targetEnemy != null)
        {
            var dir = targetEnemy.transform.localPosition - transform.localPosition;
            var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            projecttile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
            projecttile.transform.localPosition = Vector2.MoveTowards(projecttile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);
            yield return null;
        }
        if (projecttile != null || targetEnemy != null)
        {
            Destroy(projecttile);
        }
    }
    private float GetTargetDistance(Enemy1 thisEnemy)
    {
        if (thisEnemy == null)
        {
            thisEnemy = GetNearesEnemy();
            if (thisEnemy == null)
            {
                return 0f;
            }
        }
        return Mathf.Abs(Vector2.Distance(transform.localPosition, thisEnemy.transform.localPosition));
    }
    private List<Enemy1> GetEnemiesInRange()
    {
        List<Enemy1> enemiesInRange = new List<Enemy1>();
        foreach (Enemy1 enemy in Manager1.intance1.EnemyList)
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRadius)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }
    private Enemy1 GetNearesEnemy()
    {
        Enemy1 nearesEnemy = null;
        float smallestDistance = float.PositiveInfinity;
        foreach (Enemy1 enemy in GetEnemiesInRange())
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
                nearesEnemy = enemy;
            }
        }
        return nearesEnemy;
    }
}
