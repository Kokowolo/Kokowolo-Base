/* 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: July 15, 2025
 * 
 * Additional Comments:
 *      File Line Length: ~140
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kokowolo.Base.Demo.SchedulingDemo
{
    public class WaitForJobManager : CustomYieldInstruction
    {
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        public override bool keepWaiting => !JobManager.Instance.IsFree;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        public WaitForJobManager()
        {
            // nada
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}