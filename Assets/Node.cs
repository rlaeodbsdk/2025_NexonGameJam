using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    private int currentStepIndex = 0;
    private bool isInFirstZone;
    private bool isInSecondZone;
    private float nodeSpeed = 1;
    private bool upThrow = false;
    
    private int nodeLine = 0;

    private NodeRecipe recipe;

    enum NodeState
    {
        Moving,
        Flying
    }
    private NodeState currentState;
    private void Start()
    {
        currentState = NodeState.Moving;
    }
    private void Update()
    {
        if (nodeLine == 1) // 왼쪽 노드라인
        {
            transform.position = transform.position + new Vector3(1f * nodeSpeed * Time.fixedDeltaTime, 0, 0); // 노드 생성시 자동움직임 오른쪽으로

        }
        else if (nodeLine == 2) // 오른쪽 노드라인
        {
            transform.position = transform.position + new Vector3(-1f * nodeSpeed * Time.fixedDeltaTime, 0, 0); // 노드 생성시 자동움직임 왼쪽으로
        }


        RecipeStep currentStep = recipe.steps[currentStepIndex];

  

        if(currentState==NodeState.Flying)
        {
            return;
        }
        if (isInFirstZone) //첫번째 존
        {
            if (Input.GetKeyDown(currentStep.key)) //맞는키 누르면 
            {
                ThrowUpNode();
            }
            else if(Input.GetKeyDown(KeyCode.Alpha1)) // 첫번쨰존 넘기기 키 && 현재 Moving  상태
            {
                ThrowNode();
            }
        }
        else if(isInSecondZone) //두번쨰 존에선 S를 눌러야 사라짐
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Destroy(this.gameObject);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha3)&&currentState==NodeState.Moving)
            {
                ThrowNode();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("FirstHitLine"))
        {
            isInFirstZone = true;
            Debug.Log("First Hit");
        }
        else if(collision.CompareTag("SecondHitLine"))
        {
            isInSecondZone = true;
            Debug.Log("Second Hit");
        }
        else
        {
            Debug.Log("Nothing Trigger");
            isInFirstZone = false;
            isInSecondZone = false;
        }


        if(collision.CompareTag("DestroyZone"))
        {
            Debug.Log("파괴");
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("FirstHitLine"))
        {
            isInFirstZone = false;
            Debug.Log("First Hit");
        }
        else if (collision.CompareTag("SecondHitLine"))
        {
            isInSecondZone = false;
            Debug.Log("Second Hit");
        }
    }
    
    public void GetWhereNodeLine(int nodeNumber)
    {
        nodeLine = nodeNumber;
    }

    public void GetRecipe(NodeRecipe r)
    {
        recipe = r;
    }
    private void ThrowUpNode()
    {
        upThrow = true;
        currentState = NodeState.Flying;
        GetComponent<ParabolaMover>().StartParabolaMove(this.transform.position, this.gameObject);
    }
    private void ThrowNode()
    {
        upThrow = false;
        Vector3 targetPosition = transform.position;
        currentState = NodeState.Flying;
        if(nodeLine==1)
        {
            targetPosition += new Vector3(12,0, 0);
        }
        else if(nodeLine==2)
        {
            targetPosition -= new Vector3(12, 0, 0);
        }
        GetComponent<ParabolaMover>().StartParabolaMove(targetPosition,this.gameObject);
    }

    public void ChangeLine()
    {
        if (upThrow == false)
        {
            if (nodeLine == 1)
            {
                nodeLine = 2;
            }
            else if (nodeLine == 2)
            {
                nodeLine = 1;
            }
        }
        currentState = NodeState.Moving;
    }
}
