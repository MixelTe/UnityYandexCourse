using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{

    [SerializeField] private Material _material;

    public void SetColor(Color color)
    {
        _material.color = color;
    }

}
