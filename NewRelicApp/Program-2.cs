// using System;
// using System.IO;
// using System.Runtime.CompilerServices;
// using NewRelic.Api.Agent;
// using NewRelic.LogEnrichers;
// using NewRelic.LogEnrichers.Serilog;
// using Serilog;
// using Serilog.Core;
//
// namespace NewRelicApp;
//
// public class Program
// {
//     private static Logger? _logger;
//
//     private static async Task Main(string[] args)
//     {
//         var folderPath_StandardLogs = "StandardLogs";
//         var folderPath_NewRelicLogs = "NewRelicLogs";
//
//         // Ensure that the output folders exist
//         Directory.CreateDirectory(folderPath_StandardLogs);
//         Directory.CreateDirectory(folderPath_NewRelicLogs);
//
//         // Create Logger
//         _logger = CreateLogger(
//             folderPath_StandardLogs: folderPath_StandardLogs,
//             folderPath_NewRelicLogs: folderPath_NewRelicLogs);
//
//         // This log information will be visible in New Relic Logging. Since
//         // a transaction has not been started, this log message will not be
//         // associated to a specific transaction.
//         _logger.Information("Hello, welcome to Serilog Logs In Context sample app!");
//
//
//         // Call three example methods that create transactions
//        TestMethod("First Transaction");
//       TestMethod("Second Transaction");
//       await  TestMethod("Third Transaction");
//
//
//         // This log information will be visible in New Relic Logging. Since
//         // a transaction has not been started, this log message will not be
//         // associated to a specific transaction.
//         _logger.Information("Thanks for visiting, please come back soon!");
//     }
//
//     /// <summary>
//     /// This method is responsible for configuring the application's logging.
//     /// </summary>
//     private static Logger CreateLogger(string folderPath_StandardLogs, string folderPath_NewRelicLogs)
//     {
//         Console.WriteLine($"Standard Logs Folder            : {folderPath_StandardLogs}");
//         Console.WriteLine($"New Relic Log Forwarder Source  : {folderPath_NewRelicLogs}");
//         Console.WriteLine();
//
//         var loggerConfig = new LoggerConfiguration();
//
//         // CONFIGURE BASIC LOGGING TO A FILE
//         // 1.  Enrich log Events with Thread information using the Serilog enricher.
//         // 2.  Write log files to local storage
//         // loggerConfig
//         //     .Enrich.WithThreadName()
//         //     .Enrich.WithThreadId()
//         //     .WriteTo.File(Path.Combine(folderPath_StandardLogs, "SerilogExtensions.log"));
//
//         // CONFIGURE NEW RELIC LOGGING.
//         // 1.  Add contextual information to log events with the New Relic Enricher
//         // 2.  Create output in New Relic's expected JSON format
//         // 3.  Map the "ThreadId" and "ThreadName" properties in the JSON format
//         // 4.  Write the JSON files to a staging location for the Log Forwarder
//         loggerConfig
//             .Enrich.WithNewRelicLogsInContext()
//             .WriteTo.File(
//                 path: Path.Combine(folderPath_NewRelicLogs, "SerilogExtensions_NewRelicLogging.json"),
//                 formatter: new NewRelicFormatter()
//                     .WithPropertyMapping("ThreadId", NewRelicLoggingProperty.ThreadId)
//                     .WithPropertyMapping("ThreadName", NewRelicLoggingProperty.ThreadName));
//
//         return loggerConfig.CreateLogger();
//     }
//
//     /// <summary>
//     /// This method will be recorded as a Transaction using the .Net Agent.
//     /// With New Relic Logging Configured, the log messages will be associated
//     /// to the transaction.
//     /// </summary>
//     //[Transaction]
//     [MethodImpl(MethodImplOptions.NoInlining)]
//     private static async Task TestMethod(string testVal)
//     {
//         _logger?.Information("Starting TestMethod - {testValue}", testVal);
//
//         try
//         {
//             for (var cnt = 0; cnt < 10000; cnt++)
//             {
//                 await Task.Delay(2000);
//                 Console.WriteLine("writing message");
//                 _logger?.Information("This is log message #{MessageID}", cnt);
//             }
//         }
//         catch (Exception ex)
//         {
//             _logger?.Error(ex, "Error has occurred in TestMethod - {testValue}", testVal);
//         }
//
//         _logger?.Information("Ending TestMethod - {testValue}", testVal);
//     }
// }