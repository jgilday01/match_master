using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Validation : MonoBehaviour
{
    public Text message;
    public Transform[] newPiece = new Transform[3];
    public GameObject[] dupTracking = new GameObject[3];
    public GameObject gpDisappear;

    public bool CheckMove(GameObject player, GameObject playField)
    {
        int lastColor = -1;
        int lastNumber = -1;
        int choicePF = (int)Mathf.Round(playField.transform.position.x + 1);

        if (dupTracking[choicePF] != null)
        {
            lastColor = dupTracking[choicePF].GetComponent<Interaction>().gpColor;
            lastNumber = dupTracking[choicePF].GetComponent<Interaction>().gpNumber;
        }

        int pColor = player.GetComponent<Interaction>().gpColor;
        int pNumber = player.GetComponent<Interaction>().gpNumber;
        int pfColor = playField.GetComponent<Interaction>().gpColor;
        int pfNumber = playField.GetComponent<Interaction>().gpNumber;

        //duplication check
        if (pColor == lastColor && pNumber == lastNumber || pColor == pfColor && pNumber == pfNumber)
        { message.color = Color.red; message.text = "Duplication"; return false; }

        //same number of different color
        if (pNumber == pfNumber && pColor != pfColor)
        { message.color = Color.green; message.text = "Number Match"; }
        //same color with sequential number
        else if (pColor == pfColor && Mathf.Abs(pNumber - pfNumber) == 1)
        { message.color = Color.green; message.text = "Color Match"; }
        //invalid selection
        else
        { message.color = Color.red; message.text = "Invalid Move"; return false; }

        //send move parameters
        player.GetComponent<MovePiece>().startMarker = player.transform;
        player.GetComponent<MovePiece>().endMarker = newPiece[choicePF].transform;

        //update game piece Type
        player.GetComponent<Interaction>().gpType = 3;
        player.GetComponent<Interaction>().gpColumn = playField.GetComponent<Interaction>().gpColumn;
        playField.GetComponent<Interaction>().gpType = 4;

        //remove old piece
        if (dupTracking[choicePF])
        { Instantiate(gpDisappear, dupTracking[choicePF].transform.position, Quaternion.Euler(270, 0, 0)); }

        Destroy(dupTracking[choicePF], 0.05f);

        //set the old Playfield piece
        dupTracking[choicePF] = playField;

        return true;
    }
}
