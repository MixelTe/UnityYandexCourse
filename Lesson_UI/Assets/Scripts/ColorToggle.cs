using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorToggle : MonoBehaviour
{
	[SerializeField] private Image _image;
	[SerializeField] private Toggle _toggle;

	public void Init(Color color, ToggleGroup group, MaterialManager materialManager)
	{
		_image.color = color;
		_toggle.group = group;
		_toggle.onValueChanged.AddListener((bool isChecked) =>
		{
			materialManager.SetColor(color);
		});
	}
}
