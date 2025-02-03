using Utilities.Interfaces;

namespace Exam.Services
{
    public class RootResolver : IAppRootResolver
    {
        IWebHostEnvironment _environment;
        public RootResolver(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public string GetRootPath()
        {
            return _environment.WebRootPath;
        }
    }
}
