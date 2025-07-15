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
//     public class WaitForScheduler : CustomYieldInstruction, IDisposable
//     {
//         /*██████████████████████████████████████████████████████████*/
//         #region Fields

//         JobRunner scheduler;

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//         #region Properties

//         public override bool keepWaiting => scheduler.IsFree;

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//         #region Functions

//         bool disposed;
//         ~WaitForScheduler() => Dispose();
//         public void Dispose()
//         {
//             if (disposed) return;
//             disposed = true;

//             scheduler = null;
//         }

//         public WaitForScheduler(JobRunner scheduler)
//         {
//             this.scheduler = scheduler;
//         }

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//     }
// }