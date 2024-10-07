using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class StaticMethods
{
    public static Vector3 GetRandomLocationOnNavmesh()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
        int maxIndices = navMeshData.indices.Length - 3;
        int firstVertexSelected = Random.Range(0, maxIndices);
        int secondVertexSelected = Random.Range(0, maxIndices);
        Vector3 point = navMeshData.vertices[navMeshData.indices[firstVertexSelected]];
        Vector3 firstVertexPosition = navMeshData.vertices[navMeshData.indices[firstVertexSelected]];
        Vector3 secondVertexPosition = navMeshData.vertices[navMeshData.indices[secondVertexSelected]];
        if ((int)firstVertexPosition.x == (int)secondVertexPosition.x ||(int)firstVertexPosition.z == (int)secondVertexPosition.z)
        {
            point = GetRandomLocationOnNavmesh();
        }
        else
        {
            point = Vector3.Lerp(firstVertexPosition, secondVertexPosition,Random.Range(0.05f, 0.95f));
        }
        return point;
    }
}
