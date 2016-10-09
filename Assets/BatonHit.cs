using UnityEngine;
using System.Collections;

public class BatonHit : MonoBehaviour {

    private Material highlightMaterial;
    public bool isBottom;

    private Color startColor;

    private float hitColorDelay = 0.1f;

    private bool waiting = false;
    private bool touched = false;

    public Color panelColor;

    public int timesHit = 0;

    void Awake() {

        highlightMaterial = new Material(this.GetComponent<MeshRenderer>().material);
        GetComponent<MeshRenderer>().material = highlightMaterial;
        startColor = highlightMaterial.color;


    }

    // Use this for initialization
    void Start () {


	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void OnTriggerEnter(Collider other) {

        if (other.GetComponent<Baton>()) {
           
            if (waiting) {

                Debug.LogWarning("TOUCHED!");
                touched = true;

            }

            if(isBottom) {
                SongDriver.instance.BeatHit();
            }

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
        highlightMaterial.SetColor("_Color", panelColor);
        Debug.LogWarning("PANEL COLOR IS: " + panelColor);

        while(!touched) {

            yield return null;

        }

        highlightMaterial.SetColor("_Color", startColor);

        waiting = false;
        touched = false;
    }
}
