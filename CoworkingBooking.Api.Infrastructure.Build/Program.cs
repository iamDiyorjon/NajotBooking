using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV1s;

var githubPipeline = new GithubPipeline
{
    Name = "NajotaBooking Build Process",

    OnEvents = new Events
    {
        Push = new PushEvent
        {
            Branches = new string[] { "main" }
        },

        PullRequest = new PullRequestEvent
        {
            Branches = new string[] { "main" }
        }
    },

    Jobs = new Jobs
    {
        Build = new BuildJob
        {
            RunsOn = BuildMachines.Windows2022,

            Steps = new List<GithubTask>
            {
                new CheckoutTaskV2
                  {
                      Name = "Check out"
                  },

                  new SetupDotNetTaskV1
                  {
                      Name = "Setup .Net",

                      TargetDotNetVersion = new TargetDotNetVersion
                      {
                          DotNetVersion = "7.0.100",
                          IncludePrerelease = true
                      }
                  },

                  new RestoreTask
                  {
                      Name = "Restore Packages"
                  },

                  new DotNetBuildTask
                  {
                      Name = "Build Projects"
                  },

                  new TestTask
                  {
                      Name = "Run Tests"
                  }
            }
        }
    }
};

var adotnetClient = new ADotNetClient();

adotnetClient.SerializeAndWriteToFile(
    adoPipeline: githubPipeline,
    path: "../../../../.github/workflows/build.yml");
