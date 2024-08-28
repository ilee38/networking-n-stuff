namespace LoadBalancer;

class Program
{
  static async Task<int> Main(string[] args)
  {
      var lbExitCode = await LoadBalancer.Start();
      return lbExitCode;
  }
}
