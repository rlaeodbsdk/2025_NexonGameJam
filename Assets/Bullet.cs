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
        if (this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Table"))
        {
            Debug.Log("테이블과의 접촉");
            

            Destroy(this.gameObject);
        }
    }

    public void getNodeRecipe(NodeRecipe r)
    {
        recipe = r;
    }
}
