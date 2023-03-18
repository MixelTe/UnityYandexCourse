using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesCreator : MonoBehaviour
{
    [SerializeField] private ClickablePiece _piecePrefab;
    [SerializeField] private ModelVariants _modelVariants;
    [SerializeField] private Resources _resources;

    private void OnEnable()
    {
        _resources.OnHitCube += CreatePiece;
    }

    private void OnDisable()
    {
        _resources.OnHitCube -= CreatePiece;
    }

    public void CreatePiece(int value, Vector3 worldPosition)
    {
        var piece = Instantiate(_piecePrefab, worldPosition, Quaternion.identity);
        piece.Init(_modelVariants.GetCurrentMesh(), value, _resources);
    }
}
