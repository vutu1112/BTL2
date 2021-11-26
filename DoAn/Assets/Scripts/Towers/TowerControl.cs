using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControl : MonoBehaviour
{
    [SerializeField]
    float timeBetweenAttacks;
    [SerializeField]
    float attackRadius;
    [SerializeField]
    Projectile projecttile;
    Enemy targetEnemy = null;
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

        if (targetEnemy==null || targetEnemy.IsDead)
        {
            Enemy nearestEnemy = GetNearesEnemy();
            if (nearestEnemy != null && Vector2.Distance(transform.localPosition, nearestEnemy.transform.localPosition) <=attackRadius)
            {
                targetEnemy = nearestEnemy;
            }
        }
        else
        {
            if (attackCounter<=0)
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
        Projectile newProjecttile = Instantiate(projecttile) as Projectile;
        newProjecttile.transform.localPosition = transform.localPosition;
        if (newProjecttile.PType == projecttiletype.arrow)
        {
            Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Arrow);
        }
        else if(newProjecttile.PType == projecttiletype.fireball)
        {
            Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.FireBall);
        }
        else if(newProjecttile.PType == projecttiletype.rock)
        {
            Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Rock);
        }
        if (targetEnemy==null)
        {
            Destroy(newProjecttile);
        }
        else
        {
           StartCoroutine(MoveProjecttile(newProjecttile));
        }
    }

    IEnumerator MoveProjecttile(Projectile projecttile)
    {
        while(GetTargetDistance(targetEnemy)>0.20f && projecttile!=null && targetEnemy != null)
        {
            var dir = targetEnemy.transform.localPosition - transform.localPosition;
            var angleDirection = Mathf.Atan2(dir.y, dir.z) * Mathf.Rad2Deg;
            projecttile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
            projecttile.transform.localPosition = Vector2.MoveTowards(projecttile.transform.localPosition, targetEnemy.transform.localPosition,5f*Time.deltaTime);
            yield return null;
        }
        if (projecttile!=null || targetEnemy!=null)
        {
            Destroy(projecttile);
        }
    }

    private float GetTargetDistance(Enemy thisEnemy)
    {
        if (thisEnemy==null)
        {
            thisEnemy = GetNearesEnemy();
            if (thisEnemy==null)
            {
                return 0f;
            }
        }
        return Mathf.Abs(Vector2.Distance(transform.localPosition, thisEnemy.transform.localPosition));
    }

    private List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();
        foreach (Enemy enemy in Manager.Instance.EnemyList)
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <=attackRadius)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }
    private Enemy GetNearesEnemy()
    {
        Enemy nearesEnemy = null;
        float smallestDistance = float.PositiveInfinity;
        foreach (Enemy enemy in GetEnemiesInRange())
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
