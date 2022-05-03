/*
 * File Name: LaserDisplay.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: April 30, 2022
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
using TMPro;

public class LaserDisplay : MonoBehaviour
{
    /************************************************************/
    #region Fields

    [SerializeField] LineRenderer lineRenderer = null;
    [SerializeField] TMP_Text text = null;
    [SerializeField] GameObject pointStart = null;
    [SerializeField] GameObject pointEnd = null;

    #endregion
    /************************************************************/
    #region Functions

    private void Awake()
    {
        lineRenderer.positionCount = 2;
    }

    private void Start()
    {
        Refresh();
    }

    private void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        lineRenderer.SetPosition(0, pointStart.transform.position);
        lineRenderer.SetPosition(1, pointEnd.transform.position);
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }

    #endregion
    /************************************************************/
}