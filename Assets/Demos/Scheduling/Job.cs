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

namespace Kokowolo.Base.Demos.SchedulingDemo
{
    public class Job : IDisposable
    {
        /*██████████████████████████████████████████████████████████*/
        #region Events

        internal event JobCallback<Job> OnDispose;

        internal event JobCallback OnCompleteInternal;
        internal event JobCallback OnStartInternal;

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

        internal bool IsScheduled { get; private set; }
        internal bool IsPending { get; set; } = true;

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
                OnCompleteInternal?.Invoke();
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
            OnStartInternal = null;
            OnCompleteInternal = null;
        }

        internal Job (Action function, float time) : this(Utils.InvokeFunctionAfterTime(function, time)) {}
        internal Job(IEnumerator routine)
        {
            this.routine = routine;
            instanceId = id++;
        }

        public static Job Get(Action function, float time) => Get(Utils.InvokeFunctionAfterTime(function, time));
        public static Job Get(IEnumerator routine)
        {
            Job job = new Job(routine);
            JobManager.PendJob(job);
            return job;
        }

        public static Job Schedule(Action function, float time) => Schedule(Utils.InvokeFunctionAfterTime(function, time));
        public static Job Schedule(IEnumerator routine)
        {
            Job job = new Job(routine);
            return JobManager.ScheduleJob(job);
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
            JobManager.Instance.activeJobs.Add(this);
            coroutine = JobManager.Instance.StartCoroutine(RunRoutine());
        }

        IEnumerator RunRoutine()
        {
            OnStartInternal?.Invoke();
            yield return routine;
            Dispose(complete: true);
        }

        public Job OnStart(JobCallback callback)
        {
            OnStartInternal += callback;
            return this;
        }

        public Job OnComplete(JobCallback callback)
        {
            OnCompleteInternal += callback;
            return this;
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}