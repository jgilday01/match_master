using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_logic : MonoBehaviour
{
    public GameController GC;
    public GameObject[] gamePieceList;

    public void p2Moves()
    {
        gamePieceList = GameObject.FindGameObjectsWithTag("GamePiece");

        List<GameObject> player1GamePiece = new List<GameObject>(); //player1 pieces
        List<GameObject> player2GamePiece = new List<GameObject>(); //player2 pieces
        List<GameObject> playFieldGamePiece = new List<GameObject>(); //playfieild (playfield1)
        List<GameObject> oldPieceGamePiece = new List<GameObject>(); //old peice (playfield2)
        List<GameObject> player2MovePiece = new List<GameObject>(); //player piece selection
        List<GameObject> playfieldMovePiece = new List<GameObject>(); //playfield position

        for (int gamePiece = 0; gamePiece < gamePieceList.Length; gamePiece++)
        {
            if (gamePieceList[gamePiece].GetComponent<Interaction>().gpType == 1) { player1GamePiece.Add(gamePieceList[gamePiece]); }
            if (gamePieceList[gamePiece].GetComponent<Interaction>().gpType == 2) { player2GamePiece.Add(gamePieceList[gamePiece]); }
            if (gamePieceList[gamePiece].GetComponent<Interaction>().gpType == 3) { playFieldGamePiece.Add(gamePieceList[gamePiece]); }
            if (gamePieceList[gamePiece].GetComponent<Interaction>().gpType == 4) { oldPieceGamePiece.Add(gamePieceList[gamePiece]); }
        }

        for (int playfield = 0; playfield < playFieldGamePiece.Count; playfield++)
        {
            for (int player2 = 0; player2 < player2GamePiece.Count; player2++)
            {
                bool duplicated = false;

                if (pieceCheck(playFieldGamePiece[playfield], player2GamePiece[player2], 1))
                {
                    for (int oldPiece = 0; oldPiece < oldPieceGamePiece.Count; oldPiece++)
                    {
                        if (oldPieceGamePiece[oldPiece].GetComponent<Interaction>().gpColumn == playFieldGamePiece[playfield].GetComponent<Interaction>().gpColumn)
                        {
                            duplicated = pieceCheck(player2GamePiece[player2], oldPieceGamePiece[oldPiece], 2);
                        }
                    }

                    if (!duplicated)
                    {
                        player2MovePiece.Add(player2GamePiece[player2]);
                        playfieldMovePiece.Add(playFieldGamePiece[playfield]);
                    }
                }
            }
        }

        assignPieces(playfieldMovePiece, player2MovePiece);
    }

    private bool pieceCheck(GameObject playfieldPiece, GameObject player2Piece, int checkNumber)
    {
        string playfieldName = playfieldPiece.name; //playfield
        int playfieldColor = playfieldPiece.GetComponent<Interaction>().gpColor;
        int playfieldNumber = playfieldPiece.GetComponent<Interaction>().gpNumber;

        string player2Name = player2Piece.name; //player2
        int player2Color = player2Piece.GetComponent<Interaction>().gpColor;
        int player2Number = player2Piece.GetComponent<Interaction>().gpNumber;

        if (playfieldNumber == player2Number && playfieldColor != player2Color && checkNumber == 1) return true; //number match
        else if (playfieldColor == player2Color && Mathf.Abs(playfieldNumber - player2Number) == 1 && checkNumber == 1) return true; //color match

        if (playfieldNumber == player2Number && playfieldColor == player2Color && checkNumber == 2) return true; //dup test

        return false;
    }

    private void assignPieces(List<GameObject> playfieldMovePiece, List<GameObject> player2MovePiece)
    {
        if (player2MovePiece.Count == 1)
        {
            GC.AImove1 = player2MovePiece[0];
            GC.AImove2 = playfieldMovePiece[0];
        }
        else if (player2MovePiece.Count > 1)
        {
            int whichPiece = Random.Range(0, player2MovePiece.Count);
            GC.AImove1 = player2MovePiece[whichPiece];
            GC.AImove2 = playfieldMovePiece[whichPiece];
        }
    }
}
