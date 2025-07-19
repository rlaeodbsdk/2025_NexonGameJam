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
    public float playerTotalMoney = 5000;
    public bool isTutorial;
    public float todaySelling=0;


    public float totalIngredientMoney = 0;

    public float ingredientDiscount = 0f;

    public float villainRate = 0f;

    public float addDaytime = 0f;



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

    }

    public void GoNextStage()
    {
        roundNumber++;
        switch (roundNumber)
        {
            case 2: Managers.Scene.LoadScene(Define.Scene.Stage2);
                break;
            case 3: Managers.Scene.LoadScene(Define.Scene.Stage3);
                break;
            case 4: Managers.Scene.LoadScene(Define.Scene.Stage4);
                break;
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

    public void ApplyIngredientDiscount(int level)
    {
        switch (level)
        {
            case 1:
                ingredientDiscount = 0.2f;
                break;
            case 2:
                ingredientDiscount = 0.4f;
                break;
            case 3:
                ingredientDiscount = 0.6f;
                break;
            default:
                ingredientDiscount = 0f;
                break;
        }
    }

    public int GetDiscountedIngredientPrice()
    {
        return Mathf.RoundToInt(totalIngredientMoney * (1f - ingredientDiscount));
    }

    public void ApplyVillainRate(int level)
    {
        switch (level)
        {
            case 1:
                villainRate = 0.08f;
                break;
            case 2:
                villainRate = 0.06f;
                break;
            case 3:
                villainRate = 0.04f;
                break;
            default:
                villainRate = 0.10f;
                break;
        }
    }

    public void ApplyDaytimeAddition(int level)
    {
        switch (level)
        {
            case 1:
                addDaytime = 5f;
                break;
            case 2:
                addDaytime = 10f;
                break;
            case 3:
                addDaytime = 15f;
                break;
            default:
                addDaytime = 0f;
                break;
        }
    }

    public float GetAddedDaytime(float originalDaytime)
    {
        return originalDaytime + addDaytime;
    }
}
