using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_FX : MonoBehaviour
{
    public BG_Music BGM;
    public AudioSource[] myTones = new AudioSource[2];
    public AudioClip[] toneStorage = new AudioClip[7];

    public void PlayTones(int tone1, int tone2)
    {
        float clipRoundUp = Mathf.Ceil(BGM.clipTime);
        float toHalfNote = clipRoundUp % 2;
        float waitToPlay = clipRoundUp - BGM.clipTime + toHalfNote;

        myTones[0].clip = toneStorage[tone1];
        myTones[0].PlayScheduled(AudioSettings.dspTime + waitToPlay);
        myTones[1].clip = toneStorage[tone2];
        myTones[1].PlayScheduled(AudioSettings.dspTime + waitToPlay + 0.25f);
    }

    void VolumesUpdated()
    {
        for (int i = 0; i < 2; i++)
        {
            myTones[i].volume = GetComponent<AudioSource>().volume;
        }
    }
}
