using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBtn1 : MonoBehaviour
{
    [SerializeField]
    TowersControl1 towerObject;
    [SerializeField]
    Sprite dragSprite;
    [SerializeField]
    int towerPrice;
    // Start is called before the first frame update

    public TowersControl1 TowerObject
    {
        get
        {


            return towerObject;
        }
    }
    public Sprite DragSprite
    {
        get
        {


            return dragSprite;
        }
    }
    public int TowerPrice
    {
        get
        {
            return towerPrice;
        }
    }
}
