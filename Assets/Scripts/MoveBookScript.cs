using UnityEngine;
using System.Collections;

public class MoveBookScript : MonoBehaviour {
    Vector3 newBookPos = new Vector3(4.5f, 1.9f, 1.9f);
    Vector3 newBookScale = new Vector3(.00315f, .00315f, .00235f);

	// Use this for initialization
	void Start () {
        GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        RectTransform bookRect = GetComponent<RectTransform>();

        bookRect.position = newBookPos;
       // bookRect.localScale = newBookScale;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
