// StUtil.Native.Injection.cpp : Defines the exported functions for the DLL application.
//
#include "stdafx.h"
#include "StUtil.Native.Injection.h"
#include <string>
#include "Shlwapi.h"
#include "TlHelp32.h"
#include <metahost.h>
#include <mscoree.h>
#include <exception>
using namespace std;

#pragma comment(lib, "mscoree.lib")
#pragma comment(lib, "Shlwapi.lib")

#import "mscorlib.tlb" raw_interfaces_only				\
    high_property_prefixes("_get","_put","_putref")		\
    rename("ReportEvent", "InteropServices_ReportEvent")

inline size_t GetStringAllocSize(const LPCWSTR str)
{
	// size (in bytes) of string
	return (wcsnlen(str, 65536) * sizeof(wchar_t)) + sizeof(wchar_t); // +sizeof(wchar_t) for null
}

DWORD_PTR GetRemoteModuleHandle(DWORD dwProcessId, const LPCWSTR moduleName)
{
	wchar_t* module = PathFindFileName(moduleName);

	MODULEENTRY32 me32;
	HANDLE hSnapshot = INVALID_HANDLE_VALUE;

	me32.dwSize = sizeof(MODULEENTRY32);
	hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE, dwProcessId);

	if (!Module32First(hSnapshot, &me32))
	{
		CloseHandle(hSnapshot);
		return 0;
	}

	while (wcscmp(me32.szModule, module) != 0 && Module32Next(hSnapshot, &me32));

	CloseHandle(hSnapshot);

	if (wcscmp(me32.szModule, module) == 0)
	{
		return (DWORD_PTR)me32.modBaseAddr;
	}
	return 0;
}

DWORD_PTR GetProcOffset(LPCWSTR lpLibraryFilePath, LPCSTR lpProcName)
{
	HMODULE hModule;
	BOOL bFreeLibrary = false;
	hModule = GetModuleHandle(lpLibraryFilePath);
	if (hModule == 0)
	{
		hModule = LoadLibrary(lpLibraryFilePath);
		if (hModule == 0)
		{
			DWORD error = GetLastError();
			wchar_t message[256];
			swprintf_s(message, L"LoadLibrary failed with error code: %d", error);
			throw message;
		}
		bFreeLibrary = true;
	}

	FARPROC lpProcAddress = GetProcAddress(hModule, lpProcName);
	DWORD_PTR dwOffset = (DWORD_PTR)lpProcAddress - (DWORD_PTR)hModule;

	if (bFreeLibrary)
	{
		FreeLibrary(hModule);
	}

	return dwOffset;
}

DWORD WINAPI RemoteInvokeProc(DWORD dwProcessId, LPCWSTR lpLibraryFilePath, LPCSTR lpProcName, LPVOID lpParameter)
{
	DWORD_PTR dwOffset = GetProcOffset(lpLibraryFilePath, lpProcName);
	DWORD_PTR lpRemoteModule = GetRemoteModuleHandle(dwProcessId, lpLibraryFilePath);
	return 0;
	//TODO:
	//return RemoteInvoke(dwProcessId, (LPVOID)lpRemoteModule, lpParameter);
}

DWORD WINAPI RemoteInvoke(DWORD dwProcessId, LPVOID lpStartAddress, LPVOID lpParameter)
{
	HANDLE hProcess = OpenProcess(PROCESS_ALL_ACCESS, FALSE, dwProcessId);
	HANDLE hThread = CreateRemoteThread(hProcess, NULL, 0, (LPTHREAD_START_ROUTINE)lpStartAddress, lpParameter, NULL, 0);
	if (hThread == 0)
	{
		DWORD error = GetLastError();
		wchar_t message[256];
		swprintf_s(message, L"CreateRemoteThread failed with error code: %d", error);
		CloseHandle(hProcess);
		throw message;
	}

	WaitForSingleObject(hThread, INFINITE);

	DWORD result = 0;
	GetExitCodeThread(hThread, &result);
	CloseHandle(hProcess);
	return result;
}

DWORD_PTR WINAPI RemoteLoadLibrary(DWORD dwProcessId, LPCWSTR lpLibraryFilePath)
{
	SIZE_T szPath = GetStringAllocSize(lpLibraryFilePath);
	HANDLE hProcess = OpenProcess(PROCESS_ALL_ACCESS, FALSE, dwProcessId);

	LPVOID lpBaseAddress = VirtualAllocEx(hProcess, NULL, szPath, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
	if (lpBaseAddress == 0)
	{
		DWORD error = GetLastError();
		wchar_t message[256];
		swprintf_s(message, L"VirtualAllocEx failed with error code: %d", error);
		CloseHandle(hProcess);
		throw message;
	}
	DWORD_PTR dwInject;
	try
	{
		dwInject = RemoteInvokeProc(dwProcessId, L"kernel32.dll", "LoadLibrary", lpBaseAddress);
	}
	catch (exception &e)
	{
		VirtualFreeEx(hProcess, lpBaseAddress, 0, MEM_RELEASE);
		CloseHandle(hProcess);
		throw e;
	}

	VirtualFreeEx(hProcess, lpBaseAddress, 0, MEM_RELEASE);
	CloseHandle(hProcess);

	return dwInject;
}