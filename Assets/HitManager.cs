using UnityEngine;
using System.Collections;

public class HitManager : MonoBehaviour {

    public BatonHit center;
    public BatonHit left;
    public BatonHit bottom;
    public BatonHit right;
    public BatonHit top;

    private int beatsPerMeasure = 4;

	// Use this for initialization
	void Start () {
        beatsPerMeasure = SongDriver.instance.activeSong.beatsInMeasure;
        StartCoroutine(highlightBeats());
    }

    // Update is called once per frame
    void Update () {
	
	}

    public IEnumerator highlightBeats() {

        while(true) {

            float beatsPerSecond = SongDriver.instance.activeSong.bpm / 60.0f;
            float timeBetweenBeats = 1.0f / beatsPerSecond;

            bottom.ColorHit(Color.green);

            yield return new WaitForSeconds(timeBetweenBeats / 2.0f);

            center.ColorHit(Color.magenta);

            yield return new WaitForSeconds(timeBetweenBeats / 2.0f);

            bottom.ColorHit(Color.green);

            yield return new WaitForSeconds(timeBetweenBeats / 2.0f);

            if(beatsPerMeasure > 3) {

                left.ColorHit(Color.blue);

                yield return new WaitForSeconds(timeBetweenBeats / 2.0f);
                bottom.ColorHit(Color.green);

                yield return new WaitForSeconds(timeBetweenBeats / 2.0f);

            }

            right.ColorHit(Color.red);

            yield return new WaitForSeconds(timeBetweenBeats / 2.0f);

            bottom.ColorHit(Color.green);

            yield return new WaitForSeconds(timeBetweenBeats / 2.0f);

            top.ColorHit(Color.yellow);

            yield return new WaitForSeconds(timeBetweenBeats / 2.0f);

        }
    }
}
