using UnityEngine;
using System.Collections;

public class BatonHit : MonoBehaviour {

    private Material highlightMaterial;
    public bool isBottom;

    private float hitColorDelay = 0.1f;

	// Use this for initialization
	void Start () {

        highlightMaterial = new Material(this.GetComponent<MeshRenderer>().material);
        GetComponent<MeshRenderer>().material = highlightMaterial;

	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void OnTriggerEnter(Collider other) {

        if (other.GetComponent<Baton>() && isBottom) {

            SongDriver.instance.BeatHit();

        }
    }

    public IEnumerator FlashColor(Color hitColor) {

        if(highlightMaterial != null) {

            Color prevColor = highlightMaterial.color;

            highlightMaterial.SetColor("_Color", hitColor);

            yield return new WaitForSeconds(hitColorDelay);

            highlightMaterial.SetColor("_Color", prevColor);
        }

    }

    public void ColorHit(Color hitColor) {

        StartCoroutine(FlashColor(hitColor));

    }

}
