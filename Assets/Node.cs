using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    private int currentStepIndex = 0;
    private bool isInFirstZone;
    private bool isInSecondZone;
    public float nodeSpeed = 1f;
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

        if (Time.timeScale != 0f)
        {
            if (nodeLine == 1) // ���� ������
            {
                transform.position = transform.position + new Vector3(1f * nodeSpeed * Time.deltaTime, 0, 0); // ��� ������ �ڵ������� ����������

            }
            else if (nodeLine == 2) // ������ ������
            {
                transform.position = transform.position + new Vector3(-1f * nodeSpeed * Time.deltaTime, 0, 0); // ��� ������ �ڵ������� ��������
            }
        }
    
       




        if (currentState == NodeState.Flying || isReady ==true)
        {
            return;
        }
        if (isInFirstZone) //ù��° ��
        {
            if (Input.GetKeyDown(currentStep.key) && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.W))) //�´�Ű ������ 
            {
                if(Input.GetKeyDown(KeyCode.A))
                {
                    if(recipe.dishName=="ChickenSoup"&&currentStepIndex==0) // ���� ���� ���� 
                    {
                        Managers.Sound.Play("SFX/chickenDie1"); 
                    }
                    Managers.Sound.Play("SFX/Kongjui_knife1");
                }
                else if(Input.GetKeyDown(KeyCode.D))
                {
                    Managers.Sound.Play("SFX/Kongjui_salt1");
                }
                else if(Input.GetKeyDown(KeyCode.W))
                {
                    Managers.Sound.Play("SFX/Kongjui&Gretel_pass1");
                }

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
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Managers.Sound.Play("SFX/Gretel_fire1");
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    Managers.Sound.Play("SFX/Gretel_water1");
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Managers.Sound.Play("SFX/Kongjui&Gretel_pass1");
                }

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
        Managers.Game.OrderCount++;
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
                requestedTable.currentPassenger.Exit(false, 0,recipe);
                requestedTable.ResetTable();
            }
            DestroyNode(); }
        Managers.Game.totalIngredientMoney += recipe.ingredientMoney;
        Debug.Log(Managers.Game.totalIngredientMoney);



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
