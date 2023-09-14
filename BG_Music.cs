using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Music : MonoBehaviour
{
    public GameController GC;

    public GameObject winStar;

    public AudioSource myBeats;
    public AudioClip[] beatStorage = new AudioClip[4];

    public float clipTime =0.0f;

    public int lowestCount = 0;
    public int currentStage = 0;
    public int previousStage = 0;

    private bool started = false;
    private bool stopping = false;
    private bool stopped = false;

    void Start()
    {
        myBeats.clip = beatStorage[currentStage];
    }

    void Update()
    {
        //GC.gameOver
        if (!started || stopped) return;

        clipTime = myBeats.time;

        if ( clipTime < 0.1f )
        {
            if (currentStage != previousStage) SwapAudio(); 
            if (stopping) StopAudio();
        }
    }

    public void StartMusic()
    {
        myBeats.Play();
        started = true;
    }

    public void StopMusic()
    {
        stopping = true;
        stopped = false;
    }

    public void ChangeMusic()
    {
        lowestCount = GC.p1_count < GC.p2_count ? GC.p1_count : GC.p2_count;

        if (lowestCount > 4) currentStage = 0;
        else if (lowestCount <= 4 && lowestCount >= 3) currentStage = 1;
        else if (lowestCount < 3) currentStage = 2;
    }

    public void SwapAudio()
    {
        myBeats.clip = beatStorage[currentStage];
        myBeats.Play();

        previousStage = currentStage;
    }

    public void StopAudio()
    {
        myBeats.loop = false; 
        myBeats.clip = beatStorage[3];
        myBeats.Play();
        stopped = true;

        winStar.SetActive(true);
    }
}
