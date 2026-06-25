using UnityEngine;
using FMODUnity;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    public float openAngle = 90f;
    public float speed = 3f;

    public bool isOpen = false;
    private Quaternion defaultRotation;
    private Quaternion openRotation;

    [Header("FMOD Audio")]
    public EventReference openEvent;
    public EventReference closeEvent;

    void Start()
    {
        defaultRotation = transform.localRotation;
        openRotation = defaultRotation * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        Quaternion targetRotation = isOpen ? openRotation : defaultRotation;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * speed);
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        // Logika odtwarzania dźwięku przy zmianie stanu drzwi
        if (isOpen)
        {
            // Jeśli drzwi właśnie się otworzyły, odtwarzamy dźwięk otwierania w pozycji drzwi
            RuntimeManager.PlayOneShot(openEvent, transform.position);
        }
        else
        {
            // Jeśli drzwi właśnie się zamknęły, odtwarzamy dźwięk zamykania
            RuntimeManager.PlayOneShot(closeEvent, transform.position);
        }
    }
}