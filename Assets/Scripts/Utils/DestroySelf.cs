using UnityEngine;
using System.Collections;

public class DestroySelf : MonoBehaviour {
    float start;

	void Start () {
        start = Time.time;
	}
	
	void Update () {
        if (!Application.isPlaying)
            DestroyImmediate(gameObject);
	    if(Time.time - start > 0.1f)
        {
            Destroy(gameObject);
        }
	}
}
