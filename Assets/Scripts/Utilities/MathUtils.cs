using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    /************************************************************/
    #region Fields

    static Texture2D _noiseSource;

    #endregion
    /************************************************************/
    #region Properties

    public static float noiseSampleScale { get; set; } = 0.2f;
    public static float maxHorizontalNoiseDelta { get; set; } = 0.2f;
    public static float maxVerticalNoiseDelta { get; set; } = 0.003f;

    public static Texture2D noiseSource
    {
        get
        {
            if (_noiseSource == null) _noiseSource = Resources.Load<Texture2D>("noise_Perlin");
            return _noiseSource;
        }
        set => _noiseSource = value;
    }

    #endregion
    /************************************************************/
    #region Functions

    #region Noise & Randomness Functions

    public static Vector3 Perturb(Vector3 point, bool perturbHorizontal = true, bool perturbVertical = true)
    {
        return Perturb(point, maxHorizontalNoiseDelta, maxVerticalNoiseDelta, perturbHorizontal, perturbVertical);
    }

    public static Vector3 Perturb(
        Vector3 point, float maxNoiseDelta,
        bool perturbHorizontal = true, bool perturbVertical = true)
    {
        return Perturb(point, maxNoiseDelta, maxNoiseDelta, perturbHorizontal, perturbVertical);
    }

    public static Vector3 Perturb(
        Vector3 point,
        float maxHorizontalNoiseDelta, float maxVerticalNoiseDelta, bool perturbHorizontal, bool perturbVertical)
    {
        // samples the noise source for randomness using a given point, yields a random value between 0 and 1
        Vector4 sample = noiseSource.GetPixelBilinear(point.x * noiseSampleScale, point.z * noiseSampleScale);

        // convert the sample to a value between -1 and 1, then multiply it by it corresponding noise strength
        if (perturbHorizontal)
        {
            point.x += (sample.x * 2f - 1f) * maxHorizontalNoiseDelta;
            point.z += (sample.z * 2f - 1f) * maxHorizontalNoiseDelta;
        }
        if (perturbVertical)
        {
            point.y += (sample.y * 2f - 1f) * maxVerticalNoiseDelta;
        }

        return point;
    }

    public static Vector3 GetRandomVector3(float randomness) // HACK: is this really needed?
    {
        return new Vector3
        {
            x = Random.Range(-randomness, randomness),
            y = Random.Range(-randomness, randomness),
            z = Random.Range(-randomness, randomness)
        };
    }

    public static Vector3 GetRandomVector3(float from, float to) // HACK: is this really needed?
    {
        return new Vector3
        {
            x = Random.Range(from, to),
            y = Random.Range(from, to),
            z = Random.Range(from, to)
        };
    }

    #endregion

    #region Other Functions

    /// <summary>
    /// Remaps a value from one range to a value from another
    /// </summary>
    public static float Remap(float value, float min1, float max1, float min2, float max2)
    {
        value = Mathf.Clamp(value, min1, max1);
        return (value - min1) / (max1 - min1) * (max2 - min2) + min2;
    }

    #endregion

    #endregion
    /************************************************************/
}
