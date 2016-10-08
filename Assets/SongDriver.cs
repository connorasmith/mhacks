using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

    public Song activeSong;

    public Text bpmText;

    private const string statusString = "Your BPM: {0}\n Target BPM: {1}\n Relative Speed: x{2}";

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

        StartCoroutine(checkMissedBeats());
    }

    // Update is called once per frame
    void Update() {

        // Calculate time passed since the last baton hit.
        timeSinceLastHit += Time.deltaTime;

        activeBPM = 60.0f / timeSinceLastHit;

        UpdateText();
	}

    public void UpdateText() {

        string updatedString = string.Format(statusString, (int)GetAverageBPM(), activeSong.bpm, GetBPMRatio());
        bpmText.text = updatedString;


    }

    // When the player hits, force set the last bpm
    public void BeatHit() {

        UpdateSongPitchByBPM();

        StartNewBeat();

    }

    public IEnumerator checkMissedBeats() {

        while(true) {

            // THis is what the song's BPM SHOULD be.
            float actualBPM = activeSong.bpm;

            // If at any point the player's BPM is slower (i.e. they missed a hit)
            if(activeBPM < actualBPM) {

                UpdateSongPitchByBPM();

                yield return new WaitForSeconds(3.0f);

                if(activeBPM < actualBPM) {

                    storedBeats.Dequeue();
                    storedBeats.Enqueue(activeBPM);

                }

            }

            yield return null;
        }
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
        float playerBPM = GetAverageBPM();
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