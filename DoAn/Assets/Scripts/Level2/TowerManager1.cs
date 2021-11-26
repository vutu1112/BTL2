using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager1 : Loader1<TowerManager1>
{
    public TowerBtn1 towerBtnPressed { get; set; }
    SpriteRenderer SpriteRenderer;

    private List<TowersControl1> TowerList = new List<TowersControl1>();
    private List<Collider2D> BuildList = new List<Collider2D>();
    private Collider2D buildTile;
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        buildTile = GetComponent<Collider2D>();
        SpriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);
            
            PlaceTower(hit);
            
            
            
        }
        if (SpriteRenderer.enabled)
        {
            FollowMouse();
        }

    }
    public void FollowMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }
    public void EnableDrag(Sprite sprite)
    {
        SpriteRenderer.enabled = true;
        SpriteRenderer.sprite = sprite;
    }
    public void DisableDrag()
    {
        SpriteRenderer.enabled = false;
    }
    public void PlaceTower(RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null)
        {
            TowersControl1 newTower = Instantiate(towerBtnPressed.TowerObject);
            newTower.transform.position = hit.transform.position;
            ByTower(towerBtnPressed.TowerPrice);
            //Manager1.intance1.AudioSource1.PlayOneShot(SoundManager1.Instance1.TowerBuilt1);
            RegisterTower(newTower);
            DisableDrag();
        }
        
    }
    public void RenameTagBuildSite()
    {
        foreach (Collider2D buildTag in BuildList)
        {
            buildTag.tag = "TowerSide";
        }
        BuildList.Clear();
    }
    public void RegisterTower(TowersControl1 tower)
    {
        TowerList.Add(tower);
    }

    public void RegisterBuildSide(Collider2D buildTag)
    {
        BuildList.Add(buildTag);
    }
    public void DestroyAllTowers()
    {
        foreach (TowersControl1 tower in TowerList)
        {
            Destroy(tower.gameObject);
        }
        TowerList.Clear();
    }
    public void ByTower(int price)
    {
        Manager1.intance1.subtractMoney(price);
    }
    public void SelectedTower(TowerBtn1 towerSelected)
    {

        if (towerSelected.TowerPrice <= Manager1.intance1.TotalMoney)
        {
            towerBtnPressed = towerSelected;
            EnableDrag(towerBtnPressed.DragSprite);
        }


    }
}
