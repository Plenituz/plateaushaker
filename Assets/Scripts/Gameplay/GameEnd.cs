using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour {

    private bool lost = false;

    void Awake()
    {
        ControlManager.GetInstance().CreateEvent("Loose");
        ControlManager.GetInstance().SubscribeToEvent("Loose", Loose);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("ExitKill"))
        {
            Loose(coll.transform.position);
        }
    }

    public void Loose(object o)
    {
        if (lost)
            return;
        lost = true;
        StartCoroutine(End(o));
    }

    IEnumerator End(object o)
    {
        GameObject death = Instantiate(Resources.Load<GameObject>("Death"));
        Destroy(death.GetComponent<Trans>());
        death.transform.position = (Vector3)o;

        FindObjectOfType<Controller>().enabled = false;
        FindObjectOfType<Generator>().enabled = false;
        FindObjectOfType<ScoreCounter>().enabled = false;

        yield return new WaitForSeconds(1.3f);

        GameObject deathScreen = Instantiate(Resources.Load<GameObject>("DeathScreen"));
        Image[] imgs = deathScreen.GetComponentsInChildren<Image>();
        for (int i = 0; i < imgs.Length; i++)
        {
            imgs[i].GetComponent<CanvasRenderer>().SetAlpha(0f);
            imgs[i].CrossFadeAlpha(1f, 0.4f, true);
        }
    }
}
