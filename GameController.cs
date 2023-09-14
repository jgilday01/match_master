using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameStack GS;
    public Validation PV;
    public GetPiece GP;
    public ButtonClick BC;
    public AI_logic AI;
    public Sound_FX FX;
    public BG_Music BG;

    public GameObject AImove1;
    public GameObject AImove2;
    public GameObject clicked;
    public GameObject piece1;
    public GameObject piece2;
    public GameObject MainMenu;
    public GameObject ActionOpt;
    public GameObject Messages;
    public GameObject RestartBtn;
    public GameObject QuitBtn;
    public GameObject Sounds_CTL;
    //public GameObject winStar;
    public GameObject MainCanvas;
    public GameObject MainCamera;
    public Transform CameraStart;
    public Transform CameraMount;

    public Animator menuAnimation;
    public RectTransform mainPanel;

    public Toggle CToggle;
    public Toggle HToggle;

    public Slider FXVolume;
    public Slider ToneVolume;
    public Slider BeatVolume;

    public Text PlayOption;
    public Text piecesLeft;
    public Text round;
    public Text message;
    public Text Player1;
    public Text Player2;
    public Text StartText;
    public Text winText;

    public int playTurn;
    public int roundCount;
    public int p1_count;
    public int p2_count;
    public List<int> gpTracking = new List<int>();

    public bool useAI = false;
    public bool clickable = false;
    public bool playable = false;
    public bool pieceAdded = false;
    public bool P1Skip = false;
    public bool P2Skip = false;
    public bool turnSkip = false;
    public bool bagEmpty = false;
    public bool confirmedMove = false;
    public bool stalemate = false;
    public bool gameOver = false;
    public bool startup = false;
    public bool restart = false;
    public bool soundMute = false;
    public bool OpenDoor = false;
    public bool DoorOpened = false;

    // Start is called before the first frame update
    void Start()
    {
        MainMenu.SetActive(true);

        FXVolume.value = 1.00f;
        ToneVolume.value = 0.75f;
        BeatVolume.value = 0.50f;

        GS.SetupPieces();

        for (int i = 0; i < 50; i++) { gpTracking.Add(i); }

        RestartBtn.SetActive(false);
        QuitBtn.SetActive(false);

        roundCount = 1;
        p1_count = 7;
        p2_count = 7;
    }

    void Update()
    {
        if (restart && !RestartBtn.activeSelf)
        {
            RestartBtn.SetActive(true);
            
            #if ((UNITY_EDITOR || UNITY_STANDALONE) && !UNITY_WEBGL)
                QuitBtn.SetActive(false);
            #endif
        }

        if (clickable && ActionOpt.activeSelf == false) { ActionOpt.SetActive(true); }
        else if (!clickable && ActionOpt.activeSelf == true) { ActionOpt.SetActive(false); }
    }

    public void PlayerTypeSwap()
    {
        if (CToggle.isOn) { useAI = true; }
        if (HToggle.isOn) { useAI = false; }
    }

    public void MenuShow()
    {
        MainMenu.SetActive(true);
        if (startup) StartText.text ="Resume";
        MainCamera.transform.position = new Vector3(0, 5, -88);
    }

    public void MenuDismiss()
    {
        menuAnimation.Play("Menu_ZoomOut");
        StartCoroutine(OpenandZoom());
    }

    private IEnumerator OpenandZoom()
    {
        yield return new WaitForSeconds(0.50f);
        MainMenu.SetActive(false);

        if (!startup) 
        {
            MainCamera.transform.position = CameraStart.position;
            yield return new WaitForSeconds(1.0f);
            OpenDoor = true;
        }
        else MainCamera.transform.position = CameraMount.position;
    }

    public void StartupSequence()
    {
        if (soundMute) AudioListener.volume = 0.0f;

        GameStart();
        MainCanvas.SetActive(true);
        Messages.SetActive(true);
        startup = true;

        StartCoroutine(GamePlay()); 
    }


    public void GameStart()
    {
        //place two rows of pieces for players
        for (int player = 1; player < 3; player++)
        {
            float xOffset = 0.0f;
            if (player == 1) xOffset = -1.0f;
            if (player == 2) xOffset = 1.0f;

            for (int pieces = 1; pieces < 8; pieces++)
            {
                float Px = xOffset * ((Mathf.CeilToInt(pieces / 3)) + 3);
                float Py = (pieces % 3) + 3;
                float Pz = 0.0f;

                GP.Retrieve(Px, Py, Pz, player, 0);
            }
        }

        for (int g = 0; g < 3; g++) //playfield 3 pieces
        {
            float Gx = g - 1;
            float Gy = 5.0f;
            GP.Retrieve(Gx, Gy, 0.5f, 3, g + 1);
        }
    }

    private IEnumerator GamePlay()
    {
        yield return new WaitForSeconds(1.0f);
        playTurn = 1;
        BG.StartMusic();

        while (true)
        {
            piece1 = null; piece2 = null;
            AImove1 = null; AImove2 = null;

            confirmedMove = false;
            pieceAdded = false;
            turnSkip = false;

            round.text = roundCount.ToString();
            piecesLeft.text = gpTracking.Count.ToString();

            if (playTurn == 1)
            {
                message.text = "Ready Player 1";
                message.color = Color.white;
                Player1.color = Color.yellow;
                Player2.color = Color.gray;
            }
            else if (playTurn == 2)
            {
                message.text = "Ready Player 2";
                message.color = Color.white;
                Player2.color = Color.yellow;
                Player1.color = Color.gray;
            }

            //setting for AI (player 2) - AI move generation
            if (playTurn == 2 && useAI) AI.p2Moves();
            else { clickable = true; playable = true; }

            //Change Button text when max is reached
            if (playTurn == 1 && p1_count >= BC.pieceMax)
            { PlayOption.text = "SKIP"; }
            else if (playTurn == 1 && p1_count < BC.pieceMax)
            { PlayOption.text = "DRAW"; }

            if (playTurn == 2 && p2_count >= BC.pieceMax)
            { PlayOption.text = "SKIP"; }
            else if (playTurn == 2 && p2_count < BC.pieceMax)
            { PlayOption.text = "DRAW"; }

            //get piece1 and piece2 or draw a piece if any remain
            while (!piece1 && !pieceAdded && !turnSkip && !bagEmpty || !piece2 && !pieceAdded && !turnSkip && !bagEmpty)
            {
                int pieceType = clicked ? clicked.GetComponent<Interaction>().gpType : 0; //new 04-22

                if (clicked)
                {
                    //reset shaders when user changes selection
                    if (piece1 && pieceType != 3) piece1.GetComponent<Renderer>().material.shader = Shader.Find("Bumped Specular");
                    if (piece2 && pieceType == 3) piece2.GetComponent<Renderer>().material.shader = Shader.Find("Bumped Specular");              

                    if (pieceType == 1 && playTurn == 1 || pieceType == 2 && playTurn == 2) //player 1 or 2 piece
                    {
                        if (clicked == piece1) 
                        {
                            clicked = null; piece1 = null; //deselected
                        }
                        else
                        {
                            piece1 = clicked; clicked = null; //selected
                            piece1.GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Bumped Specular");
                        }
                    }
                    else if (pieceType == 3) //gamefield piece
                    {
                        if (clicked == piece2) 
                        {
                            clicked = null; piece2 = null; //deselected
                        }
                        else
                        {
                            piece2 = clicked; clicked = null; //selected
                            piece2.GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Bumped Specular");
                        }
                    }
                }

                //process AI moves if AI enabled
                if (useAI == true && playTurn == 2 && AImove1 && AImove2)
                {
                    yield return new WaitForSeconds(1);
                    piece1 = AImove1;
                    piece1.GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Bumped Specular");
                    yield return new WaitForSeconds(1);
                    piece2 = AImove2;
                    piece2.GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Bumped Specular");
                }
                else if (useAI == true && playTurn == 2 && !AImove1 && !AImove2)
                {
                    yield return new WaitForSeconds(2);
                    BC.ClickedButton();
                    yield return new WaitForSeconds(0.25f);
                }

                yield return new WaitForSeconds(0.01f);
            }

            clickable = false; playable = false;

            yield return new WaitForSeconds(0.25f);

            if (piece1 && piece2)
            {
                piece1.GetComponent<Renderer>().material.shader = Shader.Find("Bumped Specular");
                piece2.GetComponent<Renderer>().material.shader = Shader.Find("Bumped Specular");
            }
            else if (piece1 && !piece2)
            {
                piece1.GetComponent<Renderer>().material.shader = Shader.Find("Bumped Specular");
            }
            else if (!piece1 && piece2)
            {
                piece2.GetComponent<Renderer>().material.shader = Shader.Find("Bumped Specular");
            }

            if (!pieceAdded && !bagEmpty && !turnSkip)
            {
                confirmedMove = PV.CheckMove(piece1, piece2);
            }

            if (pieceAdded)
            {
                //track new pieces total count for both players
                if (playTurn == 1) { p1_count = p1_count + 1; }
                else if (playTurn == 2) { p2_count = p2_count + 1; }

                BG.ChangeMusic();

                yield return new WaitForSeconds(1);

                if (gpTracking.Count == 0)
                {
                    bagEmpty = true;
                    piecesLeft.text = " " + gpTracking.Count + " ";
                }
            }

            if (confirmedMove)
            {
                int piece1Number = piece1.GetComponent<Interaction>().gpNumber - 1;
                int piece2Number = piece2.GetComponent<Interaction>().gpNumber - 1;
                FX.PlayTones(piece1Number, piece2Number);

                //track new pieces total count for both players
                if (playTurn == 1) { p1_count = p1_count - 1; }
                else if (playTurn == 2) { p2_count = p2_count - 1; }

                BG.ChangeMusic();

                //track new pieces column counts Player 1
                if (playTurn == 1 && piece1.transform.position.x == -3) { BC.p1c1Count = BC.p1c1Count - 1; }
                else if (playTurn == 1 && piece1.transform.position.x == -4) { BC.p1c2Count = BC.p1c2Count - 1; }
                else if (playTurn == 1 && piece1.transform.position.x == -5) { BC.p1c3Count = BC.p1c3Count - 1; }


                //track new pieces column counts Player 2
                if (playTurn == 2 && piece1.transform.position.x == 3) { BC.p2c1Count = BC.p2c1Count - 1; }
                else if (playTurn == 2 && piece1.transform.position.x == 4) { BC.p2c2Count = BC.p2c2Count - 1; }
                else if (playTurn == 2 && piece1.transform.position.x == 5) { BC.p2c3Count = BC.p2c3Count - 1; }

                yield return new WaitForSeconds(2);
            }

            if (pieceAdded && playTurn == 1 || confirmedMove && playTurn == 1) { P1Skip = false; }
            else if (pieceAdded && playTurn == 2 || confirmedMove && playTurn == 2) { P2Skip = false; }

            if (turnSkip && playTurn == 1) { P1Skip = true; }
            else if (turnSkip && playTurn == 2) { P2Skip = true; }


            if (P1Skip && P2Skip)
            { stalemate = true; }


            if (confirmedMove || pieceAdded || turnSkip)
            {
                if (playTurn == 1) { playTurn = 2; }
                else if (playTurn == 2) { playTurn = 1; roundCount++; }
            }
            else
            {
                yield return new WaitForSeconds(1);
                message.color = Color.white;
            }


            if (bagEmpty)
            {
                message.text = " Last Piece Pulled \nGAME OVER! ";
                yield return new WaitForSeconds(2);
            }

            if (stalemate)
            {
                message.text = " STALEMATE! ";
                yield return new WaitForSeconds(2);
            }


            if (p1_count == 0 || p2_count == 0 || bagEmpty || stalemate)
            {
                message.text = " GAME OVER ";
                StartCoroutine(GameEnd());
                yield return new WaitForSeconds(2);
                break;
            }
        }
    }

    private IEnumerator GameEnd()
    {
        gameOver = true;

        BG.StopMusic();

        if (p1_count < p2_count)
        {
            winText.text = "Player 1\nWins";
        }
        else if (p2_count < p1_count)

        {
            winText.text = "Player 2\nWins";
        }
        else
        {
            winText.text = "Its a\nDraw";
        }

        yield return new WaitForSeconds(1);

        Player1.text = ""; Player2.text = "";

        GameObject[] Pieces = GameObject.FindGameObjectsWithTag("GamePiece");

        foreach (GameObject GamePiece in Pieces)
        {
            Destroy(GamePiece);
        }

        yield return new WaitForSeconds(1);

        //winStar.SetActive(true);

        restart = true;
    }

    public void RestartGame()
    {
        //Get current scene name
        string scene = SceneManager.GetActiveScene().name;
        //Load it
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        #if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_STANDALONE) 
            Application.Quit();
        #endif
    }

}
