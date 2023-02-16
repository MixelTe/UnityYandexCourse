using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowingCamera : MonoBehaviour
{
    [SerializeField]
    private float _leftBoundary = 0;
    [SerializeField]
    private float _rightBoundary = 100;
    [SerializeField]
    private GameObject _followingObj;

    private Camera _camera;

	private void Start()
	{
        _camera = GetComponent<Camera>();
    }

	void LateUpdate()
    {
        if (!_followingObj) return;
        var pos = transform.position;

        var cameraHeight = _camera.orthographicSize;
        var cameraWidth = cameraHeight * Screen.width / Screen.height;

        pos.x = _followingObj.transform.position.x;
        pos.x = Mathf.Min(Mathf.Max(pos.x, _leftBoundary + cameraWidth), _rightBoundary - cameraWidth);

        transform.position = Vector3.Lerp(transform.position, pos, 0.05f);
    }

	private void OnDrawGizmosSelected()
	{
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(_leftBoundary, 100, 0), new Vector3(_leftBoundary, -100, 0));
        Gizmos.DrawLine(new Vector3(_rightBoundary, 100, 0), new Vector3(_rightBoundary, -100, 0));
    }
}
