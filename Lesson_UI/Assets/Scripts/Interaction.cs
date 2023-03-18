using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interaction : MonoBehaviour
{

	[SerializeField] private Camera _camera;

	void Update()
	{
		if (EventSystem.current.IsPointerOverGameObject()) return;
		
		Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
		if (!Physics.Raycast(ray, out RaycastHit hit)) return;

		if (hit.collider.TryGetComponent(out IHoverable hoverable))
		{
			hoverable.Hover();
		}
		
		if (Input.GetMouseButtonDown(0) && 
			hit.collider.TryGetComponent(out IClickable clickable))
		{
			clickable.Hit();
		}
	}
}
public interface IClickable
{
	public void Hit();
}

public interface IHoverable
{
	public void Hover();
}