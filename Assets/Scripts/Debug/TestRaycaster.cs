// /*
//  * File Name: TestRaycaster.cs
//  * Description: This script is for ...
//  * 
//  * Author(s): Kokowolo, Will Lacey
//  * Date Created: October 18, 2022
//  * 
//  * Additional Comments:
//  *		File Line Length: 120
//  */

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;

// using Kokowolo.Utilities;

// public class TestRaycaster : MonoBehaviour
// {
//     /************************************************************/
//     #region Fields

//     [SerializeField] LayerMask layerMask;
//     [SerializeField] GameObject prefab;
//     [Tooltip("if 0, do not destroy")]
//     [SerializeField, Min(0)] float waitForSeconds = 1f;

//     #endregion
// 	/************************************************************/
//     #region Properties

//     #endregion
//     /************************************************************/
//     #region Functions

//     private void Update()
//     {
//         if (!BaseInputManager.WasClickPressedThisFrame()) return;

//         DrawRaycastToWorldPosition();
//     }

//     private void DrawRaycastFromMouseScreenPoint()
//     {
//         Vector3 origin = Camera.main.ScreenToWorldPoint(BaseInputManager.GetMouseScreenPoint());
//         float maxDistance = float.MaxValue;
//         Color color = Color.white;

//         if (Raycasting.RaycastFromMouseScreenPoint(out RaycastHit hitInfo, layerMask, maxDistance))
//         {
//             Debug.Log("Hit");
//         }
//         else
//         {
//             color = Color.red;
//             Debug.Log("No Hit");
//         }

//         StartCoroutine(ShowLine(origin, hitInfo.point, color));
//     }

//     private void DrawRaycastToWorldPosition()
//     {
//         Vector3 origin = Camera.main.ScreenToWorldPoint(BaseInputManager.GetMouseScreenPoint());
//         Vector3 destination = FindObjectOfType<Rotator>().transform.position;
//         Color color = Color.white;
//         if (Raycasting.RaycastToDestinationPoint(destination, origin, out RaycastHit hitInfo, layerMask))
//         {
//             Debug.Log("Hit");
//         }
//         else
//         {
//             color = Color.red;
//             Debug.Log("No Hit");
//         }

//         StartCoroutine(ShowLine(origin, destination, color));
//     }

//     private void DrawRaycastBothWays()
//     {
//         // Vector3 origin = Camera.main.ScreenToWorldPoint(InputManager.GetMouseScreenPoint());
//         // RaycastHit raycastHit;
//         // float maxDistance = float.MaxValue;
//         // Color color = Color.white;

//         // if (!Raycasting.RaycastBothWays(out raycastHit, layerMask, maxDistance)) 
//         // {
//         //     Debug.Log("Hit");
//         // }
//         // else
//         // {
//         //     color = Color.red;
//         //     Debug.Log("No Hit");
//         // }
//         // StartCoroutine(ShowLine(origin, raycastHit.point, color));
//     }

//     private IEnumerator ShowLine(Vector3 origin, Vector3 destination, Color color)
//     {
//         LineRenderer lineRenderer = Instantiate(prefab).GetComponent<LineRenderer>();
//         lineRenderer.positionCount = 2;
//         lineRenderer.startColor = lineRenderer.endColor = color;
//         lineRenderer.SetPosition(0, origin);
//         lineRenderer.SetPosition(1, destination);

//         if (waitForSeconds != 0)
//         {
//             yield return new WaitForSeconds(waitForSeconds);
//             Destroy(lineRenderer.gameObject);
//         }
//     }
    
//     #endregion
//     /************************************************************/
// }