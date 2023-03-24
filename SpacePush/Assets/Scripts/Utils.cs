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

	public static float Random(this Vector2 v)
	{
		return UnityEngine.Random.Range(v.x, v.y);
	}

	public static int RandomSign()
	{
		return UnityEngine.Random.value < 0.5 ? 1 : -1;
	}

	public static float Atan2(Vector2 v)
	{
		return Mathf.Atan2(v.y, v.x);
	}

	public static Vector3 ToVector3(this Vector2 v)
	{
		return new Vector3(v.x, v.y);
	}

	public static Vector2 ToVector2(this Vector3 v)
	{
		return new Vector2(v.x, v.y);
	}
}
