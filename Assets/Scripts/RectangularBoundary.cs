using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangularBoundary : MonoBehaviour
{
    public Vector3 innerShellAnchor;
    public Vector3Int numPointsInnerShell;
    public float density;
    public int layers;

    private float linearSpacing;
    //private Vector3[] points;

    public void GenerateBoundary()
    {
        RemoveChildren();

        linearSpacing = 1 / Mathf.Pow(density, 1f / 3f);
        for (int i = 0; i < layers; i++) {
            Vector3Int numPoints = numPointsInnerShell + i * (Vector3Int.one * 2 - Vector3Int.up);
            Vector3 lengths = linearSpacing * (numPoints - Vector3.one);
            Vector3 anchor = innerShellAnchor - i * linearSpacing * Vector3.one;
            GenerateShell(anchor, lengths, numPoints);
        }
       
    }

    private Vector3[] GenerateShell(Vector3 anchor, Vector3 lengths, Vector3Int numPoints) {
        int numEdgePoints = 4 * (numPoints.x + numPoints.y + numPoints.z);
        int numFacePoints = 2 * (numPoints.x - 2) * (numPoints.y - 2)
                            + (numPoints.x - 2) * (numPoints.z - 2)
                            + 2 * (numPoints.y - 2) * (numPoints.z - 2);
        Vector3[] points = new Vector3[numEdgePoints + numFacePoints];
        int pointIndex = 0;

        // x edges
        for (int xi = 0; xi < numPoints.x; xi++) {
            points[pointIndex++] = new Vector3(anchor.x + xi * linearSpacing, anchor.y, anchor.z);
            InstantiateSphere(points[pointIndex - 1], Color.white);
            points[pointIndex++] = new Vector3(anchor.x + xi * linearSpacing, anchor.y + lengths.y, anchor.z);
            InstantiateSphere(points[pointIndex - 1], Color.white);
            points[pointIndex++] = new Vector3(anchor.x + xi * linearSpacing, anchor.y, anchor.z + lengths.z);
            InstantiateSphere(points[pointIndex - 1], Color.white);
            points[pointIndex++] = new Vector3(anchor.x + xi * linearSpacing, anchor.y + lengths.y, anchor.z + lengths.z);
            InstantiateSphere(points[pointIndex - 1], Color.white);
        }

        // y edges
        for (int yi = 0; yi < numPoints.y; yi++) {
            points[pointIndex++] = new Vector3(anchor.x, anchor.y + yi * linearSpacing, anchor.z);
            InstantiateSphere(points[pointIndex - 1], Color.white);
            points[pointIndex++] = new Vector3(anchor.x + lengths.x, anchor.y + yi * linearSpacing, anchor.z);
            InstantiateSphere(points[pointIndex - 1], Color.white);
            points[pointIndex++] = new Vector3(anchor.x, anchor.y + yi * linearSpacing, anchor.z + lengths.z);
            InstantiateSphere(points[pointIndex - 1], Color.white);
            points[pointIndex++] = new Vector3(anchor.x + lengths.x, anchor.y + yi * linearSpacing, anchor.z + lengths.z);
            InstantiateSphere(points[pointIndex - 1], Color.white);
        }

        // z edges
        for (int zi = 0; zi < numPoints.z; zi++) {
            points[pointIndex++] = new Vector3(anchor.x, anchor.y, anchor.z + zi * linearSpacing);
            InstantiateSphere(points[pointIndex - 1], Color.white);
            points[pointIndex++] = new Vector3(anchor.x + lengths.x, anchor.y, anchor.z + zi * linearSpacing);
            InstantiateSphere(points[pointIndex - 1], Color.white);
            points[pointIndex++] = new Vector3(anchor.x, anchor.y + lengths.y, anchor.z + zi * linearSpacing);
            InstantiateSphere(points[pointIndex - 1], Color.white);
            points[pointIndex++] = new Vector3(anchor.x + lengths.x, anchor.y + lengths.y, anchor.z + zi * linearSpacing);
            InstantiateSphere(points[pointIndex - 1], Color.white);
        }

        // bottom face
        for (int xi = 1; xi < numPoints.x - 1; xi++) {
            for (int zi = 1; zi < numPoints.z - 1; zi++) {
                points[pointIndex++] = new Vector3(anchor.x + xi * linearSpacing, anchor.y, anchor.z + zi * linearSpacing);
                InstantiateSphere(points[pointIndex - 1], Color.green);
            }
        }

        // left face
        for (int xi = 1; xi < numPoints.x - 1; xi++) {
            for (int yi = 1; yi < numPoints.y - 1; yi++) {
                points[pointIndex++] = new Vector3(anchor.x + xi * linearSpacing, anchor.y + yi * linearSpacing, anchor.z);
                InstantiateSphere(points[pointIndex - 1], Color.blue);
            }
        }

        // right face
        for (int xi = 1; xi < numPoints.x - 1; xi++) {
            for (int yi = 1; yi < numPoints.y - 1; yi++) {
                points[pointIndex++] = new Vector3(anchor.x + xi * linearSpacing, anchor.y + yi * linearSpacing, anchor.z + lengths.z);
                InstantiateSphere(points[pointIndex - 1], Color.blue);
            }
        }

        // back face
        for (int zi = 1; zi < numPoints.z - 1; zi++) {
            for (int yi = 1; yi < numPoints.y - 1; yi++) {
                points[pointIndex++] = new Vector3(anchor.x, anchor.y + yi * linearSpacing, anchor.z + zi * linearSpacing);
                InstantiateSphere(points[pointIndex - 1], Color.red);
            }
        }

        // front face
        for (int zi = 1; zi < numPoints.z - 1; zi++) {
            for (int yi = 1; yi < numPoints.y - 1; yi++) {
                points[pointIndex++] = new Vector3(anchor.x + lengths.x, anchor.y + yi * linearSpacing, anchor.z + zi * linearSpacing);
                InstantiateSphere(points[pointIndex - 1], Color.red);
            }
        }

        return points;
    }

    private void RemoveChildren() {
        for (int i = transform.childCount - 1; i >= 0; i--) {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    private void InstantiateSphere(Vector3 position, Color color) {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;
        sphere.transform.parent = transform;
        sphere.GetComponent<MeshRenderer>().material.color = color;
    }
}
