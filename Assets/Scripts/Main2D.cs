using UnityEngine;

public class Main2D : MonoBehaviour
{
    public Material baseMaterial;
    public Vector2 bottomLeft;
    public Vector2 topRight;
    public int numParticles;
    public float smoothingLength;
    public GameObject particlePrefab;

    private float maxDensity = float.NegativeInfinity;
    private float minDensity = float.PositiveInfinity;

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
        for (int i = 0; i < particles.Length; i++) {
            GameObject particleI = particles[i];
            float densityI = 0f;
            for (int j = 0; j < particles.Length; j++) {
                if (i == j) continue;
                GameObject particleJ = particles[j];
                Vector2 diff = particleJ.transform.position - particleI.transform.position;
                float massJ = particleJ.GetComponent<Particle>().mass;
                densityI += massJ * CubicSplineKernel.Evaluate2D(diff, smoothingLength);
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

    private void ColorParticlesByDensity() {
        foreach (GameObject particle in particles) {
            float relativeDensity = Mathf.InverseLerp(minDensity, maxDensity, particle.GetComponent<Particle>().density);
            Renderer renderer = particle.GetComponent<Renderer>();
            Material material = new Material(baseMaterial);
            material.color = Color.Lerp(Color.yellow, Color.cyan, relativeDensity);
            renderer.material = material; 
        }
    }

    private void DestroyChildren() {
        for (int i = transform.childCount - 1; i >= 0; i--) {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
