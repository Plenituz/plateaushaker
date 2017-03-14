using UnityEngine;
using System.Collections;

public class DeathAnim : MonoBehaviour {
    private Trans trans;
    private SpriteRenderer sr;

	void Start () {
        sr = GetComponent<SpriteRenderer>();
        trans = GetComponent<Trans>();
        StartCoroutine("swag");
	}
	
	IEnumerator swag()
    {
        Vector3 size = transform.localScale;
        yield return Anim.AnimateValue(1f, 2f, 1f, Anim.AccelerateDeccelerateInterpolator,
            (float value) =>
            {
                transform.localScale = size * value;
                float alpha = Mathf.Abs(value - 2f);
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
            }, null);
        Destroy(gameObject);
    }
}
