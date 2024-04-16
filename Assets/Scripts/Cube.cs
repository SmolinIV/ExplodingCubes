using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private Renderer _renderer;

    public Action<Cube> CubeClicked;

    public int Grade { get; private set; } = 0;
    public float Size { get; private set; }
    public int ShardsCount { get; private set; }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

        Size = transform.localScale.x;

        RandomizeShardsCount();
    }

    private void OnMouseUpAsButton()
    {
        CubeClicked?.Invoke(this);
    }

    public void Initialize(float size, int grade, Color color)
    {
        Size = size;
        transform.localScale = new Vector3(size, size, size);

        Grade = grade;

        _renderer.material.color = color;

        RandomizeShardsCount();
    }

    private void RandomizeShardsCount()
    {
        int minShardsCount = 2;
        int maxShardsCount = 6;

        ShardsCount = UnityEngine.Random.Range(minShardsCount, maxShardsCount);
    }
}
