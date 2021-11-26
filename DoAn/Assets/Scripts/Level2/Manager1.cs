using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Manager1 : MonoBehaviour
{
    public enum gameStatus
    {
        next, play, gameover, win
    };
    public static Manager1 intance1 = null;
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
    GameObject[] spawnPoint;
    [SerializeField]
    Enemy1[] enemies;
    [SerializeField]
    int totalEnemies=5;
    [SerializeField]
    int enemiesPerSpawn;
    [SerializeField]
    //int enemiesOnScreen = 0;
    public List<Enemy1> EnemyList = new List<Enemy1>();
    const float spawnDelay = 0.5f;
    const float spawnDelay1 = 1f;
    int waveNumber = 0;
    int totalMoney = 20;
    int totalEscaped = 0;
    int roundEscaped = 0;
    int totalKill = 0;
    int whichEnemiesToSpawn = 0;
    int enemiesToSpawn = 0;
    gameStatus currentState = gameStatus.play;
    AudioSource audioSource1;
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
    public AudioSource AudioSource1
    {
        get
        {
            return audioSource1;
        }
    }
    void Awake()
    {
        if (intance1 == null)
        {
            intance1 = this;
        }
        else if (intance1!=this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        playBtn.gameObject.SetActive(false);
        ShowMenu();
    }

    // Update is called once per frame
    void Update()
    {
        HandleEscaped();
    }

    IEnumerator Spawn()
    {
        if(enemiesPerSpawn>0 && EnemyList.Count < totalEnemies)
        {
            for(int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < totalEnemies)
                {
                    Enemy1 newEnemy = Instantiate(enemies[0]) as Enemy1;
                    newEnemy.transform.position = spawnPoint[0].transform.position;
                    //enemiesOnScreen += 1;
                }
                
            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }
    IEnumerator Spawn1()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < totalEnemies)
                {

                    Enemy1 newEnemy1 = Instantiate(enemies[1]) as Enemy1;
                    newEnemy1.transform.position = spawnPoint[1].transform.position;
                    //enemiesOnScreen += 1;
                }

            }
            yield return new WaitForSeconds(spawnDelay1);
            StartCoroutine(Spawn1());
        }
    }
    public void RegisterEnemy(Enemy1 enemy)
    {
        EnemyList.Add(enemy);
    }
    public void UnregisterEnemy(Enemy1 enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }
    public void DestroyEnemy()
    {
        foreach (Enemy1 enemy in EnemyList)
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
        if ((RoundEscaped + TotalKilled) == totalEnemies)
        {
            if (waveNumber <= enemies.Length)
            {
                enemiesToSpawn = waveNumber;
            }
            SetCurrentGameState();
            ShowMenu();
        }
    }
    public void SetCurrentGameState()
    {
        if (totalEscaped >= 5)
        {
            currentState = gameStatus.gameover;
        }
        else if (waveNumber == 0 && (RoundEscaped + TotalKilled) == 0)
        {
            currentState = gameStatus.play;
        }
        else if (waveNumber >= totalWaves)
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
                waveNumber += 1;
                totalEnemies += waveNumber;
                break;

            default:
                totalEnemies = 5;
                TotalEscaped = 0;
                TotalMoney = 20;
                enemiesToSpawn = 0;
                TowerManager1.Instance1.DestroyAllTowers();
                TowerManager1.Instance1.RenameTagBuildSite();
                totalMoneyLabel.text = TotalMoney.ToString();
                totalEscapedLabel.text = "Escaped" + TotalEscaped + "/5";
                //audioSource1.PlayOneShot(SoundManager1.Instance1.NewGame1);
                break;
        }
        DestroyEnemy();
        TotalKilled = 0;
        RoundEscaped = 0;
        currentWave.text = "Wave" + (waveNumber + 1);
        StartCoroutine(Spawn());
        StartCoroutine(Spawn1());
        playBtn.gameObject.SetActive(false);
    }
    public void ShowMenu()
    {
        switch (currentState)
        {
            case gameStatus.gameover:
                playBtnLabel.text = "Play again!";
                //AudioSource1.PlayOneShot(SoundManager1.Instance1.GameOver1);
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
            TowerManager1.Instance1.DisableDrag();
            TowerManager1.Instance1.towerBtnPressed = null;
        }
    }
    //public void removeEnemyFromScreen()
    //{
    //    if (enemiesOnScreen > 0)
    //    {
    //        enemiesOnScreen -= 1;
    //    }
    //}
}
