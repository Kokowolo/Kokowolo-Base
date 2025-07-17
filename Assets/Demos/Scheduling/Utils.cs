/* 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: July 17, 2025
 * 
 * Additional Comments:
 *      File Line Length: ~140
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kokowolo.Utilities;

namespace Kokowolo.Base.Demos.SchedulingDemo
{
    public static class Utils
    {
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        public static IEnumerator InvokeFunctionAfterTime(Action function, float time)
        {
            if (time == 0)
            {
                yield return null;
            }
            else if (time > 0)
            {
                yield return new WaitForSeconds(time);
            }
            function.Invoke();
        }
        
        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}