using UnityEngine;

public class ItemMono : MonoBehaviour
{

    public ItemManager.Item item = ItemManager.globalItemPool[0];

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("item raccolto dal giocatore : " + item.id);
        Debug.Log("item raccolto dal giocatore : " + item.name);
    }

}
