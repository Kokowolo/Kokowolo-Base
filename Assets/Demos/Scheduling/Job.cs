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

        internal event JobCallback<Job> OnDispose;

        // event EventHandler OnCompleteInternal;
        internal event JobCallback onComplete;
        // {
        //     add
        //     {
        //         IsPending = false;
        //         OnCompleteInternal += value;
        //     }
        //     remove
        //     {
        //         // ValidateInvocationList = true;
        //         OnCompleteInternal -= value;
        //     }
        // }

        // event EventHandler OnStartInternal;
        internal event JobCallback onStart;
        // {
        //     add
        //     {
        //         IsPending = false;
        //         OnStartInternal += value;
        //     }
        //     remove
        //     {
        //         // ValidateInvocationList = true;
        //         OnStartInternal -= value;
        //     }
        // }

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        static int id = 0;
        internal int instanceId;

        protected IEnumerator routine;
        Coroutine coroutine;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        public bool IsDisposed => disposed;
        public bool IsRunning => coroutine != null;

        internal bool IsPending { get; set; } = true;
        // internal bool ValidateInvocationList { get; private set; } = false;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        protected bool disposed;
        ~Job() => Dispose(complete: false);
        void IDisposable.Dispose() => Dispose(complete: false);

        public virtual void Dispose(bool complete = false)
        {
            if (disposed) return;
            disposed = true;

            // Complete
            if (complete)
            {
                onComplete?.Invoke();
            }
            
            // Release resources
            if (coroutine != null) 
            {
                JobManager.Instance.StopCoroutine(coroutine);
                coroutine = null;
            }
            routine = null;
            OnDispose?.Invoke(this);
            OnDispose = null;
            onStart = null;
            onComplete = null;
        }

        static IEnumerator Routine(Action function, float time)
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

        public static Job Get(Action function, float time)
        {
            return Get(Routine(function, time));
        }

        public static Job Get(IEnumerator routine)
        {
            Job job = new Job(routine);
            JobManager.PendJob(job);
            return job;
        }

        public static Job Schedule(Action function, float time)
        {
            return JobManager.ScheduleJob(new Job(Routine(function, time)));
        }

        public static Job Schedule(IEnumerator routine)
        {
            return JobManager.ScheduleJob(new Job(routine));
        }

        internal Job (Action function, float time) : this(Routine(function, time)) {}
        internal Job(IEnumerator routine) : this() 
        {
            this.routine = routine;
        }

        protected Job()
        {
            instanceId = id++;
        }

        internal void Start()
        {
            if (disposed) 
            {
                throw new Exception($"[{nameof(Job)}] {nameof(Start)} called after {nameof(Dispose)}");
            }
            if (coroutine != null) 
            {
                throw new Exception($"[{nameof(Job)}] {nameof(Start)} called twice");
            }
            coroutine = JobManager.Instance.StartCoroutine(RunRoutine());
        }

        IEnumerator RunRoutine()
        {
            onStart?.Invoke();
            yield return routine;
            Dispose(complete: true);
        }

        public Job OnStart(JobCallback callback)
        {
            onStart += callback;
            return this;
        }

        public Job OnComplete(JobCallback callback)
        {
            onComplete += callback;
            return this;
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}