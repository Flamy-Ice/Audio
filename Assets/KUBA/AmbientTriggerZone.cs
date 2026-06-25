using UnityEngine;

public class AmbientTriggerZone : MonoBehaviour
{
    private AmbientManager ambientManager;

    void Start()
    {
        // Szukamy menedżera na scenie
        ambientManager = FindObjectOfType<AmbientManager>();
    }

    // Wywoływane gdy gracz wchodzi w obszar budynku
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ambientManager.SetAmbientZone(1f);
        }
    }

    // Wywoływane gdy gracz opuszcza obszar budynku
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ambientManager.SetAmbientZone(0f);
        }
    }
}