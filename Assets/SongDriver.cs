using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class SongDriver : MonoBehaviour {

    public BatonHit bottom;
    public HitManager hitManager;

    public static SongDriver instance;

    public float marginOfError = 15;

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
    private int songIndex = 0;

    public Song activeSong;

    public Text bpmText;

    private const string statusString = "Your BPM: {0}\n Target BPM: {1}\n Relative Speed: x{2}";

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

        string updatedString = string.Format(statusString, (int)activeBPM, activeSong.bpm, GetBPMRatio());
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

    public void StopAndReset() {

        source.Stop();
        StopAllCoroutines();
        songActive = false;
        StartCoroutine(waitForStart());

    }

    public void PlayNextSong() {

        songIndex++;

        if (songIndex >= songs.Length) {
            songIndex = 0;
        }

        activeSong = songs[songIndex];
        source.Play();
        StartCoroutine(checkMissedBeats());
        StartCoroutine(hitManager.highlightBeats());
        StartCoroutine(lerpMusic());
        songActive = true;

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

        PlayNextSong();

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