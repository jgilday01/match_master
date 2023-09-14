using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public GetPiece GP;
    public GameController GC;
    public GameObject gpAppear;
    public int pieceMax;
    public int p1c1Count;
    public int p1c2Count;
    public int p1c3Count;
    public int p2c1Count;
    public int p2c2Count;
    public int p2c3Count;
    public float dropHeight;

    private int playerNum;

    // Start is called before the first frame update
    void Start()
    {
        p1c1Count = 2;
        p1c2Count = 3;
        p1c3Count = 2;

        p2c1Count = 2;
        p2c2Count = 3;
        p2c3Count = 2;
    }

    public void ClickedButton()
    {
        if (GC.playTurn == 1 && GC.p1_count >= pieceMax) { GC.turnSkip = true; return; }
        if (GC.playTurn == 2 && GC.p2_count >= pieceMax) { GC.turnSkip = true; return; }

        float placementX = 0.0f;

        if (GC.playTurn == 1)
        {
            if (p1c1Count < p1c2Count)
            {
                placementX = -3;
                p1c1Count++;
            }
            else if (p1c2Count <= p1c3Count)
            {
                placementX = -4;
                p1c2Count++;
            }
            else
            {
                placementX = -5;
                p1c3Count++;
            }

            playerNum = 1;
        }
        else if (GC.playTurn == 2)
        {
            if (p2c1Count < p2c2Count)
            {
                placementX = 3;
                p2c1Count++;
            }
            else if (p2c2Count <= p2c3Count)
            {
                placementX = 4;
                p2c2Count++;
            }
            else
            {
                placementX = 5;
                p2c3Count++;
            }

            playerNum = 2;
        }

        if (!GC.pieceAdded) StartCoroutine(addPiece(placementX, playerNum));

        GC.pieceAdded = true;
        GC.message.text = "New Piece";
        GC.message.color = Color.gray;
    }

    private IEnumerator addPiece(float placementX, int playerNum)
    {
        Instantiate(gpAppear, new Vector3(placementX, dropHeight, 0), Quaternion.Euler(270, 0, 0));
        yield return new WaitForSeconds(0.5f);

        GP.Retrieve(placementX, dropHeight, 0, playerNum, 0);
    }
}
