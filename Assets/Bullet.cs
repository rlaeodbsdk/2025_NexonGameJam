using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private NodeRecipe recipe;
    public float delay = 0.7f;

    public IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(delay);
        if (this != null)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Table"))
        {
           
            var collidingTable = collision.gameObject.GetComponent<Table>();
            if (collidingTable == null)
            {
                Debug.Log("Table ������Ʈ ����!");
                return;
            }
            if (collidingTable.currentPassenger == null)
            {
                Debug.Log("currentPassenger ����!");
                return;
            }
            if (collidingTable.currentPassenger.selectedFood == null)
            {
                Debug.Log("selectedFood ����!");
                return;
            }
            Debug.Log(collidingTable.currentPassenger.selectedFood.foodNodeName);
            Debug.Log(recipe);
 
                if(collidingTable.currentPassenger.selectedFood.foodNodeName == recipe.dishName)
                {
                collidingTable.currentPassenger.Exit(true, recipe.currentstepIndex);
                collidingTable.ReceivedFood(recipe.steps[recipe.currentstepIndex].sprite, true);
                   
                }
            
            Debug.Log("���̺���� ����");
            

            Destroy(this.gameObject);
        }
    }

    public void getNodeRecipe(NodeRecipe r)
    {
        recipe = r;
    }
}
