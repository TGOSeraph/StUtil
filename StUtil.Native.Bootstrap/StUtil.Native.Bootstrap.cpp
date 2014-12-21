// StUtil.Native.Injection.cpp : Defines the exported functions for the DLL application.
//
#include "stdafx.h"
#include "StUtil.Native.Bootstrap.h"
#include <metahost.h>
#include <mscoree.h>

#pragma comment(lib, "mscoree.lib")
#pragma comment(lib, "Shlwapi.lib")

#import "mscorlib.tlb" raw_interfaces_only				\
    high_property_prefixes("_get","_put","_putref")		\
    rename("ReportEvent", "InteropServices_ReportEvent")

ICLRRuntimeHost* GetRuntimeHost(LPCWSTR dotNetVersion)
{
	ICLRMetaHost* metaHost = NULL;
	ICLRRuntimeInfo* info = NULL;
	ICLRRuntimeHost* runtimeHost = NULL;

	// Get the CLRMetaHost that tells us about .NET on this machine
	if (S_OK == CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost, (LPVOID*)&metaHost))
	{
		// Get the runtime information for the particular version of .NET
		if (S_OK == metaHost->GetRuntime(dotNetVersion, IID_ICLRRuntimeInfo, (LPVOID*)&info))
		{
			// Get the actual host
			if (S_OK == info->GetInterface(CLSID_CLRRuntimeHost, IID_ICLRRuntimeHost, (LPVOID*)&runtimeHost))
			{
				// Start it. This is okay to call even if the CLR is already running
				runtimeHost->Start();
			}
		}
	}
	if (NULL != info)
		info->Release();
	if (NULL != metaHost)
		metaHost->Release();

	return runtimeHost;
}

int ExecuteClrCode(ICLRRuntimeHost* host, LPCWSTR assemblyPath, LPCWSTR typeName,
	LPCWSTR function, LPCWSTR param)
{
	if (NULL == host)
		return -1;

	DWORD result = -1;
	HRESULT hresult;

	if (S_OK != (hresult = host->ExecuteInDefaultAppDomain(assemblyPath, typeName, function, param, &result)))
	{
		return hresult;
	}

	return result;
}

DWORD WINAPI CLRBootstrap(CLRINITARGS* args)
{
	ICLRRuntimeHost *pClrRuntimeHost = GetRuntimeHost(L"v4.0.30319");

	int res = ExecuteClrCode(pClrRuntimeHost, args->dll, args->typeName, args->method, args->args);
	pClrRuntimeHost->Release();
	return res;
}