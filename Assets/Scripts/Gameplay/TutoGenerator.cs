using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutoGenerator : MonoBehaviour {
    private int clicked = 0;
    public GameObject saveText;
    public GameObject text2;
    public GameObject text3;
    public GameObject text4;
    public GameObject plank;
    public Rigidbody2D left;
    public Rigidbody2D right;
    public Transform spawnRight;
    public Transform spawnLeft;

    private float startTime;

	void Start () {
        startTime = Time.time;
        StartCoroutine("Gen");
        ControlManager.GetInstance().CreateEvent("Loose");
        ControlManager.GetInstance().SubscribeToEvent("Loose", DoDeath);
	}

    public void Click()
    {
        if (Time.time - startTime < 1f)
            return;
        if (clicked == 0)
        {
            startTime = Time.time;
            clicked++;
            StopCoroutine("Gen");
            StartCoroutine("Noel");
            saveText.SetActive(false);
            text2.SetActive(true);
            text4.SetActive(true);
            left.isKinematic = false;
            right.isKinematic = false;
            Destroy(left.gameObject, 2f);
            Destroy(right.gameObject, 2f);
        }
        else if(clicked == 1)
        {
            Time.timeScale = 1.3f;
            startTime = Time.time + 9f;
            clicked++;
            StopCoroutine("Noel");
            text2.SetActive(false);
            text3.SetActive(true);
            text4.SetActive(false);
            plank.transform.rotation = Quaternion.Euler(0f, 0f, -17f);
            StartCoroutine("Gen2");
        }
        else
        {
            DataStorer.Store("tuto", true.ToString());
            Time.timeScale = 1f;
            SceneManager.LoadScene("Tuto");
        }
    }

    void DoDeath(object o)
    {
        if(clicked == 2)
        {
            GameObject death = Instantiate(Resources.Load<GameObject>("Death"));
            death.GetComponent<Trans>().SetPos(new Vector2(0.7f, 0f));
        }
    }

    IEnumerator Gen2()
    {
        while (true)
        {
            Destroy(SpawnBoxAt(spawnLeft.position), 7.3f);
            Destroy(SpawnBoxAt(spawnRight.position), 5f);
            Destroy(SpawnBoxAt(transform.position), 5f);
            yield return new WaitForSeconds(3f);
            yield return Anim.AnimateValue(-17f, 0f, 0.5f, Anim.AccelerateDeccelerateInterpolator,
                (float value) =>
                {
                    plank.transform.rotation = Quaternion.Euler(0f, 0f, value);
                }, null);
            GameObject death1 = Instantiate(Resources.Load<GameObject>("Death"));
            death1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Pouss");
            death1.GetComponent<Trans>().SetPos(new Vector2(0f, 0.2f));
            yield return new WaitForSeconds(1f);
            StartCoroutine(Anim.AnimateValue(0f, -17f, 0.5f, Anim.AccelerateDeccelerateInterpolator,
                (float value) =>
                {
                    plank.transform.rotation = Quaternion.Euler(0f, 0f, value);
                }, null));
            Destroy(SpawnBoxAt(spawnLeft.position), 8f);
            Destroy(SpawnBoxAt(spawnRight.position), 5f);
            Destroy(SpawnBoxAt(transform.position), 5f);
            yield return new WaitForSeconds(5.3f);
        }
    }

    IEnumerator Noel()
    {
        while (true)
        {
            float v = 1f;
            yield return new WaitForSeconds(v);

            GameObject g1 = Instantiate(Resources.Load<GameObject>("Death"));
            g1.GetComponent<Trans>().SetPos(new Vector2(-0.54f, -0.88f));
            GameObject g = Instantiate(Resources.Load<GameObject>("Death"));
            g.GetComponent<Trans>().SetPos(new Vector2(0.54f, -0.88f));

            yield return new WaitForSeconds(2f - v);
            Destroy(Instantiate(Resources.Load<GameObject>("Right Dude")), 2f);
            Destroy(Instantiate(Resources.Load<GameObject>("Left Dude")), 2f);
        }
    }
	
    IEnumerator Gen()
    {
        while (true)
        {
            Destroy(SpawnBoxAt(transform.position), 3.5f);
            yield return new WaitForSeconds(3f);
        }
    }

    private GameObject SpawnBoxAt(Vector3 position)
    {
        GameObject box = Instantiate(Resources.Load<GameObject>("KDO"));
        box.transform.position = position;
        return box;
    }
}
