using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager1 : Loader1<SoundManager1>
{
    [SerializeField]
    AudioClip arrow;
    [SerializeField]
    AudioClip death;
    [SerializeField]
    AudioClip fireBall;
    [SerializeField]
    AudioClip gameOver;
    [SerializeField]
    AudioClip hit;
    [SerializeField]
    AudioClip level;
    [SerializeField]
    AudioClip newGame;
    [SerializeField]
    AudioClip rock;
    [SerializeField]
    AudioClip towerBuilt;

    public AudioClip Arrow1
    {
        get
        {
            return arrow;
        }
    }
    public AudioClip Death1
    {
        get
        {
            return death;
        }
    }
    public AudioClip FireBall1
    {
        get
        {
            return fireBall;
        }
    }
    public AudioClip GameOver1
    {
        get
        {
            return gameOver;
        }
    }
    public AudioClip Hit1
    {
        get
        {
            return hit;
        }
    }
    public AudioClip Level1
    {
        get
        {
            return level;
        }
    }
    public AudioClip NewGame1
    {
        get
        {
            return newGame;
        }
    }
    public AudioClip Rock1
    {
        get
        {
            return rock;
        }
    }
    public AudioClip TowerBuilt1
    {
        get
        {
            return towerBuilt;
        }
    }
}
