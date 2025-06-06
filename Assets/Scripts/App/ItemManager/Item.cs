using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ItemManager.Instance.AddItem();
            Destroy(gameObject); 
        }
    }
}