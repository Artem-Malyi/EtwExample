using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.Threading;

namespace EtwConsumer
{
    class Program
    {
        public static string ExampleProvidernName = "ExampleTraceLoggingProvider";
        public static string ExampleProviderGuid = "253025BF-E388-4768-96AF-5CD5A6B3D9E2";
        //                                         "2A576B87-09A7-520E-C21A-4942F0271D67"

        static void Main(string[] args)
        {
            try
            {
                StartEventsConsumer();

                while (true)
                {
                    Thread.Sleep(2000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Start rsAmsiLoggingConsumer error: " + e.Message);
            }
        }

        static void StartEventsConsumer()
        {
            var ExampleSession = new TraceEventSession(ExampleProvidernName, null)
            {
                BufferSizeMB = 128,
                CpuSampleIntervalMSec = 10,
                StopOnDispose = true
            };

            var ExampleTraceSource = new ETWTraceEventSource(ExampleSession.SessionName, TraceEventSourceType.Session);

            var ExampleParser = new RegisteredTraceEventParser(ExampleTraceSource);
            ExampleParser.All += ExampleEventsParser;

            ExampleSession.EnableProvider(new Guid(ExampleProviderGuid), TraceEventLevel.Always);

            ExampleTraceSource.Process();
        }

        static void ExampleEventsParser(TraceEvent obj)
        {
            try
            {
                if (obj == null)
                    return;

                if (!obj.ProviderName.Equals(ExampleProvidernName) || obj.EventName == null)
                    return;

                if (obj.EventName.Equals("ScriptBlockAttributes"))
                {
                    string scriptBlockInfo =
                        "{ ScanId: " + obj.PayloadByName("ScanId").ToString() +
                        ", AppName: " + obj.PayloadByName("AppName").ToString() +
                        ", ContentName: " + obj.PayloadByName("ContentName").ToString() +
                        ", ContentSize: " + obj.PayloadByName("ContentSize").ToString() +
                        ", Session: " + obj.PayloadByName("Session").ToString() +
                        ", ContentAddress: " + obj.PayloadByName("ContentAddress").ToString() +
                        ", Content: \"" + obj.PayloadByName("Content").ToString() + "\"" +
                        " }"
                    ;
                    Console.WriteLine(scriptBlockInfo);
                }

                Console.WriteLine("Event name: " + obj.EventName ?? "(empty)");
            }
            catch (Exception e)
            {
                Console.WriteLine("ExampleEventsParser error: " + e.Message);
            }
        }
    }
}
