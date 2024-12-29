using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    
    [Header("In Game")]
    public GameObject gameOverPanel;
    public GameObject gameClearPanel;
    public GameObject[] inGameObejcts;
    public GameObject[] birdPrefab;
    public bool canShot = false;
    public bool selectBird = true;
    public int birdCount;
    public int monsterCount;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveStage(int stageNumber)
    {
        SceneManager.LoadScene(stageNumber);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoHome()
    {
        SceneManager.LoadScene(0);
    }
    
    

    public void CheckCount()
    {
        if (monsterCount > 0 && birdCount > 0)
            return;
        
        HideObject();
        if (monsterCount == 0)
        {
            gameClearPanel.SetActive(true);
        }
        else
        {
            gameOverPanel.SetActive(true);
        }
    }

    void HideObject()
    {
        for (int i = 0; i < inGameObejcts.Length; i++)
        {
            inGameObejcts[i].SetActive(false);
        }
    }

}
