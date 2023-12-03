using UnityEngine;

public class RectangularBoundary3D : MonoBehaviour
{
    public Vector3 innerShellAnchor;
    public Vector3Int numPointsInnerShell;
    public float density;
    public int layers;

    public Material mat1;
    public Material mat2;
    public Material mat3;
    public Material mat4;

    public GameObject prefab;

    private float linearSpacing;

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
            DrawSprite(points[pointIndex - 1], mat1);
            points[pointIndex++] = new Vector3(anchor.x + xi * linearSpacing, anchor.y + lengths.y, anchor.z);
            DrawSprite(points[pointIndex - 1], mat1);
            points[pointIndex++] = new Vector3(anchor.x + xi * linearSpacing, anchor.y, anchor.z + lengths.z);
            DrawSprite(points[pointIndex - 1], mat1);
            points[pointIndex++] = new Vector3(anchor.x + xi * linearSpacing, anchor.y + lengths.y, anchor.z + lengths.z);
            DrawSprite(points[pointIndex - 1], mat1);
        }

        // y edges
        for (int yi = 0; yi < numPoints.y; yi++) {
            points[pointIndex++] = new Vector3(anchor.x, anchor.y + yi * linearSpacing, anchor.z);
            DrawSprite(points[pointIndex - 1], mat1);
            points[pointIndex++] = new Vector3(anchor.x + lengths.x, anchor.y + yi * linearSpacing, anchor.z);
            DrawSprite(points[pointIndex - 1], mat1);
            points[pointIndex++] = new Vector3(anchor.x, anchor.y + yi * linearSpacing, anchor.z + lengths.z);
            DrawSprite(points[pointIndex - 1], mat1);
            points[pointIndex++] = new Vector3(anchor.x + lengths.x, anchor.y + yi * linearSpacing, anchor.z + lengths.z);
            DrawSprite(points[pointIndex - 1], mat1);
        }

        // z edges
        for (int zi = 0; zi < numPoints.z; zi++) {
            points[pointIndex++] = new Vector3(anchor.x, anchor.y, anchor.z + zi * linearSpacing);
            DrawSprite(points[pointIndex - 1], mat1);
            points[pointIndex++] = new Vector3(anchor.x + lengths.x, anchor.y, anchor.z + zi * linearSpacing);
            DrawSprite(points[pointIndex - 1], mat1);
            points[pointIndex++] = new Vector3(anchor.x, anchor.y + lengths.y, anchor.z + zi * linearSpacing);
            DrawSprite(points[pointIndex - 1], mat1);
            points[pointIndex++] = new Vector3(anchor.x + lengths.x, anchor.y + lengths.y, anchor.z + zi * linearSpacing);
            DrawSprite(points[pointIndex - 1], mat1);
        }

        // bottom face
        for (int xi = 1; xi < numPoints.x - 1; xi++) {
            for (int zi = 1; zi < numPoints.z - 1; zi++) {
                points[pointIndex++] = new Vector3(anchor.x + xi * linearSpacing, anchor.y, anchor.z + zi * linearSpacing);
                DrawSprite(points[pointIndex - 1], mat2);
            }
        }

        // left face
        for (int xi = 1; xi < numPoints.x - 1; xi++) {
            for (int yi = 1; yi < numPoints.y - 1; yi++) {
                points[pointIndex++] = new Vector3(anchor.x + xi * linearSpacing, anchor.y + yi * linearSpacing, anchor.z);
                DrawSprite(points[pointIndex - 1], mat3);
            }
        }

        // right face
        for (int xi = 1; xi < numPoints.x - 1; xi++) {
            for (int yi = 1; yi < numPoints.y - 1; yi++) {
                points[pointIndex++] = new Vector3(anchor.x + xi * linearSpacing, anchor.y + yi * linearSpacing, anchor.z + lengths.z);
                DrawSprite(points[pointIndex - 1], mat3);
            }
        }

        // back face
        for (int zi = 1; zi < numPoints.z - 1; zi++) {
            for (int yi = 1; yi < numPoints.y - 1; yi++) {
                points[pointIndex++] = new Vector3(anchor.x, anchor.y + yi * linearSpacing, anchor.z + zi * linearSpacing);
                DrawSprite(points[pointIndex - 1], mat4);
            }
        }

        // front face
        for (int zi = 1; zi < numPoints.z - 1; zi++) {
            for (int yi = 1; yi < numPoints.y - 1; yi++) {
                points[pointIndex++] = new Vector3(anchor.x + lengths.x, anchor.y + yi * linearSpacing, anchor.z + zi * linearSpacing);
                DrawSprite(points[pointIndex - 1], mat4);
            }
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
