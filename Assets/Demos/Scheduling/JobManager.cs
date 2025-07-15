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
using Kokowolo.Utilities;

namespace Kokowolo.Base.Demo.SchedulingDemo
{
    [DefaultExecutionOrder(-100)]
    public class JobManager : MonoBehaviourSingleton<JobManager>
    {
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        List<Job> scheduledJobs;
        List<Job> activeJobs;
        
        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        public bool IsFree => scheduledJobs.Count == 0 && !IsRunning;
        bool IsRunning => activeJobs.Count != 0;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        protected override void Singleton_OnDestroy()
        {
            ListPool.Release(ref scheduledJobs);
            ListPool.Release(ref activeJobs);
        }

        protected override void Singleton_Awake()
        {
            scheduledJobs = ListPool.Get<Job>();
            activeJobs = ListPool.Get<Job>();
        }

        public static Job StartJob(Action action, float time) => StartJob(new Job(action, time));
        public static Job StartJob(IEnumerator routine) => StartJob(new Job(routine));
        static Job StartJob(Job job)
        {
            Instance.activeJobs.Add(job);
            job.OnComplete += Instance.Handle_Job_OnComplete;
            Instance.StartCoroutine(job.Run());
            return job;
        }

        public static Job ScheduleJob(Action action, float time) => ScheduleJob(new Job(action, time));
        public static Job ScheduleJob(IEnumerator routine) => ScheduleJob(new Job(routine));
        static Job ScheduleJob(Job job)
        {
            Instance.scheduledJobs.Add(job);
            return job;
        }

        // public static JobSequence StartJobSequence(IEnumerator routine)
        // {
        //     JobSequence job = new JobSequence(routine);
        //     Instance.StartCoroutine(job.Run());
        //     return job;
        // }

        // void Run()
        // {
        //     if (IsRunning) return;
        //     StartCoroutine(RunRoutine());
        // }

        // IEnumerator RunRoutine()
        // {
            
        // }

        void Handle_Job_OnComplete(object sender, EventArgs e)
        {
            Job job = sender as Job;
            job.OnComplete -= Instance.Handle_Job_OnComplete;
            activeJobs.Remove(job);
            job.Dispose();
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}