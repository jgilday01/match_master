using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStack : MonoBehaviour
{
    public Color[] matCol = new Color[5];
    public Material[] materials = new Material[5];
    public gpMaster[] gpStack = new gpMaster[50];

    //[System.Serializable] //show in inspector

    public class gpMaster
    {
        public int gpColor;
        public int gpNumber;
        public bool gpUsed;
    }

    public void SetupPieces()
    {
        int x = 0;

        for (var c = 1; c < 6; c++)
        {
            for (var n = 1; n < 6; n++)
            {
                gpStack[x] = new gpMaster();
                gpStack[x + 1] = new gpMaster();
                gpStack[x].gpColor = c;
                gpStack[x].gpNumber = n;
                gpStack[x + 1].gpColor = c;
                gpStack[x + 1].gpNumber = n;
                x = x + 2;
            }
        }

    }
}
