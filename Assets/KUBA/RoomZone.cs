using UnityEngine;

public class RoomZone : MonoBehaviour
{
    [Header("Przypisz drzwi do tego pokoju")]
    public DoorController roomDoor;

    private AmbientManager ambientManager;
    private bool isPlayerInside = false;

    void Start()
    {
        ambientManager = FindObjectOfType<AmbientManager>();
    }

    void Update()
    {
        // Jeśli gracz jest wewnątrz pokoju
        if (isPlayerInside)
        {
            // Jeśli drzwi są ZAMKNIĘTE (!roomDoor.isOpen), aktywujemy tłumienie (1f)
            // Jeśli drzwi są OTWARTE, wyłączamy tłumienie (0f)
            if (!roomDoor.isOpen)
            {
                ambientManager.SetRoomOcclusion(1f);
            }
            else
            {
                ambientManager.SetRoomOcclusion(0f);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            // Po wyjściu z pokoju natychmiast upewniamy się, że tłumienie jest wyłączone
            ambientManager.SetRoomOcclusion(0f);
        }
    }
}