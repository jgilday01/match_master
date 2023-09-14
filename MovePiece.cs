using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePiece : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;
    private RigidbodyConstraints originalConstraints;

    void Awake()
    {
        originalConstraints = GetComponent<Rigidbody>().constraints;
    }

    void Update()
    {
        if (startMarker && endMarker)
        {
            GetComponent<Rigidbody>().constraints = originalConstraints;
            transform.position = endMarker.position;
            startMarker = null; endMarker = null;
        }
    }
}
