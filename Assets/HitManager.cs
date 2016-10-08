using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HitManager : MonoBehaviour {

    public BatonHit center;
    public BatonHit left;
    public BatonHit bottom;
    public BatonHit right;
    public BatonHit top;

    private int beatsPerMeasure = 4;

    private AudioSource audioSource;
    public AudioClip beat;

    public Text tutorialText;

    public const string TUTORIAL_START = "Welcome to Conduct VR! \n begin, move your baton to the green panel.";
    public const string TUTORIAL_01 = "Congrats! You just conducted your first beat! Try it one more time!";
    public const string TUTORIAL_02 = "Great! The bottom panel will be touched every beat of the song. \n Follow this beat!";
    public const string TUTORIAL_03 = "Good job! Now, for the other panels! \n Try touching the panels when they light up!";
    public const string TUTORIAL_04 = "Nicely done! This is how you conduct a regular song! \n Now try it with this slow beat!";
    public const string TUTORIAL_05 = "Let's go a little faster!";
    public const string TUTORIAL_06 = "Even faster!";
    public const string TUTORIAL_07 = "Great! You're ready for your first song! \n The song will only play correctly if you stay on beat!";

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        beatsPerMeasure = SongDriver.instance.activeSong.beatsInMeasure;
        StartCoroutine(highlightBeats());

        bottom.panelColor = Color.green;
        center.panelColor = Color.magenta;
        left.panelColor = Color.blue;
        right.panelColor = Color.red;
        top.panelColor = Color.yellow;
    }

    // Update is called once per frame
    void Update () {
	
	}

    public IEnumerator highlightBeats() {

        while(true) {

            float beatsPerSecond = SongDriver.instance.activeSong.bpm / 60.0f;
            float timeBetweenBeats = 1.0f / beatsPerSecond;

            bottom.ColorHit();

            yield return new WaitForSeconds(timeBetweenBeats / 2.0f);

            center.ColorHit();

            yield return new WaitForSeconds(timeBetweenBeats / 2.0f);

            bottom.ColorHit();

            yield return new WaitForSeconds(timeBetweenBeats / 2.0f);

            if (beatsPerMeasure > 3) {

                left.ColorHit();

                yield return new WaitForSeconds(timeBetweenBeats / 2.0f);
                bottom.ColorHit();

                yield return new WaitForSeconds(timeBetweenBeats / 2.0f);
            }

            right.ColorHit();

            yield return new WaitForSeconds(timeBetweenBeats / 2.0f);

            bottom.ColorHit();

            yield return new WaitForSeconds(timeBetweenBeats / 2.0f);

            top.ColorHit();

            yield return new WaitForSeconds(timeBetweenBeats / 2.0f);

        }
    }

    public IEnumerator tutorialSequence() {

       tutorialText.text = TUTORIAL_START;
       yield return StartCoroutine(bottom.WaitForBatonTouch());
       tutorialText.text = TUTORIAL_01;
        yield return StartCoroutine(bottom.WaitForBatonTouch());
        tutorialText.text = TUTORIAL_02;


    }
}
