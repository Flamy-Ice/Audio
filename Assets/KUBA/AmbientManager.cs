using UnityEngine;
using FMODUnity;

public class AmbientManager : MonoBehaviour
{
    public EventReference ambientEvent;
    private FMOD.Studio.EventInstance ambientInstance;

    void Start()
    {
        // Odpalamy ambient od razu po włączeniu gry
        ambientInstance = RuntimeManager.CreateInstance(ambientEvent);
        ambientInstance.start();

        // NOWE: Zaczynamy na zewnątrz (wartość 0)
        ambientInstance.setParameterByName("InsideOutside", 0f);
    }

    // Ta metoda będzie wywoływana przez triggery przy wejściu/wyjściu z budynku
    public void SetAmbientZone(float value)
    {
        ambientInstance.setParameterByName("InsideOutside", value);
    }

    private void OnDestroy()
    {
        ambientInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambientInstance.release();
    }

    // Metoda kontrolująca tłumienie dźwięków w pokoju
    public void SetRoomOcclusion(float value)
    {
        ambientInstance.setParameterByName("RoomClosed", value);
    }
}