using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public GameObject Node;
    public Transform nodeStart_1;
    public Transform nodeStart_2;

    void Start()
    {
        Managers.UI.ShowPopUpUI<UI_Test>(); // 테스트 Manager 호출
        StartCoroutine(PatternGoNode());
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
            yield return new WaitForSeconds(1);
        }
    }
    void NodeGo()
    {
        Node FirstNodeCs = Instantiate(Node, nodeStart_1).GetComponent<Node>();
        FirstNodeCs.GetWhereNodeLine(1);
        NodeRecipe steakRecipe = Resources.Load<NodeRecipe>("Recipes/Steak");
        FirstNodeCs.GetRecipe(steakRecipe);

        Node SecondNodeCs = Instantiate(Node, nodeStart_2).GetComponent<Node>();
        SecondNodeCs.GetWhereNodeLine(2);
        NodeRecipe steakRecipe2 = Resources.Load<NodeRecipe>("Recipes/Steak");
        SecondNodeCs.GetRecipe(steakRecipe2);
    }
}
