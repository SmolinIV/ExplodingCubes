using System.Collections.Generic;
using UnityEngine;

public class CubesCreator : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private List<Cube> _firstCubes;

    [SerializeField] private float _explosionForce = 1f;
    [SerializeField] private float _explosionRadius = 10f;
    [SerializeField] private float _affectedZoneRadius = 15f;

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
                newCube.Initialize(newSize, incrementalGrade, Random.ColorHSV());

                newCube.CubeClicked += ExplodeClickedCube;

                newCube.GetComponent<Rigidbody>().AddExplosionForce(_explosionForce, cube.transform.position, _explosionRadius);
            }
        }
        else
        {
            ExplodeWithoutSplit(cube);
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

    private void ExplodeWithoutSplit(Cube cube)
    {
        List<Rigidbody> nearbyCubes = GetExplodeHittedCubes(cube);

        if (nearbyCubes.Count == 0)
            return;

        foreach (Rigidbody cubeRigidbody in nearbyCubes) 
        {
            float distance = Vector3.Distance(cubeRigidbody.transform.position, cube.transform.position);
            float ScaledForce = (_explosionForce - distance) * cube.Grade;

            cubeRigidbody.AddExplosionForce(ScaledForce, cube.transform.position, _explosionRadius * cube.Grade);
        }
    }

    private List<Rigidbody> GetExplodeHittedCubes(Cube cube)
    {
        Collider[] hits = Physics.OverlapSphere(cube.transform.position, _affectedZoneRadius * cube.Grade);

        List<Rigidbody> cubes = new List<Rigidbody>();

        foreach (Collider hit in hits) 
            if (hit.TryGetComponent(out Cube component) || hit.attachedRigidbody != null)
                cubes.Add(hit.attachedRigidbody);

        return cubes;
    }
}
