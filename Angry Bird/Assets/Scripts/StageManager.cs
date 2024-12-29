using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject slingShot;
    [SerializeField] private GameObject stageObjects;

    [SerializeField] private Handler handler;

    [SerializeField] private int monsterCount = 3;
    [SerializeField] private int redBirdCount = 2;
    [SerializeField] private int bombBirdCount = 1;
    
    public TextMeshProUGUI redBirdText;
    public TextMeshProUGUI bombBirdText;
    
    private GameManager gameManager;

    
    // Start is called before the first frame update
    void Start()
    {
        redBirdText.text = redBirdCount.ToString();
        bombBirdText.text = bombBirdCount.ToString();
        
        gameManager = GameManager.instance;
        gameManager.birdCount = redBirdCount + bombBirdCount;
        gameManager.monsterCount = monsterCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnClickbird(int index)
    {
        if (!gameManager.selectBird)
            return;
        
        switch (index)
        { 
            case 0:
                if (redBirdCount == 0)
                    break;
                redBirdCount--;
                redBirdText.text = redBirdCount.ToString();
                handler.OnSpawnBird(0);
                StartCoroutine(CanShot());
                Debug.Log("OnClickBird");
                
                break;
            
            case 1:
                if (bombBirdCount == 0)
                    break;
                bombBirdCount--;
                bombBirdText.text = bombBirdCount.ToString();
                handler.OnSpawnBird(1);
                StartCoroutine(CanShot());
                break;
        }
        
    }

    IEnumerator CanShot()
    {
        yield return new WaitForSeconds(0.5f);
        gameManager.canShot = true;
        gameManager.selectBird = false;
    }
}
