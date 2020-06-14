using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BackgroundMusic
{

    private static AudioSource backMusic;

    public static void SetBackgroundMusic(AudioSource music)
    {
        if (backMusic != null)
        {
            if (backMusic.clip == music.clip) return;
            GameObject.Destroy(backMusic.gameObject);
        }

        backMusic = music;
        GameObject.DontDestroyOnLoad(backMusic.gameObject);
        backMusic.Play();
    }
    
}
