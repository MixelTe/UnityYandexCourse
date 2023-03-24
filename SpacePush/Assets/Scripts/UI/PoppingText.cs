using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TextMeshProUGUI))]
public class PoppingText : MonoBehaviour
{
	[SerializeField] private AnimationCurve _textAnim;
	[SerializeField] private float _textAnimTime;
	private TextMeshProUGUI _text;

	private float _curTextAnimTime = 1;

	private void Awake()
	{
		_text = GetComponent<TextMeshProUGUI>();
	}

	private void Update()
	{
		if (_curTextAnimTime == 1) return;
		_curTextAnimTime = Mathf.Min(_curTextAnimTime + Time.deltaTime / _textAnimTime, 1);
		var scale = _textAnim.Evaluate(_curTextAnimTime);
		_text.transform.localScale = Vector3.one * scale;
	}

	public void UpdateText(IConvertible value, bool pop = true)
	{
		_text.text = value.ToString();
		if (pop)
			_curTextAnimTime = 0;
	}
}
