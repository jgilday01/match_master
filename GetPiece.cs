using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPiece : MonoBehaviour
{
    public GameController GC;
    public GameStack GS;
    public GameObject piecePrefab;

    public void Retrieve(float Px, float Py, float Pz, int gpType, int gpPos)
    {
        //check that there are pieces remaining
        if (GC.gpTracking.Count == 0) { return; }

        //choose a new random GamePiece
        int whichPiece = Random.Range(0, GC.gpTracking.Count);
        int temp = GC.gpTracking[whichPiece]; //parseInt(GC.gpTracking[whichPiece].ToString());
        GameObject newPiece = Instantiate(piecePrefab, new Vector3(Px, Py, Pz), Quaternion.identity);

        //setup GamePiece color, number and name
        int gpCol = GS.gpStack[temp].gpColor;
        int gpNum = GS.gpStack[temp].gpNumber;
        newPiece.GetComponent<Renderer>().material = GS.materials[gpNum - 1]; //gpNum
        newPiece.GetComponent<Renderer>().material.color = (GS.matCol[gpCol - 1]); //gpCol
        newPiece.name = temp.ToString();

        GS.gpStack[temp].gpUsed = true; //added in

        //Intraction script with gpIntialize function
        newPiece.GetComponent<Interaction>().GpIntialize(gpCol, gpNum, gpType, gpPos);

        //remove GamePiece from tracking array
        GC.gpTracking.RemoveAt(whichPiece);
    }
}
