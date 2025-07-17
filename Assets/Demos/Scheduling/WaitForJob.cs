/* 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: July 15, 2025
 * 
 * Additional Comments:
 *      File Line Length: ~140
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kokowolo.Base.Demos.SchedulingDemo
{
    public class WaitForJob : CustomYieldInstruction, IDisposable
    {
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        Job job;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        public override bool keepWaiting => !job.IsDisposed;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        bool disposed;
        ~WaitForJob() => Dispose();
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            job = null;
        }
        
        public WaitForJob(Job job)
        {
            this.job = job;
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}