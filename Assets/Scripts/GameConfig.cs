using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : ScriptableObject
{
    /************************************************************/
    #region Fields

    [Header("Noise Settings")]

    [Tooltip("source of map noise")]
    [SerializeField] Texture2D _noiseSource;

    [Tooltip("max possible horizontal grid noise; max displacement will equal [2 * (value ** 2)] ** 0.5")]
    [SerializeField, Min(0f)] float _maxHorizontalNoiseDelta = 0f;

    [Tooltip("max possible vertical height noise")]
    [SerializeField, Min(0f)] float _maxVerticalNoiseDelta = 1f;

    [Tooltip("how often the noise repeats itself; repeats every [1 / (2 * noiseScale * innerRadius)]")]
    [SerializeField, Min(0f)] float _noiseSampleScale = 0.003f;

    #endregion
    /************************************************************/
    #region Properties 
    
    public Texture2D noiseSource => _noiseSource;
    
    public float maxHorizontalNoiseDelta => _maxHorizontalNoiseDelta;
    
    public float maxVerticalNoiseDelta => _maxVerticalNoiseDelta;

    public float noiseSampleScale => _noiseSampleScale;

    #endregion
    /************************************************************/
}
