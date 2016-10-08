using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SongDriver : MonoBehaviour {

    public static SongDriver instance;

    private Queue<float> storedBeats;
    public int numberOfBeatsToAverageAcross;

    [System.Serializable]
    public struct Song {
        public string songName;
        public AudioClip songAudio;
        public int bpm;
        public int beatsInMeasure;
        public int noteGetsBeat;
    }

    public Song[] songs;

    private Song activeSong;

    private AudioSource source;

    private float timeSinceLastHit;

    private float activeBPM;

    public void Awake() {

        if (SongDriver.instance == null) {
            SongDriver.instance = this;
        }

    }

	// Use this for initialization
	void Start () {

        source = GetComponent<AudioSource>();
        activeSong = songs[0];
        source.clip = activeSong.songAudio;
		source.Play ();

        storedBeats = new Queue<float>();
        for (int i = 0; i < numberOfBeatsToAverageAcross; i++) {

            storedBeats.Enqueue(activeSong.bpm);

        }
    }

    // Update is called once per frame
    void Update() {

        // Calculate time passed since the last baton hit.
        timeSinceLastHit += Time.deltaTime;

        // This is the player's current active BPM.
        // Note: Between hits, this will be very high.
        activeBPM = 60.0f / timeSinceLastHit;

        // THis is what the song's BPM SHOULD be.
        float actualBPM = activeSong.bpm;

        // If at any point the player's BPM is slower (i.e. they missed a hit)
        if(activeBPM < actualBPM) {

            UpdateSongPitchByBPM();

        }
	}

    // When the player hits, force set the last bpm
    public void BeatHit() {

        UpdateSongPitchByBPM();

        StartNewBeat();

    }

    public void UpdateSongPitchByBPM() {

        activeBPM = 60.0f / timeSinceLastHit;

        float averageBPM = GetAverageBPM();

        source.pitch = averageBPM / activeSong.bpm;

    }

    public void StartNewBeat() {
        storedBeats.Dequeue();
        storedBeats.Enqueue(activeBPM);

        // Reset the time since the last hit
        timeSinceLastHit = 0.0f;

    }

    // Gets the ratio of the player's BPM to the actual BPM
    public float GetBPMRatio() {

        // Calculate player BPM on the hit
        float playerBPM = 60.0f / timeSinceLastHit;
        float actualBPM = activeSong.bpm;

        float bpmRatio = playerBPM / actualBPM;

        return bpmRatio;

    }
    
    public float GetAverageBPM() {

        float totalBPM = 0.0f;

        foreach (float bpm in storedBeats) {

            totalBPM += bpm;

        }

        totalBPM += activeBPM;

        totalBPM /= (numberOfBeatsToAverageAcross + 1);

        return totalBPM;

    }
}