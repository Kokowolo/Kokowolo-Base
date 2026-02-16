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

        void OnValidate()
        {
            new TestObj(3);
        }
        
        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}

public class TestObj
{
    public TestObj()
    {
        Debug.Log("base");
    }

    public TestObj(int a) : this()
    {
        Debug.Log("a");
    }
}