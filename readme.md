# ETW Example

Example of custom EtwProvider in native code and its EtwConsumer in managed code

Inspired by:
 * https://stackoverflow.com/questions/63432283/in-dotnet-how-do-you-listen-to-events-from-specific-etw-providers-given-you-hav
 * https://kallanreed.com/2016/05/28/creating-an-etw-provider-step-by-step/
 * https://www.nuget.org/packages/Microsoft.Diagnostics.Tracing.TraceEvent

# Collect logs
1. Run EtwProvider, it start producing TraceLogging events.
2. Start logging session with `xperf -start mySession -f myFile.etl -on 253025BF-E388-4768-96AF-5CD5A6B3D9E2`
3. After some time stop logging session with `xperf -stop mySession`
4. Convert the binary etl file to xml format `tracerpt.exe .\myFile.etl /y`
5. Observe the results in generated `dumpfile.xml`

# Notes
 * xperf.exe is a part of Windows ADK
 * managed ETW consumer relies on the Microsoft.Diagnostics.Tracing.TraceEvent library
 * use `Install-Package Microsoft.Diagnostics.Tracing.TraceEvent -Version 3.0.1` in Tools -> NuGet Package Manager -> Console