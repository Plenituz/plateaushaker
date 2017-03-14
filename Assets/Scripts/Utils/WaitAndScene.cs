using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WaitAndScene : MonoBehaviour {

	void Start () {
        StartCoroutine("w");
	}
	
	IEnumerator w()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Main");
    }
}
