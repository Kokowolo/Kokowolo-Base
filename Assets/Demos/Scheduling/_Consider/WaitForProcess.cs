// /* 
//  * Author(s): Kokowolo, Will Lacey
//  * Date Created: July 15, 2025
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
//     public class WaitForJob : CustomYieldInstruction, IDisposable
//     {
//         /*██████████████████████████████████████████████████████████*/
//         #region Fields

//         Job process;

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//         #region Properties

//         public override bool keepWaiting => process.IsActive;

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//         #region Functions

//         bool disposed;
//         ~WaitForJob() => Dispose();
//         public void Dispose()
//         {
//             if (disposed) return;
//             disposed = true;

//             process = null;
//         }
        
//         public WaitForJob(Job process)
//         {
//             this.process = process;
//         }

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//     }
// }