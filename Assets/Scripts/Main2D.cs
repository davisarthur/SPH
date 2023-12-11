using System.Collections.Generic;
using UnityEngine;

public class Main2D : MonoBehaviour {
    public Material baseMaterial;
    public Vector2 bottomLeft;
    public Vector2 topRight;
    public int numParticles;
    public float smoothingLength;
    public GameObject particlePrefab;

    private float maxDensity = float.NegativeInfinity;
    private float minDensity = float.PositiveInfinity;

    // Primes used for hashing
    private static int PRIME1 = 73856093;
    private static int PRIME2 = 19349663;
    private static int HASH_TABLE_SIZE = 1000;

    GameObject[] particles;

    public void SpawnParticles() {
        DestroyChildren();
        particles = new GameObject[numParticles];
        for (int i = 0; i < numParticles; i++) {
            Vector2 diff = topRight - bottomLeft;
            float xOffset = Random.Range(0f, diff.x);
            float yOffset = Random.Range(0f, diff.y);
            particles[i] = Instantiate(particlePrefab, bottomLeft + new Vector2(xOffset, yOffset), Quaternion.identity);
            particles[i].name = "particle" + i;
            particles[i].transform.position = bottomLeft + new Vector2(xOffset, yOffset);
            particles[i].transform.parent = transform;
        }
    }

    public void ComputeDensity() {
        Dictionary<int, List<GameObject>> particleHashMap = CreateParticleHashMap();
        foreach (GameObject particleI in particles) {
            int[] neighborhood = GetNeighborhoodHashIds(particleI.transform.position);
            float densityI = 0f;
            foreach (int tileId in neighborhood) {
                if (!particleHashMap.ContainsKey(tileId)) continue;
                foreach (GameObject particleJ in particleHashMap[tileId]) {
                    Vector2 diff = particleJ.transform.position - particleI.transform.position;
                    float massJ = particleJ.GetComponent<Particle>().mass;
                    densityI += massJ * CubicSplineKernel.Evaluate2D(diff, smoothingLength);
                }
            }
            particleI.GetComponent<Particle>().density = densityI;
            if (densityI < minDensity) {
                minDensity = densityI;
            }
            if (densityI > maxDensity) {
                maxDensity = densityI;
            }
        }
        ColorParticlesByDensity();
    }

    public void ComputeHash() {
        foreach (GameObject particle in particles) {
            particle.GetComponent<Particle>().hash = GetHashFromRawPosition(particle.transform.position);
        }
        ColorParticlesByHashId();
    }

    private void ColorParticlesByDensity() {
        foreach (GameObject particle in particles) {
            float relativeDensity = Mathf.InverseLerp(minDensity, maxDensity, particle.GetComponent<Particle>().density);
            Color particleColor = Color.Lerp(Color.yellow, Color.cyan, relativeDensity);
            ColorParticle(particle, particleColor);
        }
    }

    private void ColorParticlesByHashId() {
        Dictionary<int, Color> hashIdColors = new Dictionary<int, Color>();
        foreach (GameObject particle in particles) {
            Particle particleDetails = particle.GetComponent<Particle>();
            if (!hashIdColors.ContainsKey(particleDetails.hash)) {
                hashIdColors[particleDetails.hash] = Random.ColorHSV();
            }
            ColorParticle(particle, hashIdColors[particleDetails.hash]);
        }
    }

    private void ColorParticle(GameObject particle, Color color) {
        Renderer renderer = particle.GetComponent<Renderer>();
        Material material = new Material(baseMaterial);
        material.color = color;
        renderer.material = material;
    }

    private Dictionary<int, List<GameObject>> CreateParticleHashMap() {
        Dictionary<int, List<GameObject>> particleMap = new Dictionary<int, List<GameObject>>();
        foreach (GameObject particle in particles) {
            int particleKey = GetHashFromRawPosition(particle.transform.position);
            if (!particleMap.ContainsKey(particleKey)) {
                particleMap[particleKey] = new List<GameObject>();
            }
            particleMap[particleKey].Add(particle);
        }
        return particleMap;
    }

    private void DestroyChildren() {
        for (int i = transform.childCount - 1; i >= 0; i--) {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    private int[] GetNeighborhoodHashIds(Vector2 position) {
        Vector2Int positionIndex = GetPositionIndex(position);
        int[] neighbors = new int[9];
        int neighborIdx = 0;
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                neighbors[neighborIdx++] = GetHashFromPositionIndex(positionIndex + i * Vector2Int.right + j * Vector2Int.up);
            }
        }
        return neighbors;
    }

    private int GetHashFromRawPosition(Vector2 position) {
        Vector2Int positionIndex = GetPositionIndex(position);
        return GetHashFromPositionIndex(positionIndex);
    }

    private Vector2Int GetPositionIndex(Vector2 position) {
        int xi = Mathf.FloorToInt(position.x / smoothingLength);
        int yi = Mathf.FloorToInt(position.y / smoothingLength);
        return new Vector2Int(xi, yi);
    }

    private int GetHashFromPositionIndex(Vector2Int index) {
        return ((index.x * PRIME1) ^ (index.y * PRIME2)) % HASH_TABLE_SIZE;
    }
}