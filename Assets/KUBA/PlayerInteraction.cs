using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private float interactionDistance = 3f; // Jak blisko musisz być drzwi
    private KeyCode interactionKey = KeyCode.E; // Klawisz interakcji

    void Update()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        // Tworzymy promień wychodzący ze środka kamery, skierowany w przód
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Rzucamy promień na odległość interactionDistance
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            // Sprawdzamy, czy obiekt, w który trafiliśmy, ma skrypt DoorController
            DoorController door = hit.collider.GetComponent<DoorController>();

            // Jeśli tak – wywołujemy funkcję otwierania/zamykania
            if (door != null)
            {
                door.ToggleDoor();
            }
        }
    }
}