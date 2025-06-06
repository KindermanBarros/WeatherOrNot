using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    private int itemCount = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddItem()
    {
        itemCount++;
        Debug.Log("Itens collectable: " + itemCount);
    }

    public int GetItemCount()
    {
        return itemCount;
    }
}