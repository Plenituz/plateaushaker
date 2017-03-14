using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
    private float time;
    private float start;

    void Start()
    {
        start = Time.time;
        time = Random.Range(2f, 2f);
    }

    void Update()
    {
        if (Time.time - start > time)
            Explode();
    }	

    void Explode()
    {
        float radius = P.pocP(0.8f, Side.W);
        Collider2D[] list = Physics2D.OverlapCircleAll(transform.position, radius);
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].CompareTag("KDO"))
            {
                Rigidbody2D rg = list[i].GetComponent<Rigidbody2D>();
                AddExplosionForce(rg, 8f, transform.position, radius);
            }
        }
        Destroy(gameObject);
    }

    public static void AddExplosionForce(Rigidbody2D body, float expForce, Vector3 expPosition, float expRadius)
    {
        var dir = (body.transform.position - expPosition);
        float calc = 1 - (dir.magnitude / expRadius);
        if (calc <= 0)
        {
            calc = 0;
        }

        body.AddForce(dir.normalized * expForce * calc, ForceMode2D.Impulse);
    }
}
