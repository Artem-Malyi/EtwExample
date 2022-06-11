// EtwProvider.cpp : This file contains the 'main' function. Program execution begins and ends there.
//
#include <Windows.h>
#include <TraceLoggingProvider.h>

#include <iostream>
#include <string>

// Define a trace logging provider: 253025BF-E388-4768-96AF-5CD5A6B3D9E2
TRACELOGGING_DEFINE_PROVIDER(g_traceLoggingProvider, "ExampleTraceLoggingProvider", (0x253025BF, 0xE388, 0x4768, 0x96, 0xAF, 0x5C, 0xD5, 0xA6, 0xB3, 0xD9, 0xE2));

using namespace std;

int main()
{
    TraceLoggingRegister(g_traceLoggingProvider);
    TraceLoggingWrite(g_traceLoggingProvider, "Started");

    ULONG scanRequestNumber = 0;
    wstring appName = L"Powershell.exe";
    wstring contentName = L"C:\\ws\\print-env.ps1";
    ULONGLONG contentSize = 0x123456780abcdef;
    PVOID session = reinterpret_cast<PVOID>(0x12345);
    wstring content = L"echo hello!";

    while (true) {
		TraceLoggingWrite(
			g_traceLoggingProvider, "ScriptBlockAttributes",
			TraceLoggingValue(scanRequestNumber, "ScanId"),
			TraceLoggingWideString(appName.c_str(), "AppName"),
			TraceLoggingWideString(contentName.c_str(), "ContentName"),
			TraceLoggingUInt64(contentSize, "ContentSize"),
			TraceLoggingPointer(session, "Session"),
			TraceLoggingPointer(content.c_str(), "ContentAddress"),
			TraceLoggingWideString(content.c_str(), "Content")
		);

        ++scanRequestNumber;

        cout << "Produced new TraceLogging event" << endl;

        Sleep(2000);
    }

    TraceLoggingWrite(g_traceLoggingProvider, "Stopped");
    TraceLoggingUnregister(g_traceLoggingProvider);

    return 0;
}
