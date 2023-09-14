using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSelector : MonoBehaviour
{
    public GameController GC;
    public Vector3 direction;
    public float moveSpeed;
    public float turnSpeed;
    public float stopMark;
    public int turnTrack;
    public Transform SP;
    public Transform P1;
    public Transform P2;
    public Transform moveMarker;
    private bool moving = false;

    void FixedUpdate()
    {
        if (turnTrack != GC.playTurn)
        {
            turnTrack = GC.playTurn;
            if (turnTrack == 1)
            {
                moveMarker = P1;
                direction = Vector3.up;
            }
            if (turnTrack == 2)
            {
                moveMarker = P2;
                direction = Vector3.down;
            }
            moving = true;
        }

        if (moving)
        {
            transform.position = Vector3.Lerp(transform.position, moveMarker.transform.position, Time.deltaTime * moveSpeed);

            if (Vector3.Distance(transform.position, moveMarker.transform.position) < stopMark)
            {
                transform.position = moveMarker.position;
                moving = false;
            }
        }

        if (transform.position.x < -2.2 || transform.position.x > 2.2) transform.Rotate(direction * Time.deltaTime * turnSpeed);
    }
}
