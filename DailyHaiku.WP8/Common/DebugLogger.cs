﻿using System;
using System.Diagnostics;
using Caliburn.Micro;

namespace DailyHaiku.WP8.Common
{
    public class DebugLogger : ILog
    {
        private readonly Type type;

        public DebugLogger(Type type)
        {
            this.type = type;
        }

        private string CreateLogMessage(string format, params object[] args)
        {
            return string.Format("[{0}] {1}",
                                 DateTime.Now.ToString("o"),
                                 string.Format(format, args));
        }

        public void Error(Exception exception)
        {
            Debug.WriteLine(CreateLogMessage(exception.ToString()), "ERROR");
        }

        public void Info(string format, params object[] args)
        {
            Debug.WriteLine(CreateLogMessage(format, args), "INFO");
        }

        public void Warn(string format, params object[] args)
        {
            Debug.WriteLine(CreateLogMessage(format, args), "WARN");
        }

    }
}
