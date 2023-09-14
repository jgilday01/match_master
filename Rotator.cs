using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed;
    public float range;

    void Update()
    {
        transform.rotation = Quaternion.Euler(0.0f, (Mathf.PingPong(Time.time * speed, range) - range / 2), 0.0f);
    }
}
