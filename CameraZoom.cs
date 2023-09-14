using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Transform CameraMount;
    public float speed;
    public float startDistance;
    public GameController GC;
    private bool mounted = false;

    void Update()
    {
        if (mounted) return;

        if (GC.DoorOpened && !mounted)
        {
            transform.position = CameraMount.position; //move to play area
            mounted = true;
            GC.StartupSequence();
        }
    }
}