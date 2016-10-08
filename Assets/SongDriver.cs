using UnityEngine;
using System.Collections;

[System.Serializable]
public class SongDriver : MonoBehaviour {

    [System.Serializable]
    public struct Song {
        public string songName;
        public AudioClip songAudio;
        public int bpm;
        public int beatsInMeasure;
        public int noteGetsBeat;
    }

    public Song[] songs;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}