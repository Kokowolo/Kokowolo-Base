/*
 * File Name: Curved.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: April 27, 2022
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kokowolo.Utilities;

public class Curved : MonoBehaviour
{
    /************************************************************/
    #region Fields

    [Header("Cached References")]
    [SerializeField] LineRenderer lineRenderer = null;

    [Header("Settings")]
    [Tooltip("array of points for curve")]
    [SerializeField] Vector3[] points = null;

    //[SerializeField, Min(0)] float smoothingLength = 0f;

    [SerializeField, Min(0)] int smoothingSections = 0;


    #endregion
    /************************************************************/
    #region Functions

    private void EnsureCorrectLineRenderPositionCountLinear()
    {
        int positionCount = points.Length + (points.Length - 1) * smoothingSections;
        lineRenderer.positionCount = positionCount;

        int count = 0;
        for (int i = 0; i < points.Length; i++)
        {
            for (int j = 0; i != 0 && j < smoothingSections; j++)
            {
                float t = 1f / (smoothingSections + 1f) * (j + 1f);
                lineRenderer.SetPosition(count++, Vector3.Lerp(points[i - 1], points[i], t));
            }
            lineRenderer.SetPosition(count++, points[i]);
        }
    }

    //private void EnsureCorrectLineRenderPositionCountQuadraticBezier()
    //{
    //    // HACK: this is broken
    //    int positionCount = points.Length + (points.Length - 1) * smoothingSections;
    //    lineRenderer.positionCount = positionCount;

    //    Vector3 a, b, c = points[0];

    //    int count = 0;
    //    for (int i = 1; i < points.Length; i++)
    //    {
    //        a = c;
    //        b = points[i - 1];
    //        c = (b + points[i]) * 0.5f;
    //        for (float t = 0; t < 1f; t += 1f / (smoothingSections + 1))
    //        {
    //            lineRenderer.SetPosition(count++, Bezier.GetQuadraticPoint(a, b, c, t));
    //        }
    //    }

    //    a = c;
    //    b = points[points.Length - 1];
    //    c = b;
    //    for (float t = 0; t < 1f; t += 1f / (smoothingSections + 1))
    //    {
    //        lineRenderer.SetPosition(count++, Bezier.GetQuadraticPoint(a, b, c, t));
    //    }
    //}

    #endregion
    /************************************************************/
    #region Debug
    #if UNITY_EDITOR

    [Header("Debug")]
    [SerializeField, Range(0, 2)] float gizmosSphereRadius = 0.1f;

    private void OnValidate()
    {
        EnsureCorrectLineRenderPositionCountLinear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.DrawSphere(points[i], gizmosSphereRadius);
        }

        Gizmos.color = Color.white;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Gizmos.DrawSphere(lineRenderer.GetPosition(i), gizmosSphereRadius * 0.5f);
        }
    }

    #endif
    #endregion
    /************************************************************/
}