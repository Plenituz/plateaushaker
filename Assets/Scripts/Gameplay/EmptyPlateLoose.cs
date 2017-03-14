using UnityEngine;
using System.Collections;

public class EmptyPlateLoose : MonoBehaviour {
    BoxCollider2D collibro;

    void Start()
    {
        collibro = GetComponent<BoxCollider2D>();
    }

	void OnTriggerExit2D(Collider2D collision)
    {
        //check if the plate is empty, if so loose 
        if (collision.CompareTag("KDO"))
        {
            bool good = false;
            GameObject[] kdos = GameObject.FindGameObjectsWithTag("KDO");
            for (int i = 0; i < kdos.Length; i++)
            {
                PolygonCollider2D coll = kdos[i].GetComponent<PolygonCollider2D>();
                if (coll != collision && collibro.bounds.Intersects(coll.bounds))
                {
                    good = true;
                    break;
                }
            }
            
            if (!good)
                ControlManager.GetInstance().TriggerEvent("Loose", collision.gameObject.transform.position);
        }
       
    }
}
