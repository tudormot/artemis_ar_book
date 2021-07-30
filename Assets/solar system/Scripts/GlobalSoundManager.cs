using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalSoundManager : MonoBehaviour {
    public Sprite SoundOn;
    public Sprite SoundOff;
    public Button sndbutton;
    public void SoundManager () {
        AudioListener.volume = 1-AudioListener.volume;
        if (AudioListener.volume==1) sndbutton.GetComponent<UnityEngine.UI.Image>().sprite = SoundOn;
        else sndbutton.GetComponent<UnityEngine.UI.Image>().sprite = SoundOff;
    }
}
