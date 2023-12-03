using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangularBoundary2D : MonoBehaviour
{
    public Vector2 innerShellAnchor;
    public Vector2Int numPointsInnerShell;
    public float density;
    public int layers;

    public Material mat1;
    public Material mat2;
    public Material mat3;

    public GameObject prefab;

    private float linearSpacing;

    public void GenerateBoundary() {
        RemoveChildren();

        linearSpacing = 1 / Mathf.Pow(density, 1f / 2f);
        for (int i = 0; i < layers; i++) {
            Vector2Int numPoints = numPointsInnerShell + i * (Vector2Int.one * 2 - Vector2Int.up);
            Vector2 lengths = linearSpacing * (numPoints - Vector2.one);
            Vector2 anchor = innerShellAnchor - i * linearSpacing * Vector2.one;
            GenerateShell(anchor, lengths, numPoints);
        }

    }

    private Vector2[] GenerateShell(Vector2 anchor, Vector2 lengths, Vector2Int numPoints) {
        int numCornerPoints = 4;
        int numEdgePoints = (numPoints.x - 2) + 2 * (numPoints.y - 2);
        Vector2[] points = new Vector2[numEdgePoints + numCornerPoints];
        int pointIndex = 0;

        // corners
        points[pointIndex++] = new Vector2(anchor.x, anchor.y);
        DrawSprite(points[pointIndex - 1], mat1);
        points[pointIndex++] = new Vector2(anchor.x + lengths.x, anchor.y);
        DrawSprite(points[pointIndex - 1], mat1);
        points[pointIndex++] = new Vector2(anchor.x, anchor.y + lengths.y);
        DrawSprite(points[pointIndex - 1], mat1);
        points[pointIndex++] = new Vector2(anchor.x + lengths.x, anchor.y + lengths.y);
        DrawSprite(points[pointIndex - 1], mat1);

        // bottom edge
        for (int xi = 1; xi < numPoints.x - 1; xi++) {
            points[pointIndex++] = new Vector2(anchor.x + xi * linearSpacing, anchor.y);
            DrawSprite(points[pointIndex - 1], mat2);
        }

        for (int yi = 1; yi < numPoints.y - 1; yi++) {
            points[pointIndex++] = new Vector2(anchor.x, anchor.y + yi * linearSpacing); // left edge
            DrawSprite(points[pointIndex - 1], mat3);
            points[pointIndex++] = new Vector2(anchor.x + lengths.x, anchor.y + yi * linearSpacing); // right edge
            DrawSprite(points[pointIndex - 1], mat3);
        }

        return points;
    }

    private void RemoveChildren() {
        for (int i = transform.childCount - 1; i >= 0; i--) {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    private void DrawSprite(Vector3 position, Material mat) {
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        obj.transform.position = position;
        obj.transform.parent = transform;
        obj.GetComponent<SpriteRenderer>().material = mat;
    }
}
