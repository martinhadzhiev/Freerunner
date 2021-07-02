using UnityEngine;
using System.Collections.Generic;

public class MeshFilterBoundsExtender : MonoBehaviour
{
    [SerializeField]
    private GameObject[] surroungings;
    [SerializeField]
    private float meshBoundsMultiplier = 1.5f;

    private List<MeshFilter> surrMeshFilters = new List<MeshFilter>();
    void Start()
    {
        CollectMeshFilters();
        ExtendMeshBounds();
    }

    private void CollectMeshFilters()
    {
        foreach (var surr in surroungings)
        {
            surrMeshFilters.AddRange(surr.GetComponentsInChildren<MeshFilter>(true));
        }
    }

    private void ExtendMeshBounds()
    {
        foreach (var meshFilter in surrMeshFilters)
        {
            Bounds bounds = meshFilter.mesh.bounds;
            bounds.extents *= meshBoundsMultiplier;
            meshFilter.mesh.bounds = bounds;
        }
    }
}
