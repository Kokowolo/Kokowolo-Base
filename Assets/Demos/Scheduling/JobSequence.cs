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
using DG.Tweening;
namespace Kokowolo.Base.Demos.SchedulingDemo
{
    public class JobSequence : Job
    {
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        List<Job> jobs;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        ~JobSequence() => Dispose(complete: false);
        public override void Dispose(bool complete = false)
        {
            if (disposed) return;
            base.Dispose(complete);
            Utilities.ListPool.Release(ref jobs);
        }

        public static JobSequence Get() => new JobSequence(isScheduled: false);
        public static JobSequence Schedule() => new JobSequence(isScheduled: true);
        protected JobSequence(bool isScheduled) : base(null) // can't provide Routine() in header
        {
            jobs = Utilities.ListPool.Get<Job>();
            routine = Routine();
            this.IsScheduled = isScheduled;
            JobManager.Instance.PendJob(this);
        }
        
        IEnumerator Routine()
        {
            while (jobs.Count != 0)
            {
                Job job = jobs[0];
                jobs.Remove(job);
                yield return job.Run();
                // yield return new WaitForJob(job);
            }
        }

        bool ValidateAddedJob(Job job)
        {
            if (IsRunning || job.IsRunning) 
            {
                Utilities.LogManager.LogWarning($"{nameof(JobSequence)} job already running"); 
                return false;
            }
            if (job.IsScheduled) 
            {
                Utilities.LogManager.LogWarning($"{nameof(JobSequence)} job already scheduled"); 
                return false;
            }
            job.IsPending = false;
            job.IsScheduled = true;
            return true;
        }

        public void Prepend(Action function) => Prepend(function, -1);
        public void Prepend(Action function, float time) => Prepend(new Job(function, time));
        public void Prepend(IEnumerator routine) => Prepend(new Job(routine));
        public void Prepend(Job job)
        {
            if (ValidateAddedJob(job)) jobs.Insert(0, job);
        }

        public void Append(Action function) => Append(function, -1);
        public void Append(Action function, float time) => Append(new Job(function, time));
        public void Append(IEnumerator routine) => Append(new Job(routine));
        public void Append(Job job)
        {
            if (ValidateAddedJob(job)) jobs.Add(job);
        }

        public override string ToString()
        {
            return $"{nameof(JobSequence)}{(IsScheduled ? "(s)" : "")}:{instanceId}";
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}