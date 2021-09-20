using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;
    private void Awake()
    {
        instance = this;
    }

    public static GameAssets GetInstance()
    {
        return instance;
    }

    

    public Sprite pipeHeadSprite;
    public Transform pfPipeHead;
    public Transform pfPipeBody;

    

    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }
    public SoundAudioClip[] soundAudioClipArray;
}
