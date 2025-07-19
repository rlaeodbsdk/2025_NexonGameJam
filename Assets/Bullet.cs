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
        if(collision.CompareTag("Table"))
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
 
                if(collidingTable.currentPassenger.selectedFood.foodNodeName == recipe.dishName)
                {
                collidingTable.currentPassenger.Exit(true, recipe.currentstepIndex,recipe);
                collidingTable.ReceivedFood(recipe.steps[recipe.currentstepIndex].sprite, true);
                isShooted = true;   
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
