using UnityEngine;
using System.Collections;

public class BatonHit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void OnTriggerEnter(Collider other) {

        if (other.GetComponent<Baton>()) {

            SongDriver.instance.BeatHit();

        }
    }
}
