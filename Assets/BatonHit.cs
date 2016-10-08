using UnityEngine;
using System.Collections;

public class BatonHit : MonoBehaviour {

    private Material highlightMaterial;

    private float hitColorDelay = 0.1f;

    private bool waiting = false;
    private bool touched = false;

    public Color panelColor;

    public int timesHit = 0;

	// Use this for initialization
	void Start () {

        highlightMaterial = new Material(this.GetComponent<MeshRenderer>().material);
        GetComponent<MeshRenderer>().material = highlightMaterial;

	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void OnTriggerEnter(Collider other) {

        if (other.GetComponent<Baton>()) {

            SongDriver.instance.BeatHit();

        }

        if (waiting) {

            touched = true;

        }

        timesHit++;
    }

    public IEnumerator FlashColor() {

        if(highlightMaterial != null) {

            Color prevColor = highlightMaterial.color;

            highlightMaterial.SetColor("_Color", panelColor);

            yield return new WaitForSeconds(hitColorDelay);

            highlightMaterial.SetColor("_Color", prevColor);
        }
    }

    public void ColorHit() {

        StartCoroutine(FlashColor());

    }

    public IEnumerator WaitForBatonTouch() {

        waiting = true;

        Color prevColor = highlightMaterial.color;
        highlightMaterial.SetColor("_Color", panelColor);


        while(!touched) {

            yield return null;

        }

        highlightMaterial.SetColor("_Color", prevColor);

        waiting = false;
        touched = false;
    }
}
