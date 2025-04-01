using UnityEngine;

public class ItemMono : MonoBehaviour
{

    public ItemManager.Item item = ItemManager.globalItemPool[1];

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("press E");
    }

}
