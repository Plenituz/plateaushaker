using UnityEngine;
using System.Collections;

public class TriggerAnim : MonoBehaviour {

	public void Trigger()
    {
        ControlManager.GetInstance().TriggerEvent("Anim", null);
    }
}
