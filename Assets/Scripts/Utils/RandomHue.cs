using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RandomHue : MonoBehaviour {

	void Start () {

        var r = GetComponent<Image>();
        Material mat = Instantiate(r.material);
        r.material = mat;
        r.material.SetVector("_HSLAAdjust", new Vector4(Random.Range(0f, 1f), 0f, 0f, 0f));
	}
}
