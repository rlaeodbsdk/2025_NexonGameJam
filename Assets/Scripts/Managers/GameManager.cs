using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager
{
    public float extraDamage = 1;
    public int beltCount = 0;
    public int roundNumber = 1;
    public int stageNumber = 1;
    //게임 상태를 나눠서 상태에 따라 스크립트들이 돌아가게 함

    public float completeOrderCount;
    public float OrderCount;
    public float playerTotalMoney = 1000;
    public bool isTutorial;
    public float todaySelling=0;
    public float totalIngredientMoney = 0;

    public ClockAndMoney cMoney;
    
   
    public enum GameState
    {
        InGame,
        Option,
        Lobby

    }
    public GameState currentState;
    
    //플레이어 죽을 때 실행시킬 함수
    public void PlayerDied()
    {
       
    }
    //인게임 데이터 초기화 
    public void GameStart()
    {
        currentState = GameState.InGame;
        todaySelling = 0;
        completeOrderCount = 0;
        OrderCount = 0;
        totalIngredientMoney = 0;

        //타이머 재조정
        switch (stageNumber)
        {
            case 1: // 1일차 . 사실안씀
                {
                    cMoney.currentTime = 40f;
                    break;
                }
            case 2: // 2일차.
                {
                    cMoney.currentTime = 60f;
                    break;
                }
            case 3:
                {
                    cMoney.currentTime = 130f;
                    break;
                }
        }
}

    public void Upgrade()
    {
        Time.timeScale = 0;

    }

    public void onRoundOver()
    {
        
        Managers.UI.ShowPopUpUI<UI_Receipt>();
        
        Time.timeScale = 0;
    }

    public void openShop()
    {
        GameObject.FindFirstObjectByType<CustomShopManager>().openShop();
    }

    public void resetRound()
    {
        
    }

    void Start()
    {
        GameStart(); //임시로 매니저 켜질떄 GameStart 취급

    }

}
