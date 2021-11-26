using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum gameStatus
{
    next,play,gameover,win
};

public class Manager : Loader<Manager>
{
    [SerializeField]
    int totalWaves = 10;
    [SerializeField]
    Text totalMoneyLabel;
    [SerializeField]
    Text currentWave;
    [SerializeField]
    Text playBtnLabel;
    [SerializeField]
    Text totalEscapedLabel;
    [SerializeField]
    Button playBtn;
    [SerializeField]
    GameObject spawnPoint;
    [SerializeField]
    Enemy[] enemies;
    //[SerializeField]
    //int maxEnemiesOnScreen;
    [SerializeField]
    int totalEnemies = 5;
    [SerializeField]
    int enemiesPerSpawn;


    int waveBumber = 0;
    int totalMoney = 20;
    int totalEscaped = 0;
    int roundEscaped=0;
    int totalKill = 0;
    int whichEnemiesToSpawn=0;
    int enemiesToSpawn = 0;
    gameStatus currentState = gameStatus.play;
    AudioSource audioSource;

    public List<Enemy> EnemyList = new List<Enemy>();

   

    const float spawnDelay = 0.5f;

    public int TotalEscaped
    {
        get
        {
            return totalEscaped;
        }
        set
        {
            totalEscaped = value;
        }
    }

    public int RoundEscaped
    {
        get
        {
            return roundEscaped;
        }
        set
        {
            roundEscaped = value;
        }
    }

    public int TotalKilled
    {
        get
        {
            return totalKill;
        }
        set
        {
            totalKill = value;
        }
    }

    public int TotalMoney
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
            totalMoneyLabel.text = TotalMoney.ToString();
        }
    }

    public AudioSource AudioSource
    {
        get
        {
            return audioSource;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Spawn());
        playBtn.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        ShowMenu();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleEscaped();
    }

    IEnumerator Spawn()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for(int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < totalEnemies)
                {
                    Enemy newEnemy = Instantiate(enemies[Random.Range(0,enemiesToSpawn)]) as Enemy;
                    newEnemy.transform.position = spawnPoint.transform.position;
                    
                }
            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }
    public void UnregisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }
    public void DestroyEnemy()
    {
        foreach(Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }
    
   public void addMoney(int amount)
    {
        TotalMoney += amount;
    }
    public void subtractMoney(int amount)
    {
        TotalMoney -= amount;
    }

    public void IsWaveOver()
    {
        totalEscapedLabel.text = "Escaped" + TotalEscaped + "/5";
        if ((RoundEscaped+TotalKilled)==totalEnemies)
        {
            if (waveBumber<=enemies.Length)
            {
                enemiesToSpawn = waveBumber;
            }
            SetCurrentGameState();
            ShowMenu();
        }
    }
    public void SetCurrentGameState()
    {
        if (totalEscaped>=5)
        {
            currentState = gameStatus.gameover;
        }
        else if (waveBumber==0 && (RoundEscaped+TotalKilled)==0)
        {
            currentState = gameStatus.play;
        }
        else if (waveBumber>=totalWaves)
        {
            currentState = gameStatus.win;
        }
        else
        {
            currentState = gameStatus.next;
        }
    }

    public void PlayButtonPressed()
    {
        switch (currentState)
        {
            case gameStatus.next:
                waveBumber += 1;
                totalEnemies += waveBumber;
                break;

            default:
                totalEnemies = 1;
                TotalEscaped = 0;
                TotalMoney = 20;
                enemiesToSpawn = 0;
                TowerManager.Instance.DestroyAllTowers();
                TowerManager.Instance.RenameTagBuildSite();
                totalMoneyLabel.text = TotalMoney.ToString();
                totalEscapedLabel.text = "Escaped" + TotalEscaped + "/5";
                audioSource.PlayOneShot(SoundManager.Instance.NewGame);
                break;
        }
        DestroyEnemy();
        TotalKilled = 0;
        RoundEscaped = 0;
        currentWave.text = "Wave" + (waveBumber + 1);
        StartCoroutine(Spawn());
        playBtn.gameObject.SetActive(false);
    }

    public void ShowMenu()
    {
        switch (currentState)
        {
            case gameStatus.gameover:
                playBtnLabel.text = "Play again!";
                AudioSource.PlayOneShot(SoundManager.Instance.GameOver);
                break;
            case gameStatus.next:
                playBtnLabel.text = "Next wave";

                break;
            case gameStatus.play:
                playBtnLabel.text = "Play game";

                break;
            case gameStatus.win:
                playBtnLabel.text = "Next Level";
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
        }
        playBtn.gameObject.SetActive(true);
    }
    private void HandleEscaped()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.Instance.DisableDrag();
            TowerManager.Instance.towerBtnPressed = null;
        }
    }
}
