using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;
using Spoti;
using Spoti.Interface;

namespace Spotify.Cmd
{
    public class Log : ILog
    {
        protected Logger Logger;

        public Log()
        {

            var config = new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget { Layout = "${date:format=HH\\:MM\\:ss} ${logger} ${message}" };
            var chainsawTarget = new ChainsawTarget { Address = "udp://127.0.0.1:7071", Name = "chainsaw", Layout = "Log4JXmlEventLayout" };

            config.AddTarget("console", consoleTarget);
            config.AddTarget("chainsaw", chainsawTarget);

            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, chainsawTarget));

            LogManager.Configuration = config;


            Logger = LogManager.GetCurrentClassLogger();


        }

        public void Error(string moduleId, Exception ex)
        {
            Logger.Error("{0}> {1}", moduleId, ex.Message);
        }

        public void Debug(string moduleId, string message)
        {
            Logger.Debug("{0}> {1} ", moduleId, message);
        }

        public void Warning(string moduleId, string message)
        {
            Logger.Warn("{0}> {1} ", moduleId, message);
        }

        public void Error(string moduleId, string message)
        {
            Logger.Error("{0}> {1} ", moduleId, message);
        }

    }
}
