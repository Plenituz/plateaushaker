using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour {
    public BoxCollider2D collider;
    public Text uiScore;

   // [HideInInspector]
    public int score = 0;


	void Start () {
        StartCoroutine(Count());
	}

    IEnumerator Count()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(2.98f);
            if (!enabled)
                yield break;

            int swagAdded = 0;
            GameObject[] kdos = GameObject.FindGameObjectsWithTag("KDO");

           // PolygonCollider2D[] colls = FindObjectsOfType<PolygonCollider2D>();
            for(int i = 0; i < kdos.Length; i++)
            {
                PolygonCollider2D coll = kdos[i].GetComponent<PolygonCollider2D>();
                if (collider.bounds.Intersects(coll.bounds))
                {
                    //add points
                    score++;
                    swagAdded++;
                    if (coll.GetComponent<Trans>().size.x == 0.01f)
                        score++;
                    if (Lagger.enableLag)
                    {
                        GameObject go = Instantiate(Resources.Load<GameObject>("Points particle"));
                        go.transform.position = coll.transform.position;
                        Destroy(go, 2f);
                    }
                }
            }
            if (swagAdded == 0)
               ControlManager.GetInstance().TriggerEvent("Loose", collider.bounds.center);
            else
                uiScore.text = score.ToString();
        }
    }
}
