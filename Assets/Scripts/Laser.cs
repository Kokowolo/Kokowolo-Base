/*
 * File Name: Laser.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: April 27, 2022
 * 
 * Additional Comments:
 *		While this file has been updated to better fit this project, the original version can be found here:
 *		https://catlikecoding.com/unity/tutorials/hex-map/
 *
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kokowolo.Utilities;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    /************************************************************/
    #region Fields

    [Header("Cached References")]
    [SerializeField] LineRenderer lineRenderer = null;

    [Header("Settings")]
    [SerializeField] LayerMask layerMask;
    [SerializeField, Range(0, 50)] int bounceMax = 10;
    [SerializeField, Range(0, 10)] float rotationSpeed = 10f;

    int bounceCount = 0;

    #endregion
    /************************************************************/
    #region Functions

    #region Unity Functions

    private void Start()
    {
        Refresh();
        // gameObject.transform.f
    }

    private void Update()
    {
        Rotate();
        Refresh();
    }

    #endregion

    #region Other Functions

    public void Rotate()
    {
        float min = 0.00000001f;
        Vector3 minAngle = new Vector3(min, min, min) * rotationSpeed * Time.deltaTime;//MathUtils.Remap(rotationSpeed, 0, 10, 0, 0);
        transform.Rotate(minAngle);
        // Debug.Log(transform.rotation.eulerAngles.ToString("F16"));
        //Debug.Log(transform.rotation.eulerAngles.x);
    }

    public void Refresh()
    {
        lineRenderer.positionCount = 0;
        bounceCount = 0;

        Vector3 origin = transform.position;
        Vector3 direction = transform.TransformDirection(transform.forward);

        AddLaserPoint(origin);
        DoLaserBounce(origin, direction);
    }

    private void DoLaserBounce(Vector3 origin, Vector3 directionIn)
    {
        if (Physics.Raycast(origin, directionIn, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            Vector3 directionOut = GetBounceOutVector(directionIn, hit.normal);
            //Debug.DrawRay(transform.position, direction * 10f, Color.green);
            //Debug.DrawRay(hit.point, hit.normal * 10f, Color.yellow);
            //Debug.DrawRay(hit.point, directionOut * 10f, Color.red);
            AddLaserPoint(hit.point);
            if (bounceCount++ < bounceMax) DoLaserBounce(hit.point, directionOut);
        }
        else
        {
            AddLaserPoint(origin + directionIn * 10f);
            //Debug.DrawRay(origin, directionIn * 1000, Color.white);
        }
    }

    private Vector3 GetBounceOutVector(Vector3 v, Vector3 n)
    {
        // thanks to Gareth Rees's answer https://stackoverflow.com/questions/573084/how-to-calculate-bounce-angle
        Vector3 u = Vector3.Dot(v, n) * n;
        Vector3 w = v - u;
        return w - u;
    }

    private void AddLaserPoint(Vector3 point)
    {
        int index = lineRenderer.positionCount;
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(index, point);
    }

    #endregion

    #endregion
    /************************************************************/
    #region Debug
    #if UNITY_EDITOR

    [Header("Debug")]
    [SerializeField, Range(0, 2)] float gizmosSphereRadius = 0.1f;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Gizmos.DrawSphere(lineRenderer.GetPosition(i), gizmosSphereRadius * 0.5f);
        }
    }

    #endif
    #endregion
    /************************************************************/
}