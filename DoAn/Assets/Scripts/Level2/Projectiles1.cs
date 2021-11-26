using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum projecttiletype1
{
    rock, arrow, fireball
};
public class Projectiles1 : MonoBehaviour
{
    [SerializeField]
    int attackDamage;
    [SerializeField]
    projecttiletype pType;

    public int AttackDamage
    {
        get
        {


            return attackDamage;
        }
    }

    public projecttiletype PType
    {
        get
        {


            return pType;
        }
    }
}
