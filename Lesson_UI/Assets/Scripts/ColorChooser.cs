using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChooser : MonoBehaviour
{
	[SerializeField] private MaterialManager _materialManager;
	[SerializeField] private ToggleGroup _toggleGroup;
	[SerializeField] private ColorToggle _togglePrefab;
	[SerializeField] private Color[] _colors;

	private void Start()
	{
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
		foreach (var color in _colors)
		{
			var toggle = Instantiate(_togglePrefab, transform);
			toggle.Init(color, _toggleGroup, _materialManager);
		}
	}
}
