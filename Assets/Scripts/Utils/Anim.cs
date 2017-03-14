using System;
using System.Collections;
using UnityEngine;

public class Anim {
	public static IEnumerator AnimateValue(float from, float to, float duration, Interpolator interpolator, OnAnimationUpdate oau, OnAnimationUpdate onAnimEnd){
		float startTime = Time.time;
		while (Time.time - startTime <= duration) {
			float value;
			if(interpolator != null)
				value = (from + (to - from)*interpolator((Time.time - startTime)/duration));
			else
				value = (from + (to - from)*((Time.time - startTime)/duration));
			if (oau != null)
				oau (value);
            yield return new WaitForEndOfFrame();
		}
		if (onAnimEnd != null)
			onAnimEnd (to);
	}

	public static IEnumerator AnimateValue(float from, float to, float duration, Interpolator interpolator, OnAnimationUpdateObj oau, OnAnimationUpdateObj onAnimEnd, object o){
		float startTime = Time.time;
		while (Time.time - startTime <= duration) {
			float value;
			if(interpolator != null)
				value = (from + (to - from)*interpolator((Time.time - startTime)/duration));
			else
				value = (from + (to - from)*((Time.time - startTime)/duration));
			if (oau != null)
				oau (value, o);
			yield return new WaitForEndOfFrame ();
		}
		if (onAnimEnd != null)
			onAnimEnd (to, o);
	}

	public static float AccelerateDeccelerateInterpolator(float input){
		return (Mathf.Cos ((input + 1) * Mathf.PI) / 2.0f) + 0.5f;
	}

	public static float OvershootInterpolator(float t){
		t -= 1.0f;
		//2.0f can be replaced by tension
		return t * t * ((2.0f + 1) * t + 2.0f) + 1.0f;
	}

	public static float AnticipateInterpolator(float t){
		return t * t * ((2.0f + 1) * t - 2.0f);
	}
}
public delegate void OnAnimationUpdateObj(float value, object o);
public delegate void OnAnimationUpdate(float value);
public delegate float Interpolator(float input);

