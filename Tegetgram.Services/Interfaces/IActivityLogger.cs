using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Tegetgram.Services.Interfaces
{
    public interface IActivityLogger
    {
        void Log(ILogger logger, string userName, string actionName, string message);
    }
}
