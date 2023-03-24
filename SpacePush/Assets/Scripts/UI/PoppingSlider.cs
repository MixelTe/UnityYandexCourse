using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
[RequireComponent(typeof(PoppingItem))]
public class PoppingSlider : MonoBehaviour
{
	private Slider _slider;
	private PoppingItem _pop;

	private void Awake()
	{
		_slider = GetComponent<Slider>();
		_pop = GetComponent<PoppingItem>();
	}

	public void SetValue(float value, bool pop = false)
	{
		_slider.value = value;
		if (pop)
			_pop.Pop();
	}

	public void Pop()
	{
		_pop.Pop();
	}
}
