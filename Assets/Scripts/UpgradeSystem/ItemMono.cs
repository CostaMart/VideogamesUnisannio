using UnityEditor;
using UnityEngine;

public class ItemMono : MonoBehaviour
{

    public ItemManager.Item item = ItemManager.globalItemPool[0];

    void OnCollisionStay(Collision collision)
    {
        collision.gameObject.GetComponent<PlayerEffectDispatcher>().ItemDispatch(item);
        Debug.Log("bullet dispatched");
    }

}
