using UnityEngine;



public enum projecttiletype
{
    rock, arrow, fireball
};

public class Projectile : MonoBehaviour
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
