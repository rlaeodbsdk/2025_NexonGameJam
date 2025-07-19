using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    private int currentStepIndex = 0;
    private bool isInFirstZone;
    private bool isInSecondZone;
    public float nodeSpeed = 1;
    private bool upThrow = false;
    private bool isReady = false;
    private NodeManager nodeManager;
    
    private int nodeLine = 0;

    private NodeRecipe recipe;
    private RecipeStep currentStep;
    public Table requestedTable;
    

    enum NodeState
    {
        Moving,
        Flying
    }
    private NodeState currentState;
    private void Start()
    {
        currentState = NodeState.Moving;
        nodeManager = FindAnyObjectByType<NodeManager>();
    }

    public void setInstantiateData()
    {
        currentStep = recipe.steps[currentStepIndex];
        GetComponentInChildren<SpriteRenderer>().sprite = recipe.startIngredient;
    }
    private void Update()
    {


        if (nodeLine == 1) // ���� ������
        {
            transform.position = transform.position + new Vector3(1f * nodeSpeed * Time.fixedDeltaTime, 0, 0); // ��� ������ �ڵ������� ����������

        }
        else if (nodeLine == 2) // ������ ������
        {
            transform.position = transform.position + new Vector3(-1f * nodeSpeed * Time.fixedDeltaTime, 0, 0); // ��� ������ �ڵ������� ��������
        }




        if (currentState == NodeState.Flying || isReady ==true)
        {
            return;
        }
        if (isInFirstZone) //ù��° ��
        {
            if (Input.GetKeyDown(currentStep.key) && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.W))) //�´�Ű ������ 
            {
                currentStepIndex++;
                ThrowUpNode();
                GetComponentInChildren<SpriteRenderer>().sprite = currentStep.sprite;
                if (currentStepIndex >= recipe.steps.Count) // ���̻� ������ ����? -> �׷��� �ϼ�
                {
                    Debug.Log($"[{recipe.dishName}] �ϼ�!");
                    isReady = true;
                    return;
                }
                currentStep = recipe.steps[currentStepIndex];
                // �ϼ��� �ƴ϶��

            }
            else if(Input.GetKeyDown(KeyCode.W))
            {
                ThrowNode();
            }
            //�߸���Ű ������ �ƹ� ������ ���� -> ���
        }
        else if (isInSecondZone) //�ι��� ������ S�� ������ �����
        {
            if (Input.GetKeyDown(currentStep.key) && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow)))
            {
                currentStepIndex++;
                ThrowUpNode();
                GetComponentInChildren<SpriteRenderer>().sprite = currentStep.sprite;
                if (currentStepIndex >= recipe.steps.Count) // ���̻� ������ ����? -> �׷��� �ϼ�
                {
                    Debug.Log($"[{recipe.dishName}] �ϼ�!");
                    isReady = true;
                    return;
                }
                currentStep = recipe.steps[currentStepIndex];
                // �ϼ��� �ƴ϶��
            }
            else if(Input.GetKeyDown(KeyCode.UpArrow))
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
        }
        else if(collision.CompareTag("SecondHitLine"))
        {
            isInSecondZone = true;
        }
        else
        {
            isInFirstZone = false;
            isInSecondZone = false;
        }


        if(collision.CompareTag("DestroyZone"))
        {
            StartCoroutine(CompleteGoDestroy());
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

    IEnumerator CompleteGoDestroy() //�ϼ������� ���ִ� ����
    {
        upThrow = true;
        currentState = NodeState.Flying;
        Transform transform = this.transform;
        transform.position = transform.position + new Vector3(0, 2, 0);
        GetComponent<ParabolaMover>().StartParabolaMove(transform.position, this.gameObject);
        yield return new WaitForSecondsRealtime(0.7f);
        float sellingPrice;
        if(isReady==true)
        {
             recipe.price = recipe.price; // ��¥�� �ϼ�������
        }
        else
        {
            recipe.price = recipe.price * ((float)currentStepIndex / recipe.steps.Count);
        }
        Debug.Log($"{currentStepIndex}/{recipe.steps.Count}");
        
        if(currentStepIndex!=0)
        {
            recipe.currentstepIndex = --currentStepIndex;
            nodeManager.readyOnBulltet(recipe);
        }
        else {
            nodeManager.nodeBroken = true;
            if (requestedTable != null && requestedTable.currentPassenger != null)
            {
                requestedTable.currentPassenger.Exit(false, 0);
                requestedTable.ResetTable();
            }
            DestroyNode(); }
        Debug.Log(Managers.Data.TotalPrice);
        
        

        Destroy(this.gameObject);
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

    private void DestroyNode()
    {
        nodeManager.nodeCount--;
    }
}
