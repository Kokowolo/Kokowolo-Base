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
using Kokowolo.Utilities;

namespace Kokowolo.Base.Demos.SchedulingDemo
{
    public class Job : IEquatable<Job>, IDisposable
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
        public bool IsRunning { get; private set; }

        internal bool IsPending { get; set; } // marks for pending job removal
        internal bool IsScheduled { get; set; }

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
            // LogManager.Log($"Disposing {this}");

            // Set data
            IsPending = false;

            // Complete
            if (complete)
            {
                OnCompleteInternal?.Invoke();
            }
            
            // Release resources
            if (coroutine != null) 
            {
                IsRunning = false;
                JobManager.Instance.StopCoroutine(coroutine);
                coroutine = null;
            }
            routine = null;
            OnDispose?.Invoke(this);
            OnDispose = null;
            OnStartInternal = null;
            OnCompleteInternal = null;
        }

        public static Job Get(Action function) => Get(function, -1);
        public static Job Get(Action function, float time) => new Job(function, time, isScheduled: false);
        public static Job Get(IEnumerator routine) => new Job(routine, isScheduled: false);
        public static Job Schedule(Action function) => Schedule(function, -1);
        public static Job Schedule(Action function, float time) => new Job(function, time, isScheduled: true);
        public static Job Schedule(IEnumerator routine) => new Job(routine, isScheduled: true);
        Job(Action function, float time, bool isScheduled) : this(Utils.InvokeFunctionAfterTime(function, time), isScheduled) {}
        Job(IEnumerator routine, bool isScheduled) : this(routine)
        {
            this.IsScheduled = isScheduled;
            JobManager.Instance.PendJob(this);
        }

        // called by JobSequence specifically and pend constructor
        internal Job(Action function, float time) : this(Utils.InvokeFunctionAfterTime(function, time)) {}
        internal Job(IEnumerator routine)
        {
            this.routine = routine;
            instanceId = id++;
        }

        internal void Start()
        {
            if (IsDisposed) 
            {
                throw new Exception($"[{nameof(Job)}] {nameof(Start)} called after {nameof(Dispose)}");
            }
            if (IsRunning) 
            {
                throw new Exception($"[{nameof(Job)}] {nameof(Start)} called twice");
            }
            // if (IsScheduled)
            // {
            //     throw new Exception($"[{nameof(Job)}] {nameof(Start)} called before unscheduling");
            // }
            IsRunning = true;
            coroutine = JobManager.Instance.StartCoroutine(Run());
        }

        internal IEnumerator Run()
        {
            // LogManager.Log($"Running {this}");
            OnStartInternal?.Invoke();
            yield return routine;
            Dispose(complete: true);
        }

        public Job OnStart(JobCallback callback)
        {
            if (IsDisposed) 
            {
                Kokowolo.Utilities.LogManager.LogWarning($"callback cannot be added on disposed job");
                return null;
            }
            OnStartInternal += callback;
            return this;
        }

        public Job OnComplete(JobCallback callback)
        {
            if (IsDisposed) 
            {
                Kokowolo.Utilities.LogManager.LogWarning($"callback cannot be added on disposed job");
                return null;
            }
            OnCompleteInternal += callback;
            return this;
        }

        public bool Equals(Job other) => this == other;
        public override bool Equals(object obj) => Equals(obj as Job);
        public static bool operator !=(Job a, Job b) => !(a == b);
        public static bool operator ==(Job a, Job b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null) return false;
            if (b is null) return false;
            return a.instanceId == b.instanceId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(instanceId);
        }

        public override string ToString()
        {
            return $"{nameof(Job)}{(IsScheduled ? "(s)" : "")}:{instanceId}";
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}