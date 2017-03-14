using UnityEngine;
using System.Collections;

public class Reset : MonoBehaviour {

    public GameObject plate;
    public GameObject leftDude;
    public GameObject rightDude;

    public void Res()
    {
        plate.GetComponent<Trans>().SetPos(new Vector2(0f, 0.42f));
        plate.transform.rotation = Quaternion.identity;
        leftDude.GetComponent<Trans>().SetPos(new Vector2(-0.52f, -0.17f));
        rightDude.GetComponent<Trans>().SetPos(new Vector2(0.52f, -0.17f));
        rightDude.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        leftDude.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        plate.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        plate.GetComponent<Rigidbody2D>().angularVelocity = 0f;
    }
}
