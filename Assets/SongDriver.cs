using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class SongDriver : MonoBehaviour {

    public BatonHit bottom;
    public HitManager hitManager;

    public static SongDriver instance;

    public Text songText;


    public float marginOfError = 20;

    [System.Serializable]
    public struct Song {
        public string songName;
        public AudioClip songAudio;
        public int bpm;
        public int beatsInMeasure;
        public int noteGetsBeat;
    }

    private bool songActive = false;

    public Song[] songs;
    private int songIndex = -1;

    public Song activeSong;

    public Text bpmText;

    private const string statusString = "Your BPM: {0}\n Target BPM: {1}";

    private AudioSource source;

    private float timeSinceLastHit;

    private float activeBPM;

    private float roundedBPM;

    public void Awake() {

        if (SongDriver.instance == null) {
            SongDriver.instance = this;
        }

        source = GetComponent<AudioSource>();
        activeSong = songs[0];
        source.clip = activeSong.songAudio;

    }

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update() {

        if (songActive) {
            // Calculate time passed since the last baton hit.
            timeSinceLastHit += Time.deltaTime;

            UpdateText();

            if (!source.isPlaying) {
                StopAndReset();
            }
        }
    }

    public void UpdateText() {

        string updatedString = string.Format(statusString, (int)activeBPM, activeSong.bpm);
        bpmText.text = updatedString;

    }

    // When the player hits, force set the last bpm
    public void BeatHit() {

        float bps = 1 / timeSinceLastHit;

        activeBPM = bps * 60;

        RoundBPM();

        StartNewBeat();

    }

    public void RoundBPM() {

        float actualBPM = activeSong.bpm;

        if (activeBPM >= actualBPM - marginOfError && activeBPM <= actualBPM + marginOfError) {

            roundedBPM = actualBPM;

        }
        else {

            roundedBPM = activeBPM;

        }
    }

    public void StopAndReset(bool previous = false) {

        source.Stop();
        StopAllCoroutines();
        hitManager.StopAllCoroutines();
        songActive = false;
        PlayNextSong(previous);


    }

    public void PlayNextSong(bool previous) {


        if(!previous) {

            songIndex++;

        }
        else {
            songIndex--;
        }

        if(songIndex >= songs.Length) {
            songIndex = 0;
        }
        if (songIndex < 0) {
            songIndex = songs.Length - 1;
        }

        songText.text = "Current Song: " + songs[songIndex].songName;

        StartCoroutine(waitForStart());
    }

    public IEnumerator checkMissedBeats() {

        while (true) {

            // THis is what the song's BPM SHOULD be.
            float actualBPM = activeSong.bpm;

            float bps = 1 / timeSinceLastHit;

            float bpm = bps * 60.0f;

            if (bpm < actualBPM) {

                activeBPM = bpm;

                RoundBPM();

            }

            yield return null;
        }
    }
    
    public IEnumerator waitForStart() {

        yield return StartCoroutine(bottom.WaitForBatonTouch());

        StartCoroutine(checkMissedBeats());
        StartCoroutine(hitManager.highlightBeats());
        StartCoroutine(lerpMusic());

         
        activeSong = songs[songIndex];
        source.clip = activeSong.songAudio;
        source.Play();
        songActive = true;


    }

    public void StartNewBeat() {

        // Reset the time since the last hit
        timeSinceLastHit = 0.0f;

    }

    // Gets the ratio of the player's BPM to the actual BPM
    public float GetBPMRatio() {

        // Calculate player BPM on the hit
        float playerBPM = activeBPM;
        float actualBPM = activeSong.bpm;

        float bpmRatio = playerBPM / actualBPM;

        return bpmRatio;

    }
    

    public IEnumerator lerpMusic() {

        while (true) {

            float currentPitch = source.pitch;
            float bpmPitch = (activeBPM / activeSong.bpm);

            if(roundedBPM >= activeSong.bpm - marginOfError && roundedBPM <= activeSong.bpm + marginOfError) {

                source.pitch = 1;

            }

            else {

                for(float i = 0.0f; i < 0.4f; i += Time.deltaTime) {

                    if(roundedBPM >= activeSong.bpm - marginOfError && roundedBPM <= activeSong.bpm + marginOfError) {

                        break;

                    }

                    source.pitch = Mathf.Lerp(currentPitch, bpmPitch, i);

                }
            }

            yield return null;

        }
    }
}