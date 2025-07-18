
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CharacterType { Kongji, Gretel }
public enum ActionType { ����, ���ϱ�, �ѱ��, ����, ���̱� }

[System.Serializable]
public class RecipeStep
{
    public CharacterType character;
    public ActionType action;
    public KeyCode key; // ���� Ű
}

[CreateAssetMenu(menuName = "NodeRecipe")]
public class NodeRecipe : ScriptableObject
{
    public string dishName;
    public List<RecipeStep> steps;
}
