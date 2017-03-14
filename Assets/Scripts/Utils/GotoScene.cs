using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GotoScene : MonoBehaviour {
    public Text text;

    void Start()
    {
        StartCoroutine(Goto());
    }

	IEnumerator Goto()
    {
        yield return new WaitForSeconds(0.5f);
        text.text = "2";
        yield return new WaitForSeconds(0.5f);
        text.text = "1";
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Main");
    }
}
