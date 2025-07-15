/* 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: July 14, 2025
 * 
 * Additional Comments:
 *      File Line Length: ~140
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kokowolo.Base.Demo.SchedulingDemo
{
    public class Job : IDisposable
    {
        /*██████████████████████████████████████████████████████████*/
        #region Events

        public event EventHandler OnStart;
        public event EventHandler OnComplete;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        static int id = 0;
        internal int instanceId;

        bool isActive = true;
        internal IEnumerator routine;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        // public IEnumerator Routine => routine;
        public bool IsActive => isActive;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        bool disposed;
        ~Job() => Dispose();
        public virtual void Dispose()
        {
            if (disposed) return;
            disposed = true;
            routine = null;
        }

        internal Job(Action function, float time) : this(InvokeFunctionAfterTime(function, time)) {}
        internal Job(IEnumerator routine)
        {
            instanceId = id++;
            this.routine = routine;
        }

        internal void Complete()
        {
            isActive = false;
            OnComplete?.Invoke(this, EventArgs.Empty);
        }

        internal void Start()
        {
            OnStart?.Invoke(this, EventArgs.Empty);
        }

        internal IEnumerator Run()
        {
            OnStart?.Invoke(this, EventArgs.Empty);
            yield return routine;
            OnComplete?.Invoke(this, EventArgs.Empty);
        }
    
        public void Kill()
        {
            isActive = false;
        }

        static IEnumerator InvokeFunctionAfterTime(Action function, float time)
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