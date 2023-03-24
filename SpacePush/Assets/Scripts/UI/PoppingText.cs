using System;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(PoppingItem))]
public class PoppingText : MonoBehaviour
{
	private TextMeshProUGUI _text;
	private PoppingItem _pop;

	private void Awake()
	{
		_text = GetComponent<TextMeshProUGUI>();
		_pop = GetComponent<PoppingItem>();
	}

	public void SetText(IFormattable value, bool pop = false)
	{
		_text.text = value.ToString();
		if (pop)
			_pop.Pop();
	}
}
