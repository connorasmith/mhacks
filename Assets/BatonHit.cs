using UnityEngine;
using System.Collections;

public class BatonHit : MonoBehaviour {

    private Material highlightMaterial;

    private float hitColorDelay = 0.1f;

	// Use this for initialization
	void Start () {

        highlightMaterial = new Material(this.GetComponent<MeshRenderer>().material);
        GetComponent<MeshRenderer>().material = highlightMaterial;
        StartCoroutine(HighlightBeats());

	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void OnTriggerEnter(Collider other) {

        if (other.GetComponent<Baton>()) {

            SongDriver.instance.BeatHit();

        }
    }

    public IEnumerator ColorHit() {

        highlightMaterial.SetColor("_Color", Color.green);

        yield return new WaitForSeconds(hitColorDelay);

        highlightMaterial.SetColor("_Color", Color.white);

    }

    public IEnumerator HighlightBeats() {

        while (true) {

            StartCoroutine(ColorHit());

            yield return new WaitForSeconds(60.0f / SongDriver.instance.activeSong.bpm);

        }
    }
}
