using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPulse : MonoBehaviour
{
    public float pulseSpeed;
    public float pulseRange;
    public float pulseMinimum;

    void Update()
    {
        GetComponent<Light>().intensity = pulseMinimum + (Mathf.PingPong(Time.time * pulseSpeed, pulseRange)); //Pulsing
    }
}
