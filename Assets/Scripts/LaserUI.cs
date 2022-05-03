/*
 * File Name: LaserUI.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: May 3, 2022
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class LaserUI : MonoBehaviour
{
    /************************************************************/
    #region Fields

    [SerializeField] Laser laser = null;
    [SerializeField] TMP_Text textAngle = null;
    [SerializeField] TMP_Text textBounces = null;

    #endregion
    /************************************************************/
    #region Functions

    private void LateUpdate()
    {
        textAngle.text = $"Angle: {laser.Angle.ToString("F11")}º";
        textBounces.text = $"Bounces: {laser.BounceCount}";
    }

    #endregion
    /************************************************************/
}