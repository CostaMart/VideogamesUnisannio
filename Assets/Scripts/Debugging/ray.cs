using UnityEngine;

public class DebugRaycaster : MonoBehaviour
{
    public float rayLength = 10000f;
    public Color rayColor = Color.red;

    void Update()
    {
        // Definisce la direzione del raggio (avanti rispetto all'oggetto)
        Vector3 direction = transform.forward;

        // Disegna il raggio nella scena per debugging
        Debug.DrawLine(transform.position, transform.position + direction * rayLength, rayColor);


        // Controlla se il raggio colpisce qualcosa
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, rayLength))
        {
            Debug.Log("Raggio ha colpito: " + hit.collider.gameObject.name);
        }
    }
}
