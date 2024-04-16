using System.Collections.Generic;
using UnityEngine;

public class CubesCreator : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private List<Cube> _firstCubes;

    [SerializeField] private float _explosionForce = 1;
    [SerializeField] private float _explosionRadius = 10;

    private void Awake()
    {
        foreach (var cube in _firstCubes)
            cube.CubeClicked += ExplodeClickedCube;
    }

    public void ExplodeClickedCube(Cube cube)
    {
        if (cube == null)
            return;

        cube.gameObject.SetActive(false);

        if (GenerateChanceToSplitCube(cube.Grade))
        {
            float halfDevider = 2;
            float newSize = cube.Size / halfDevider;
            int incrementalGrade = cube.Grade + 1;

            for (int i = 0; i < cube.ShardsCount; i++)
            {
                Cube newCube = Instantiate(_cubePrefab, cube.transform.position, new Quaternion(0, 0, 0, 0));
                newCube.Initialize(newSize, incrementalGrade, GenerateNewColor());

                newCube.CubeClicked += ExplodeClickedCube;

                newCube.GetComponent<Rigidbody>().AddExplosionForce(_explosionForce, cube.transform.position, _explosionRadius);
            }
        }

        cube.CubeClicked -= ExplodeClickedCube;

        Destroy(cube.gameObject);
    }

    private bool GenerateChanceToSplitCube(int grade)
    {
        float halfDevider = 2;
        float fullPercentChance = 100;

        float currentPercentChance = fullPercentChance / Mathf.Pow(halfDevider, grade);

        return Random.Range(0, fullPercentChance) <= currentPercentChance;
    }

    private Color GenerateNewColor()
    {
        float minColorValue = 0;
        float maxColorValue = 1;
        Color newColor = new Color();

        newColor.r = Random.Range(minColorValue, maxColorValue);
        newColor.g = Random.Range(minColorValue, maxColorValue);
        newColor.b = Random.Range(minColorValue, maxColorValue);
        newColor.a = maxColorValue;

        return newColor;
    }
}
