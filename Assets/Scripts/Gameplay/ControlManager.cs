using UnityEngine;
using System.Collections;

public class ControlManager {
	private static ControlManager instance;

	public static ControlManager GetInstance(){
		if (instance == null) {
			Init ();
		}
		return instance;
	}

	/**
	 * ArrayList of Event
	 */
	private ArrayList eventList = new ArrayList ();

	public static void Init(){
		if (instance == null) {
			instance = new ControlManager ();
		}
	}

	public static void Reset(){
		instance = new ControlManager ();
	}

	public void CreateEvent(string name){
		Event ev = new Event ();
		ev.name = name;
		for (int i = 0; i < eventList.Count; i++) {
			Event e = (Event)eventList [i];
			if (e.name.Equals (name)) {
				Debug.Log ("Error creating event, name is already taken");
				return;
			}
		}
		eventList.Add (ev);
		//Debug.Log ("creating event : " + ev.name + " " + eventList.Count);
	}

	public void SubscribeToEvent(string name, EventListener l){
		for (int i = 0; i < eventList.Count; i++) {
			Event e = (Event)eventList [i];
			if (e.name.Equals (name)) {
				e.eventListener += l;
//				Debug.Log ("subscribed to " + name);
				return;
			}
		}
		Debug.Log ("Trying to subscribe to unexistant event : " + name);
	}

	public void TriggerEvent(string name, object obj){
		for (int i = 0; i < eventList.Count; i++) {
			Event e = (Event)eventList [i];
			if (e.name.Equals (name)) {
				e.Trigger (obj);
				return;
			}
		}
		Debug.Log ("event " + name + " couldn't be triggered because it was not found");
	}

	/**
	 * Kinda pointless but you never know
	 */
	public void TriggerEvent(Event ev, object obj){
		ev.Trigger (obj);
	}

	/**
	 * Get directly the event to you don't have to call triggerEvent which loop through the list every time
	 */
	public Event GetEvent(string name){
		for (int i = 0; i < eventList.Count; i++) {
			Event e = (Event)eventList [i];
			if (e.name.Equals (name)) {
				return e;
			}
		}
		return null;
	}

	public static Color GetPlayerColor(int id){
		switch (id) {
		case 0: 
			return Color.red;
		case 1:
			return Color.blue;
		case 2:
			return Color.green;
		case 3:
			return Color.magenta;
		default:
			return Color.grey;
		}
	}

	public static string GetPlayerColorString(int id){
		switch (id) {
		case 0: 
			return "R";
		case 1:
			return "G";
		case 2:
			return "B";
		case 3:
			return "Y";
		default:
			return "D";
		}
	}
}

public delegate void EventListener(object obj);

public class Event{
	public string name;
	public EventListener eventListener;

	public void Trigger(object obj){
		if (eventListener != null) {
			eventListener (obj);
		}
	}
}

public enum Move {
	GO_RIGHT, 
	GO_LEFT
}
