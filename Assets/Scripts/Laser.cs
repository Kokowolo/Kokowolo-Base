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

using Kokowolo.ProceduralMesh;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    /************************************************************/
    #region Fields

    [Header("Cached References")]
    [SerializeField] LineRenderer lineRenderer = null;
    [SerializeField] LaserDisplay laserDisplay = null;

    [Header("Settings")]
    [SerializeField] LayerMask layerMask;
    [SerializeField, Range(0, 1)] float laserSize = 0.01f;
    [SerializeField, Range(0, 100)] int bounceMax = 10;
    [SerializeField, Min(0)] float minAngle = 0.000000001f; // 11.4592982874 is ideal accuracy

    int bounceCount = 0;
    List<SphereMesh> spheres = new List<SphereMesh>(); 

    #endregion
    /************************************************************/
    #region Properties

    public float Angle => transform.rotation.eulerAngles.x;
    public float BounceCount => bounceCount;

    #endregion
    /************************************************************/
    #region Functions

    #region Unity Functions

    private void Start()
    {
        Refresh();
    }

    private void Update()
    {
        Rotate();
        Refresh();
        laserDisplay.SetText($"{transform.rotation.eulerAngles.x.ToString("F14")}º");
    }

    #endregion

    #region Other Functions

    public void Rotate()
    {
        transform.Rotate(new Vector3(minAngle, minAngle, minAngle) * Time.deltaTime);
        // Debug.Log(transform.rotation.eulerAngles.ToString("F16"));
        //Debug.Log(transform.rotation.eulerAngles.x);
    }

    public void Refresh()
    {
        lineRenderer.startWidth = lineRenderer.endWidth = laserSize;
        lineRenderer.positionCount = 0;
        bounceCount = 0;

        Vector3 origin = transform.position;
        Vector3 direction = transform.TransformDirection(transform.forward);

        AddLaserPoint(origin);
        ClearSpheres();
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
            TryAddSphere(hit.transform);
            if (bounceCount++ < bounceMax) DoLaserBounce(hit.point, directionOut);
        }
        else
        {
            AddLaserPoint(origin + directionIn * 150f);
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

    private void TryAddSphere(Transform transform)
    {
        if (!transform.TryGetComponent<SphereMesh>(out SphereMesh sphere)) return;
        if (spheres.Contains(sphere)) return;
        
        sphere.MeshRenderer.material = SphereManager.Instance.SphereMaterialLasered;
        spheres.Add(sphere);
    }

    private void ClearSpheres()
    {
        foreach (SphereMesh sphere in spheres)
        {
            sphere.MeshRenderer.material = SphereManager.Instance.SphereMaterialDefault;
        }
        spheres.Clear();
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
        Gizmos.color = Color.cyan;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Gizmos.DrawSphere(lineRenderer.GetPosition(i), gizmosSphereRadius * 0.5f);
        }
    }

    #endif
    #endregion
    /************************************************************/
}