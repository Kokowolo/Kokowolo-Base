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
namespace Kokowolo.Base.Demo.SchedulingDemo
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
            base.Dispose();
            Utilities.ListPool.Release(ref jobs);
        }

        JobSequence() : base()
        {
            jobs = Utilities.ListPool.Get<Job>();
        }

        public static JobSequence Get()
        {
            JobSequence jobSequence = new JobSequence();
            jobSequence.routine = jobSequence.Routine();
            JobManager.PendJob(jobSequence);
            return jobSequence;
        }

        IEnumerator Routine()
        {
            while (jobs.Count != 0)
            {
                Job job = jobs[0];
                jobs.Remove(job);
                JobManager.StartJob(job);
                yield return new WaitForJob(job);
            }
        }

        public void Prepend(Action action, float time) => Prepend(new Job(action, time));
        public void Prepend(IEnumerator routine) => Prepend(new Job(routine));
        public void Prepend(Job job)
        {
            if (IsRunning || job.IsRunning)
            {
                Utilities.LogManager.Log($"{nameof(JobSequence)} job already running"); 
                return;
            }
            job.IsPending = false;
            jobs.Insert(0, job);
        }

        public void Append(Action action, float time) => Append(new Job(action, time));
        public void Append(IEnumerator routine) => Append(new Job(routine));
        public void Append(Job job)
        {
            if (IsRunning || job.IsRunning)
            {
                Utilities.LogManager.Log($"{nameof(JobSequence)} job already running"); 
                return;
            }
            job.IsPending = false;
            jobs.Add(job);
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}