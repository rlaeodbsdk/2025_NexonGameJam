using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public GameObject Node;
    public Transform nodeStart_1;
    public Transform nodeStart_2;

    public int nodeCount = 0;

    private List<NodeRecipe> nodeRecipes = new List<NodeRecipe>();
    void Start()
    {
        Init();
        Managers.UI.ShowPopUpUI<UI_Test>(); // 테스트 Manager 호출
        StartCoroutine(PatternGoNode());
        
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

    IEnumerator PatternGoNode()
    {
        while(true)
        {
            NodeGo();
            yield return new WaitForSeconds(3);
        }
    }
    void NodeGo()
    {
        if (nodeCount <= 6)
        {
            int randomLine = Random.Range(0, 2);//어디에서 나올것인지에 대해
            int randomFood = Random.Range(0, 3);
            if (randomLine == 0) // 왼쪽에서 나오기
            {
                Node FirstNodeCs = Instantiate(Node, nodeStart_1).GetComponent<Node>();
                FirstNodeCs.GetWhereNodeLine(1);
                FirstNodeCs.GetRecipe(nodeRecipes[randomFood]);
                FirstNodeCs.setInstantiateData();

            }
            else // 오른쪽에서 나오기
            {
                Node SecondNodeCs = Instantiate(Node, nodeStart_2).GetComponent<Node>();
                SecondNodeCs.GetRecipe(nodeRecipes[randomFood]);
                SecondNodeCs.setInstantiateData();
            }
            nodeCount++;
        }
    }

    
}
