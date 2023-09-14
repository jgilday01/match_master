using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDoor : MonoBehaviour
{
    public GameController GC;
    public Transform DoorStart;
    public Transform DoorStop;
    public float doorSpeed;
    private bool started = false;

    void Update()
    {
        if (GC.DoorOpened) { return; }

        if (GC.OpenDoor && !started)
        {
            GetComponent<AudioSource>().Play();
            started = true;
        }

        if (GC.OpenDoor == true)
        {
            transform.position = Vector3.Lerp(DoorStart.transform.position, DoorStop.transform.position, Time.deltaTime * doorSpeed);

            if (Vector3.Distance(transform.position, DoorStop.transform.position) < .05)
            {
                transform.position = DoorStop.position;
                GC.OpenDoor = false;
                GC.DoorOpened = true;
            }
        }
    }
}
