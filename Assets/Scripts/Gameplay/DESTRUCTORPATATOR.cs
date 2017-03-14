using UnityEngine;
using System.Collections;
    
public class DESTRUCTORPATATOR : MonoBehaviour {

	void OnTriggerExit2D(Collider2D coll)
    {
        //if(!coll.name.Contains("Dude") && !coll.name.Contains("Plate"))
            Destroy(coll.gameObject);
    }
}
