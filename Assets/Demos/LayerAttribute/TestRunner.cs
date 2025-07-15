/*
 * File Name: TestRunner.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: April 9, 2024
 * 
 * Additional Comments:
 *      File Line Length: 140
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kokowolo.Utilities;

namespace Kokowolo.Base.Demo.LayerAttributeDemo
{
    public class TestRunner : MonoBehaviour
    {
        /************************************************************/
        #region Fields

        // [SerializeField, Utilities.Layer] string layerStr;
        [SerializeField, Utilities.Layer] int layerInt;
        [SerializeField] Bitmask bitmask;
        [SerializeField, Utilities.Layer] LayerMask layer;
        [SerializeField, Utilities.Layer] LayerMask defaultLayer;
        [SerializeField, Utilities.Layer] LayerMask multiLayer;
         
        #endregion
        /************************************************************/
        #region Properties

        #endregion
        /************************************************************/
        #region Functions

        private void OnValidate()
        {
            Debug.Log($"{layerInt} and {layer.value} and {defaultLayer.value} and {multiLayer.value} and {bitmask} and {bitmask.Value}");
        }

        #endregion
        /************************************************************/
    }
}