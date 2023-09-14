using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameController GC;
    public int gpColor;
    public int gpNumber;
    public int gpType;
    public int gpColumn;

    public void GpIntialize(int c, int n, int t, int l)
    {
        gpColor = c;
        gpNumber = n;
        gpType = t;
        gpColumn = l;
    }

    void Start()
    {
        GameObject GameController = GameObject.FindWithTag("GameController");
        if (GameController != null) GC = GameController.GetComponent<GameController>();
    }

    void OnMouseDown()
    {
        if (GC.gameOver || GC.useAI == true && GC.playTurn == 2 || GC.playable == false) return;

        if (GC.playTurn == 1 && gpType == 1 || GC.playTurn == 1 && gpType == 3) GC.clicked = gameObject;
        else if (GC.playTurn == 2 && gpType == 2 || GC.playTurn == 2 && gpType == 3) GC.clicked = gameObject;        
    }
}

/*
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_WEBGL
void OnMouseDown() {}
#endif
*/
