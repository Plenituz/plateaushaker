using UnityEngine;
using System.Collections;

public class PulsateTuto : MonoBehaviour {
    Trans trans;

	void Start()
    {
        trans = GetComponent<Trans>();
        StartCoroutine(An());
    }

    IEnumerator An()
    {
        Vector2 size = trans.size;
        while (true)
        {
            yield return Anim.AnimateValue(1f, 0.9f, 0.5f, Anim.AccelerateDeccelerateInterpolator,
            (float value) =>
            {
                trans.SetSize(size * value);
            }, null);
            yield return Anim.AnimateValue(0.9f, 1f, 0.5f, Anim.AccelerateDeccelerateInterpolator,
                (float value) =>
                {
                    trans.SetSize(size * value);
                }, null);
        }
    }
}
