using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager:MonoBehaviour
{
    [SerializeField] MusicConfig musicConfig;
    private AudioSource audioSource;
    //public List<ObjectBase> Object;
    //public List<ObjectBase> TriggerObject;

    public  void Init()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        SetVolum(1.0f);


        //Object = new List<ObjectBase>();
        //TriggerObject = new List<ObjectBase>();
    }

    public void PlayRandomAmbientHorror()
    {
        PlayRandomAudioClip(musicConfig.amb_horror);
    }

    public void PlayRandomGhostMovement()
    {
        PlayRandomAudioClip(musicConfig.ghost_movement);
    }

    public void PlayRandomGhostCast()
    {
        PlayRandomAudioClip(musicConfig.ghost_cast);
    }

    public void PlayRandomHumanFootstep()
    {
        PlayRandomAudioClip(musicConfig.human_footstep);
    }

    public void PlayRandomWoodMovement()
    {
        PlayRandomAudioClip(musicConfig.wood_movement);
    }

    public void PlayRandomBookPageTurn()
    {
        PlayRandomAudioClip(musicConfig.book_page_turn);
    }

    public void PlayRandomBulbEvilBreak()
    {
        PlayRandomAudioClip(musicConfig.bulb_evil_break);
    }

    public void PlayRandomFrameBroken()
    {
        PlayRandomAudioClip(musicConfig.frame_broken);
    }

    public void PlayRandomHumanHorror()
    {
        PlayRandomAudioClip(musicConfig.human_horror);
    }

    public void PlayRandomMusicIntro()
    {
        PlayRandomAudioClip(musicConfig.music_intro);
    }
    
    public void PlayRandomTelephoneRing()
    {
        PlayRandomAudioClip(musicConfig.telephone_ring);
    }
    
    public void PlayRandomHumanDead()
    {
        PlayRandomAudioClip(musicConfig.human_death);
    }

    public void PlayRandomMusicMain()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(musicConfig.music_main[Random.Range(0, musicConfig.music_main.Length)]);
    }

    private void PlayRandomAudioClip(AudioClip[] clips)
    {
        if (clips != null && clips.Length > 0)
        {
            audioSource.Stop();
            AudioClip randomClip = clips[Random.Range(0, clips.Length)];
            audioSource.PlayOneShot(randomClip);
        }
    }

    public void SetLoop()
    {
        audioSource.loop = true;
    }

    public void SetVolum(float v)
    {
        audioSource.volume = v;
    }
}
