using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private NodeRecipe recipe;

    private bool isShooted = false;
    public float delay = 0.7f;

    public IEnumerator DestroyDelay()
    {
        
        yield return new WaitForSeconds(delay);
        if (!isShooted)
        {
            TableManager tableManager = FindAnyObjectByType<TableManager>();
            foreach (Table table in tableManager.tables)
            {
                if (recipe.orderTableNumber == table.tableNumber)
                {
                    table.currentPassenger.Exit(false, 0, recipe);
                    table.ResetTable();
                }
            }
        }
        if (this != null)

        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Table")) // 테이블과 닿았을때
        {
           
            var collidingTable = collision.gameObject.GetComponentInParent<Table>();

            if (collidingTable == null)
            {
                Debug.Log("Table 컴포넌트 없음!");
                return;
            }
            if (collidingTable.currentPassenger == null)
            {
                Debug.Log("currentPassenger 없음!");
                return;
            }
            if (collidingTable.currentPassenger.selectedFood == null)
            {
                Debug.Log("selectedFood 없음!");
                return;
            }
            Debug.Log(collidingTable.currentPassenger.selectedFood.foodNodeName);
            Debug.Log(recipe);
 
            if(collidingTable.currentPassenger.selectedFood.foodNodeName == recipe.dishName) // 시킨 음식의 이름과 준비된 레시피의 이름이 같다면
            {
                collidingTable.currentPassenger.Exit(true, recipe.currentstepIndex,recipe); // 만족하면서 나가기
                collidingTable.ReceivedFood(recipe.steps[recipe.currentstepIndex].sprite, true); // 음식받기
                isShooted = true;   
             }
            else // 잘못된 테이블로의 발사?
            {
                TableManager tableManager = FindAnyObjectByType<TableManager>();
                foreach (Table table in tableManager.tables)
                {
                    if (recipe.orderTableNumber == table.tableNumber) // 맞는 테이블 찾아서 손님 내쫒기
                    {
                        table.currentPassenger.Exit(false, 0, recipe);
                        table.ResetTable();
                    }
                }
            }
            Debug.Log("테이블과의 접촉");
            

            Destroy(this.gameObject);
        }
    }

    

    public void getNodeRecipe(NodeRecipe r)
    {
        recipe = r;
    }
}
