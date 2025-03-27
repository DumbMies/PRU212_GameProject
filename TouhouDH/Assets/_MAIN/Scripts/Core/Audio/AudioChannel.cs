using COMMANDS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioChannel 
{
    private const string TRACK_CONTAINER_NAME_FORMAT = "Channel - [{0}]";
    public int channelIndex { get; private set; }

    public Transform trackContainer { get; private set; } = null;

    public AudioTrack activeTrack
    {
        get;
        private set;
    } = null;

    private AudioTrack loopTrack = null;

    private List<AudioTrack> tracks = new List<AudioTrack>();

    bool isLevelingVolume => co_volumeLeveling != null;
    Coroutine co_volumeLeveling = null;
    Coroutine co_waitAndPlayLoopTrack = null;

    public AudioChannel(int channel)
    {
        channelIndex = channel;

        trackContainer = new GameObject(string.Format(TRACK_CONTAINER_NAME_FORMAT, channel)).transform;
        trackContainer.SetParent(AudioManager.instance.transform);
    }

    public AudioTrack PlayTrack(AudioClip clip, bool loop, float startingVolume, float volumeCap, float pitch, string filePath)
    {
        if (TryGetTrack(clip.name, out AudioTrack existingTrack))
        {
            if (!existingTrack.isPlaying)
            {
                existingTrack.Play();
            }
            SetAsActiveTrack(existingTrack);

            return existingTrack;
        }

        AudioTrack track = new AudioTrack(clip, loop, startingVolume, volumeCap, pitch, this, AudioManager.instance.musicMixer, filePath);
        track.Play();

        SetAsActiveTrack(track);


        return track;
    }

    public AudioTrack PlayContinuousTrack(AudioClip initialClip, bool loop, float startingVolume, float volumeCap, float pitch, string filePath)
    {
        filePath = filePath + " - B";

        AudioClip loopClip = Resources.Load<AudioClip>(filePath);
        
        if (TryGetTrack(initialClip.name, out AudioTrack existingTrack))
        {
            if (!existingTrack.isPlaying)
            {
                existingTrack.Play();
            }
            SetAsActiveTrack(existingTrack);

            existingTrack.isContinuousTrack = true;

            string loopClipName = initialClip.name + " - B";

            //Check existing for loop track.
            if(TryGetTrack(loopClipName, out AudioTrack existingLoopTrack))
            {
                float initialTrackLength = initialClip.length / pitch;

                if (co_waitAndPlayLoopTrack != null)
                {
                    AudioManager.instance.StopCoroutine(co_waitAndPlayLoopTrack);
                }
                co_waitAndPlayLoopTrack = AudioManager.instance.StartCoroutine(WaitAndPlayLoopTrack(existingLoopTrack, initialTrackLength));
            }
            else
            {
                float initialTrackLength = initialClip.length / pitch;

                if (co_waitAndPlayLoopTrack != null)
                {
                    AudioManager.instance.StopCoroutine(co_waitAndPlayLoopTrack);
                }
                co_waitAndPlayLoopTrack = AudioManager.instance.StartCoroutine(WaitAndPlayLoopTrack(loopClip, initialTrackLength, loop, startingVolume, volumeCap, pitch, filePath));
            }    

            return existingTrack;
        }

        AudioTrack initialTrack = new AudioTrack(initialClip, loop, startingVolume, volumeCap, pitch, this, AudioManager.instance.musicMixer, filePath);
        initialTrack.Play();

        SetAsActiveTrack(initialTrack);

        initialTrack.isContinuousTrack = true;

        if (initialTrack != null)
        {
            float initialTrackLength = initialClip.length / pitch;

            if (co_waitAndPlayLoopTrack != null)
            { 
                AudioManager.instance.StopCoroutine(co_waitAndPlayLoopTrack);
            }
            co_waitAndPlayLoopTrack = AudioManager.instance.StartCoroutine(WaitAndPlayLoopTrack(loopClip, initialTrackLength, loop, startingVolume, volumeCap, pitch, filePath));
        }

        return initialTrack;
    }

    private IEnumerator WaitAndPlayLoopTrack(AudioClip loopClip, float delay, bool loop, float startingVolume, float volumeCap, float pitch, string filePath)
    {
        
        yield return new WaitForSeconds(delay);

        loopTrack = new AudioTrack(loopClip, true, startingVolume, volumeCap, pitch, this, AudioManager.instance.musicMixer, filePath);
        loopTrack.Play();

        loopTrack.volume = volumeCap;

    }

    private IEnumerator WaitAndPlayLoopTrack(AudioTrack loopTrack, float delay)
    {
        yield return new WaitForSeconds(delay);

        this.loopTrack = loopTrack;
        Debug.Log("I've come here BAKA. And the loop track is " + loopTrack.name + ", ");
        this.loopTrack.Play();

        this.loopTrack.volume = loopTrack.volumeCap;

    }



    public bool TryGetTrack(string trackName, out AudioTrack value)
    {
        trackName = trackName.ToLower();

        foreach(var track in tracks)
        {
            if (track.name.ToLower() == trackName)
            {
                value = track;
                return true;
            }
        }

        value = null;
        return false;
    }

    private void SetAsActiveTrack(AudioTrack track)
    {
        if (!tracks.Contains(track))
        {
            tracks.Add(track);
        }

        activeTrack = track;

        Debug.Log("Set active " + track.name);

        TryStartVolumeLeveling();
    }

    private void TryStartVolumeLeveling()
    {
        if (!isLevelingVolume)
        {
            co_volumeLeveling = AudioManager.instance.StartCoroutine(VolumeLeveling());
        }
    }

    private IEnumerator VolumeLeveling()
    {
        while((activeTrack != null && (tracks.Count > 1 || activeTrack.volume != activeTrack.volumeCap)) || (activeTrack == null & tracks.Count > 0))
        {
            for (int i = tracks.Count - 1; i >= 0; i--)
            {
                AudioTrack track = tracks[i];

                float targetVol = activeTrack == track ? track.volumeCap : 0;

                if (track == activeTrack && track.volume == targetVol)
                {
                    continue;
                }

                track.volume = Mathf.MoveTowards(track.volume, targetVol, AudioManager.TRACK_TRANSITION_SPEED * Time.deltaTime);

                if (track != activeTrack && track.volume == 0)
                {
                    DestroyTrack(track);
                }
            }
            yield return null;
        }

        co_volumeLeveling = null;
    }

    private IEnumerator LoopTrackVolumeLeveling()
    {
        while (loopTrack != null && loopTrack.volume != 0)
        {
            AudioTrack track = loopTrack;

            track.volume = Mathf.MoveTowards(track.volume, 0, AudioManager.TRACK_TRANSITION_SPEED * Time.deltaTime);

            //if (track == loopTrack && track.volume == 0)
            //{
            //    DestroyTrack(track);
            //}
            
            yield return null;
        }
        loopTrack = null;
        co_volumeLeveling = null;
    }

    private void DestroyTrack(AudioTrack track)
    {
        if (tracks.Contains(track))
        {
            tracks.Remove(track);
        }
        Object.Destroy(track.root);
    }

    public void StopTrack(bool immediate = false)
    {
        if (activeTrack == null)
        {
            return;
        }

        if (co_waitAndPlayLoopTrack != null)
        {
            AudioManager.instance.StopCoroutine(co_waitAndPlayLoopTrack);
            co_waitAndPlayLoopTrack = null;
        }

        if (loopTrack != null)
        {
            if (!isLevelingVolume)
            {
                co_volumeLeveling = AudioManager.instance.StartCoroutine(LoopTrackVolumeLeveling());
            }
        }

        if (immediate)
        {
            activeTrack.isContinuousTrack = false;
            DestroyTrack(activeTrack);
            activeTrack = null;
        }
        else
        {
            activeTrack.isContinuousTrack = false;
            activeTrack = null; 
            TryStartVolumeLeveling();
        }
    }

    //public void StopTrack(bool immediate = false)
    //{
    //    if (co_waitAndPlayLoopTrack != null)
    //    {
    //        AudioManager.instance.StopCoroutine(co_waitAndPlayLoopTrack);
    //        co_waitAndPlayLoopTrack = null;
    //    }

    //    if (loopTrack != null)
    //    {
    //        if (!isLevelingVolume)
    //        {
    //            co_volumeLeveling = AudioManager.instance.StartCoroutine(LoopTrackVolumeLeveling());
    //        }
    //    }

    //    activeTrack.isContinuousTrack = false;
    //    if (activeTrack == null)
    //    {
    //        return;
    //    }
    //    if (immediate)
    //    {
    //        DestroyTrack(activeTrack);
    //        activeTrack = null;
    //    }
    //    else
    //    {
    //        activeTrack = null;
    //        TryStartVolumeLeveling();
    //    }
    //}
}
