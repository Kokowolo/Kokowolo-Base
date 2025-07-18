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

        bool ValidateSequence()
        {
            if (!IsRunning) return true;
            Utilities.LogManager.LogWarning($"job already running"); 
            return false;
        }

        public Job Prepend(Action function) => Prepend(function, -1);
        public Job Prepend(Action function, float time) => ValidateSequence() ? Prepend(new Job(function, time)) : null;
        public Job Prepend(IEnumerator routine) => ValidateSequence() ? Prepend(new Job(routine)) : null;
        Job Prepend(Job job)
        {
            job.IsScheduled = true;
            jobs.Insert(0, job);
            return job;
        }

        public Job Append(Action function) => Append(function, -1);
        public Job Append(Action function, float time) => ValidateSequence() ? Append(new Job(function, time)) : null;
        public Job Append(IEnumerator routine) => ValidateSequence() ? Append(new Job(routine)) : null;
        Job Append(Job job)
        {
            job.IsScheduled = true;
            jobs.Add(job);
            return job;
        }

        public override string ToString()
        {
            return $"{nameof(JobSequence)}{(IsScheduled ? "(s)" : "")}:{instanceId}";
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}