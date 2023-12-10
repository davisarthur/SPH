using UnityEngine;

public static class CubicSplineKernel
{
    // Function variables defined in https://arxiv.org/abs/2009.06944
    public static float Evaluate2D(Vector2 diff, float smoothingLength) {
        float sigma = 40f / (7f * Mathf.PI * Mathf.Pow(smoothingLength, 2f));
        float q = diff.magnitude / smoothingLength;
        if (q < 0.5f) {
            return sigma * (6f * (Mathf.Pow(q, 3f) - Mathf.Pow(q, 2f)) + 1f); 
        } else if (q < 1f) {
            return sigma * (2f * Mathf.Pow(1f - q, 3f));
        } else {
            return 0f;
        }
    }
}
