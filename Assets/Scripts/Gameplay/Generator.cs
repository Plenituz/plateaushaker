using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generator : MonoBehaviour {

    public Transform right;
    public Transform left;

	// Use this for initialization
	void Start () {
        StartCoroutine(Gen());
	}
	
    IEnumerator Gen()
    {
        yield return new WaitForSeconds(0.5f);
        while (enabled)
        {
            SpawnBox();
            yield return new WaitForSeconds(3f);
        }
    }

    private void SpawnBoxAt(Vector3 position)
    {
        GameObject box = Instantiate(Resources.Load<GameObject>("KDO"));
        box.transform.position = position;
    }

    private void SpawnBigBoxAt(Vector3 position)
    {
        GameObject box = Instantiate(Resources.Load<GameObject>("KDO"));
        box.transform.position = position;
        box.GetComponent<Rigidbody2D>().mass *= 2f;
        Trans t = box.GetComponent<Trans>();
        t.SetSize(t.size * 2f);
    }

    private void SpawnBombAt(Vector3 position)
    {
        GameObject bomb = Instantiate(Resources.Load<GameObject>("Bomb"));
        bomb.transform.position = position;
    }

    private Transform RandomPlace()
    {
        int r = Random.Range(0, 3);
        switch (r)
        {
            case 0:
                return right;
            case 1:
                return left;
            default:
                return transform;
        }
    }

    private void SpawnBox()
    {
        int nbSpawn = Random.Range(1, 3);
        List<Transform> where = new List<Transform>();

        for(int i = 0; i < nbSpawn; i++)
        {
            Transform swag = null;
            do
            {
                swag = RandomPlace();
            }
            while (where.Contains(swag));
            where.Add(swag);
        }

        bool alreadyBig = false;
        for(int i = 0; i < where.Count; i++)
        {
            float rand = Random.value;

            if (!alreadyBig && rand < 0.12f)
            {
                alreadyBig = true;
                SpawnBigBoxAt(where[i].position);
            }
            else
            {
                SpawnBoxAt(where[i].position);
            }
        }
    }


}
