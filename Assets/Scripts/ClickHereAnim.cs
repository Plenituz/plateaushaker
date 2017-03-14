using UnityEngine;
using System.Collections;

public class ClickHereAnim : MonoBehaviour {
    public float startOffset = 0f;
    private Trans trans;
    private AIRenderer rend;

	void Start () {
        rend = GetComponent<AIRenderer>();
        trans = GetComponent<Trans>();
        StartCoroutine(An());
	}

    IEnumerator An()
    {
        yield return new WaitForSeconds(startOffset);
        while (true)
        {
            StartCoroutine(Anim.AnimateValue(0f, 0.5f, 0.5f, Anim.AccelerateDeccelerateInterpolator,
                (float value) =>
                {
                    Color c = rend.color;
                    c.a = value;
                    rend.SetColor(c);
                },
                (float value) =>
                {
                    StartCoroutine(Anim.AnimateValue(0.5f, 0f, 0.5f, Anim.AccelerateDeccelerateInterpolator,
                        (float va) =>
                        {
                            Color c = rend.color;
                            c.a = va;
                            rend.SetColor(c);
                        }, null));
                }
                ));
            yield return Anim.AnimateValue(0.007f, 0.007f * 1.5f, 1f, Anim.OvershootInterpolator,
                (float value) =>
                {
                    trans.SetSize(new Vector2(value, value));
                }, null);
            trans.SetSize(new Vector2(0.01f, 0.01f));
            yield return new WaitForSeconds(0.1f);
        }
    }
}
