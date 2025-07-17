/* 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: July 16, 2025
 * 
 * Additional Comments:
 *      File Line Length: ~140
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kokowolo.Base.Demos.TestRunnerDemo
{
    public class TestRunner1 : MonoBehaviour
    {
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        
            
        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        void Start()
        {
           StartCoroutine(Routine());
        }

        void Update()
        {
           if (Input.GetMouseButtonDown(0)) enabled = false;
        }
        
        IEnumerator Routine()
        {
            while (true)
            {
                Debug.Log("hi");
                yield return new WaitForSeconds(1);
            }
            
        }
        
        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}
