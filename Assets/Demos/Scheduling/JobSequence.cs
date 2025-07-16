// /* 
//  * Author(s): Kokowolo, Will Lacey
//  * Date Created: July 14, 2025
//  * 
//  * Additional Comments:
//  *      File Line Length: ~140
//  */

// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace Kokowolo.Base.Demo.SchedulingDemo
// {
//     public class JobSequence : Job
//     {
//         /*██████████████████████████████████████████████████████████*/
//         #region Fields



//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//         #region Properties

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//         #region Functions

//         protected JobSequence() : base()
//         {
//             // 
//         }

//         public void Prepend()
//         {
//             if (IsRunning)
//             {
//                 Utilities.LogManager.Log($"{nameof(JobSequence)} is already running"); 
//                 return;
//             }
//         }

//         public void Append(Action action, float time)
//         public void Append(IEnumerator routine)
//         void Append()
//         {
//             if (IsRunning)
//             {
//                 Utilities.LogManager.Log($"{nameof(JobSequence)} is already running"); 
//                 return;
//             }
//         }

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//     }
// }