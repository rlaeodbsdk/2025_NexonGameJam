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
        Managers.UI.ShowPopUpUI<UI_Test>(); // �׽�Ʈ Manager ȣ��
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
                Debug.Log("ū�ϳ� ����~!");
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
            int randomLine = Random.Range(0, 2);//��𿡼� ���ð������� ����
            int randomFood = Random.Range(0, 3);
            if (randomLine == 0) // ���ʿ��� ������
            {
                Node FirstNodeCs = Instantiate(Node, nodeStart_1).GetComponent<Node>();
                FirstNodeCs.GetWhereNodeLine(1);
                FirstNodeCs.GetRecipe(nodeRecipes[randomFood]);
                FirstNodeCs.setInstantiateData();

            }
            else // �����ʿ��� ������
            {
                Node SecondNodeCs = Instantiate(Node, nodeStart_2).GetComponent<Node>();
                SecondNodeCs.GetRecipe(nodeRecipes[randomFood]);
                SecondNodeCs.setInstantiateData();
            }
            nodeCount++;
        }
    }

    
}
