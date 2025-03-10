using UnityEngine;

public class TelecameraSegui : MonoBehaviour
{
    public Transform personaggio; // Riferimento al personaggio che la telecamera deve seguire
    public float distanza = 10f;  // Distanza desiderata dalla telecamera al personaggio
    public float altezza = 5f;    // Altezza della telecamera rispetto al personaggio
    public float offsetLaterale = 2f; // Offset laterale della telecamera (posizione orizzontale rispetto al personaggio)
    public float velocitàRotazione = 50f;  // Velocità con cui la telecamera ruota
    public float velocitàMovimento = 5f;  // Velocità con cui la telecamera segue il personaggio

    private Vector3 offset;  // Offset iniziale tra telecamera e personaggio

    void Start()
    {
        // Calcoliamo l'offset iniziale basato sulla distanza, altezza e offset laterale
    }

    void FixedUpdate()
    {
        offset = new Vector3(offsetLaterale, altezza, -distanza);
        // Se il personaggio è stato assegnato, spostiamo e ruotiamo la telecamera
        if (personaggio != null)
        {
            // Calcoliamo la posizione desiderata della telecamera, ruotata con il personaggio
            Vector3 posizioneDesiderata = personaggio.position + personaggio.rotation * offset;

            // Muoviamo la telecamera in modo fluido verso la posizione desiderata
            transform.position = posizioneDesiderata;

            // La telecamera guarda sempre il personaggio
            Quaternion rotazioneDesiderata = personaggio.rotation;
            transform.rotation = rotazioneDesiderata;
        }
    }
}


