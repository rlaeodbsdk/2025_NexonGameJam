using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public GameObject Node;
    public Transform nodeStart_1;
    public Transform nodeStart_2;

    public int nodeCount = 0;
    public bool nodeBroken = false;
    private int tutorialNodeLineIdx = 0;
    public NodeLauncher launcher;
    public TableManager tableManager;
    

    private List<NodeRecipe> nodeRecipes = new List<NodeRecipe>();
    void Start()
    {
        Init();
        Managers.UI.ShowPopUpUI<UI_Test>(); // 테스트 Manager 호출
        //StartCoroutine(PatternGoNode());
        tableManager = FindFirstObjectByType<TableManager>();
        Managers.Sound.Play("BGM/stageBGM1", Define.Sound.BGM);
    }

    public NodeRecipe GetRecipe(int id)
    {
        switch(id)
        {
            case 0:
                return Resources.Load<NodeRecipe>("Recipes/Steak");
            case 1:
                return Resources.Load<NodeRecipe>("Recipes/TomatoSoup");
            case 2:
                return Resources.Load<NodeRecipe>("Recipes/MeatBall");


            default:
                Debug.Log("큰일난 오류~!");
                return null;
        }
    }



    void Init()
    {
        for(int i=0;i<3;i++)
        {
            nodeRecipes.Add(GetRecipe(i));
        }
    }



    // Update is called once per frame
    void Update()
    {
  
    }


    public void NodeGo(int tableNumber, string recipeName=null, Table requestedTable = null)
    {
        if (nodeCount <= 6)
        {
            Managers.Game.OrderCount++;

            int randomLine;
            if (Managers.Game.isTutorial)
            {
                
                randomLine = tutorialNodeLineIdx;
                tutorialNodeLineIdx = (tutorialNodeLineIdx + 1) % 2;
            }
            else
            {
                randomLine = Random.Range(0, 2);
            }

            NodeRecipe recipeToUse = null;
            if (!string.IsNullOrEmpty(recipeName))
            {
                recipeToUse = nodeRecipes.Find(r => r.dishName == recipeName);
                recipeToUse.orderTableNumber = tableNumber;
            }
            if (randomLine == 0) // 왼쪽에서 나오기
            {
                Node FirstNodeCs = Instantiate(Node, nodeStart_1).GetComponent<Node>();
                FirstNodeCs.GetWhereNodeLine(1);
                FirstNodeCs.GetRecipe(recipeToUse);
                FirstNodeCs.requestedTable = requestedTable;
                FirstNodeCs.setInstantiateData();
                //FirstNodeCs.GetRecipe(nodeRecipes[randomFood]);
                //FirstNodeCs.setInstantiateData();


            }
            else // 오른쪽에서 나오기
            {
                Node SecondNodeCs = Instantiate(Node, nodeStart_2).GetComponent<Node>();
                SecondNodeCs.GetWhereNodeLine(2);
                SecondNodeCs.GetRecipe(recipeToUse);
                SecondNodeCs.requestedTable = requestedTable;
                SecondNodeCs.setInstantiateData();
            }
            nodeCount++;
        }
    }

    public void readyOnBulltet(NodeRecipe recipe)
    {
        launcher.SpawnNote(recipe);
    }

    
}
