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

        internal event EventHandler OnDispose;
        public event EventHandler OnComplete;
        public event EventHandler OnStart;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        static int id = 0;
        internal int instanceId;

        bool isActive = true;

        IEnumerator routine;
        Coroutine coroutine;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        public bool IsActive => isActive && !disposed;
        bool IsRunning => coroutine != null;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        bool disposed;
        ~Job() => Dispose(complete: false);
        void IDisposable.Dispose() => Dispose(complete: false);

        public virtual void Dispose(bool complete)
        {
            if (disposed) return;
            disposed = true;

            // Complete
            if (complete && isActive)
            {
                OnComplete?.Invoke(this, EventArgs.Empty);
            }
            isActive = false;
            
            // Release resources
            if (coroutine != null) 
            {
                JobManager.Instance.StopCoroutine(coroutine);
                coroutine = null;
            }
            routine = null;
            OnDispose?.Invoke(this, EventArgs.Empty);
        }

        internal Job(Action function, float time) : this(InvokeFunctionAfterTime(function, time)) {}
        internal Job(IEnumerator routine)
        {
            instanceId = id++;
            this.routine = routine;
        }

        internal void Start()
        {
            if (disposed) 
            {
                throw new Exception($"[{nameof(Job)}] {nameof(Start)} called after {nameof(Dispose)}");
            }
            coroutine = JobManager.Instance.StartCoroutine(RunRoutine());
        }

        IEnumerator RunRoutine()
        {
            OnStart?.Invoke(this, EventArgs.Empty);
            yield return routine;
            Dispose(complete: true);
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