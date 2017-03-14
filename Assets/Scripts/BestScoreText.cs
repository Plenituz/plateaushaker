using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BestScoreText : MonoBehaviour {

	void Start () {
        DataStorer.InitFile("prout", 0.ToString());
        int best = int.Parse(DataStorer.Read("prout"));
        if(best != 0)
        {
            GetComponent<Text>().text = "Best : " + best;
        }
	}
}
