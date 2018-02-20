using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MediaServices.Client;
using System.IO;
using System.Threading;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Collections.Generic;

namespace VideoModeration
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create Azure Media Context
            Helpers.CreateMediaContext();

            // Use a file as the input.
            IAsset asset = Helpers.CreateAssetfromFile();

            // Then submit the asset to Content Moderator
            RunContentModeratorJob(asset);
        }

        /// <summary>
        /// Run the Content Moderator job on the designated Asset from local file or blob storage
        /// </summary>
        /// <param name="asset"></param>
        static void RunContentModeratorJob(IAsset asset)
        {
            // Grab the presets
            string configuration = File.ReadAllText(Globals.CONTENT_MODERATOR_PRESET_FILE);

            // grab instance of Azure Media Content Moderator MP
            IMediaProcessor mp = Globals._context.MediaProcessors.GetLatestMediaProcessorByName(Globals.MEDIA_PROCESSOR);

            // create Job with Content Moderator task
            IJob job = Globals._context.Jobs.Create(String.Format("Content Moderator {0}",
                asset.AssetFiles.First() + "_" + Guid.NewGuid()));

            ITask contentModeratorTask = job.Tasks.AddNew("Adult and racy classifier task",
                mp, configuration,
                TaskOptions.None);
            contentModeratorTask.InputAssets.Add(asset);
            contentModeratorTask.OutputAssets.AddNew("Adult and racy classifier output",
            AssetCreationOptions.None);

            job.Submit();


            // Create progress printing and querying tasks
            Task progressPrintTask = new Task(() =>
            {
                IJob jobQuery = null;
                do
                {
                    var progressContext = Globals._context;
                    jobQuery = progressContext.Jobs
                    .Where(j => j.Id == job.Id)
                    .First();
                    Console.WriteLine(string.Format("{0}\t{1}",
                    DateTime.Now,
                    jobQuery.State));
                    Thread.Sleep(10000);
                }
                while (jobQuery.State != JobState.Finished &&
                jobQuery.State != JobState.Error &&
                jobQuery.State != JobState.Canceled);
            });
            progressPrintTask.Start();


            Task progressJobTask = job.GetExecutionProgressTask(
            CancellationToken.None);
            progressJobTask.Wait();

            // If job state is Error, the event handling 
            // method for job progress should log errors.  Here we check 
            // for error state and exit if needed.
            if (job.State == JobState.Error)
            {
                ErrorDetail error = job.Tasks.First().ErrorDetails.First();
                Console.WriteLine(string.Format("Error: {0}. {1}",
                error.Code,
                error.Message));
            }

            DownloadAsset(job.OutputMediaAssets.First(), Globals.OUTPUT_FOLDER);
        }

        /// <summary>
        /// Download the given asset to the output directory
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="outputDirectory"></param>
        static void DownloadAsset(IAsset asset, string outputDirectory)
        {
            foreach (IAssetFile file in asset.AssetFiles)
            {
                file.Download(Path.Combine(outputDirectory, file.Name));
            }
        }

        /// <summary>
        /// Event handler for job state to log job progress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void StateChanged(object sender, JobStateChangedEventArgs e)
        {
            Console.WriteLine("Job state changed event:");
            Console.WriteLine("  Previous state: " + e.PreviousState);
            Console.WriteLine("  Current state: " + e.CurrentState);
            switch (e.CurrentState)
            {
                case JobState.Finished:
                    Console.WriteLine();
                    Console.WriteLine("Job finished.");
                    break;
                case JobState.Canceling:
                case JobState.Queued:
                case JobState.Scheduled:
                case JobState.Processing:
                    Console.WriteLine("Please wait...\n");
                    break;
                case JobState.Canceled:
                    Console.WriteLine("Job is canceled.\n");
                    break;
                case JobState.Error:
                    Console.WriteLine("Job failed.\n");
                    break;
                default:
                    break;
            }
        }
    }
}
