using UnityEngine;
using System.Collections;

[System.Serializable]
public class SongDriver : MonoBehaviour {

    public static SongDriver instance;

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
	}
	
	// Update is called once per frame
	void Update () {


        timeSinceLastHit += Time.deltaTime;

	}

    public void BeatHit() {

		Debug.Log ("HIT");

        float playerBPM = 60.0f / timeSinceLastHit;
        float actualBPM = activeSong.bpm;

        float bpmRatio = playerBPM / actualBPM;

        source.pitch = bpmRatio;

        this.timeSinceLastHit = 0.0f;
    }
}