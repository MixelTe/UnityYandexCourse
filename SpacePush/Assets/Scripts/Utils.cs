using System;
using UnityEngine;


public static class Utils
{
	public static Rect Inflate(this Rect rect, float v)
	{
		return new Rect(
			rect.x - v,
			rect.y - v,
			rect.width + v * 2,
			rect.height + v * 2
			);
	}

	public static T Random<T>(this T[] array)
	{
		return array[UnityEngine.Random.Range(0, array.Length)];
	}

	public static float Atan2(Vector2 v)
	{
		return Mathf.Atan2(v.y, v.x);
	}
}
