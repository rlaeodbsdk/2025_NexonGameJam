
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CharacterType { Kongji, Gretel }
public enum ActionType { 손질, 간하기, 넘기기, 굽기, 끓이기 }

[System.Serializable]
public class RecipeStep
{
    public CharacterType character;
    public ActionType action;
    public KeyCode key; // 누를 키
    public Sprite sprite;
}

[CreateAssetMenu(menuName = "NodeRecipe")]
public class NodeRecipe : ScriptableObject
{
    public Sprite startIngredient;
    public float ingredientMoney;
    public string dishName;
    public List<RecipeStep> steps;
    public float price;
    public int eatingTime = 6;
    public int currentstepIndex;
    public int orderTableNumber;
  
}
