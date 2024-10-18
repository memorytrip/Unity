using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class AudioManager: MonoBehaviour
    {
        public static AudioManager Instance;

        private List<AudioSource> sfxPlayers;
        private AudioSource bgmPlayer;
        private const int baseSfxPlayersCount = 8;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
            DontDestroyOnLoad(gameObject);
            
            InitPlayers();
        }

        private void InitPlayers()
        {
            sfxPlayers = new List<AudioSource>(baseSfxPlayersCount);
            for (int i = 0; i < baseSfxPlayersCount; i++)
            {
                sfxPlayers.Add(gameObject.AddComponent<AudioSource>());
            }
            bgmPlayer = gameObject.AddComponent<AudioSource>();
            bgmPlayer.loop = true;
        }

        public void PlayBGM(AudioClip audioClip)
        {
            if (bgmPlayer.isPlaying) bgmPlayer.Stop();
            bgmPlayer.clip = audioClip;
            bgmPlayer.Play();
        }

        public void StopBGM()
        {
            bgmPlayer.Stop();
        }

        public void PlaySFX(AudioClip audioClip)
        {
            AudioSource audioSource = sfxPlayers.Find((e) => !e.isPlaying);
            if (audioSource == null)
            {
                Debug.LogWarning("Cannot find empty sfxPlayer");
                return;
            }
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}