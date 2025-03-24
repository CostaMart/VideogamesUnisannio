using UnityEngine;

// represents the behaviour of taking damage on hit
public class HittableCharacter : MonoBehaviour
{

    [SerializeField] private CharStats charStats;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("remaining life: " + charStats.Life);
            charStats.AddToLife(-10);
        }
    }

}
