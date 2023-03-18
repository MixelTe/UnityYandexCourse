using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ClickablePiece : MonoBehaviour, IHoverable
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private AnimationCurve _popCurve;
    [Header("Add prefab to scene to see Gizmos")]
    [SerializeField] private AnimationCurve _moveCurve;
    [SerializeField] private Vector2 _jumpHeight;
    [SerializeField] private Vector2 _radius;
    [SerializeField] private Vector2 _scale;
    [SerializeField] private Vector2 _angle;
    private Resources _resources;
    private int _value;
    private bool _collected = false;
    private bool _landed = false;

    public void Init(Mesh mesh, int value, Resources resources)
	{
        _meshFilter.mesh = mesh;
        _resources = resources;
        _value = value;
    }

    public void Hover()
    {
        if (!_landed || _collected) return;
        _collected = true;
        _resources.CollectCoins(_value, transform.position);
		StartCoroutine(Destruction());
    }

    private void Start()
    {
        transform.localScale = Vector3.one * Random.Range(_scale.x, _scale.y);

        var r = Random.Range(_radius.x, _radius.y);
        var a = Random.Range(_angle.x, _angle.y) * Mathf.Deg2Rad;
        var x = transform.position.x + Mathf.Cos(a) * r;
        var y = transform.localScale.y / 2;
        var z = transform.position.z + Mathf.Sin(a) * r;
        var targetPosition = new Vector3(x, y, z);

		StartCoroutine(MoveToPoint(transform.position, targetPosition));
    }

    private IEnumerator MoveToPoint(Vector3 a, Vector3 b)
    {
        var jumpHeight = Random.Range(_jumpHeight.x, _jumpHeight.y);
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            float x = Mathf.Lerp(a.x, b.x, t);
            float z = Mathf.Lerp(a.z, b.z, t);

            float y = _moveCurve.Evaluate(t) * jumpHeight + b.y;

            Vector3 position = new(x, y, z);
            transform.position = position;
            yield return null;
        }
        transform.position = b;
        _landed = true;
    }
    private IEnumerator Destruction()
    {
        var scale = transform.localScale.x;
        for (var t = 0f; t <= 1f; t += Time.deltaTime)
        {
            var v = _popCurve.Evaluate(t) * scale;
            transform.localScale = Vector3.one * v;
            yield return null;
        }
        Destroy(gameObject);
    }

	private void OnDrawGizmosSelected()
	{
        Handles.color = Color.green;
        var from = new Vector3(Mathf.Cos(_angle.x * Mathf.Deg2Rad), 0, Mathf.Sin(_angle.x * Mathf.Deg2Rad));
        Handles.DrawWireArc(transform.position, Vector3.up, from, _angle.x - _angle.y, _radius.x);
        Handles.DrawWireArc(transform.position, Vector3.up, from, _angle.x - _angle.y, _radius.y);

        Handles.DrawPolyLine(GenerateJumpTrack(14, _jumpHeight.x, from));
        Handles.DrawPolyLine(GenerateJumpTrack(14, _jumpHeight.y, from));
    }

    private Vector3[] GenerateJumpTrack(int pointsCount, float height, Vector3 from)
	{
        var points = new Vector3[pointsCount];
        var lineEnd = transform.position + from * ((_radius.x + _radius.y) / 2);
        for (int i = 0; i < pointsCount; i++)
        {
            var t = i / (pointsCount - 1f);
            var x = Mathf.Lerp(transform.position.x, lineEnd.x, t);
            var z = Mathf.Lerp(transform.position.z, lineEnd.z, t);
            var y = _moveCurve.Evaluate(t) * height;
            points[i] = new Vector3(x, y, z);
        }
        return points;
    }
}
