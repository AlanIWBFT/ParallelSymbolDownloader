#include <iostream>
#include <string>
#include <filesystem>
#include <algorithm>
#include <cwctype>
#include <Windows.h>
#include <DbgHelp.h>

#pragma comment(lib, "dbghelp.lib")

BOOL DebugInfoCallback(HANDLE hProcess, ULONG ActionCode, ULONG64 CallbackData, ULONG64 UserContext)
{
	switch (ActionCode)
	{
	case CBA_DEBUG_INFO:
		std::wcout << ((const wchar_t*)CallbackData);
		return TRUE;
	default:
		return FALSE;
	}
}

struct UserContext
{
	HANDLE hProcess;
	int workerId;
	int numTotalWorkers;
	int iterationIndex;
	bool dryRun;
};

BOOL CALLBACK EnumModules(
	PCWSTR   ModuleName,
	DWORD64 BaseOfDll,
	PVOID   inUserContext)
{
	UserContext& userContext = *(UserContext*)inUserContext;
	if ((userContext.iterationIndex - userContext.workerId) % userContext.numTotalWorkers != 0)
	{
		userContext.iterationIndex++;
		return true;
	}
	else
	{
		userContext.iterationIndex++;
	}

	IMAGEHLP_MODULEW64 ModuleInfo;
	ModuleInfo.SizeOfStruct = sizeof(ModuleInfo);
	SymGetModuleInfoW(userContext.hProcess, BaseOfDll, &ModuleInfo);
	// Filter out Dlls with PDBs next to them (common case for Unreal, especially for plugins)
	std::wstring absolutePath(ModuleInfo.ImageName);
	absolutePath = absolutePath.substr(0, absolutePath.find_last_of(L".")) + L".pdb";
	if (!std::filesystem::exists(absolutePath))
	{
		std::wstring ImageName(ModuleInfo.ImageName);
		std::transform(ImageName.begin(), ImageName.end(), ImageName.begin(), [](wchar_t c) { return std::towlower(c); });
		std::wcout << L"> " << ImageName << std::endl;
		if (!userContext.dryRun)
		{
			SYMBOL_INFOW SymbolInfo;
			SymbolInfo.SizeOfStruct = sizeof(SYMBOL_INFOW);
			SymbolInfo.MaxNameLen = 0;
			SymFromIndexW(userContext.hProcess, BaseOfDll, 0, &SymbolInfo);
		}
	}
	SymUnloadModule(userContext.hProcess, BaseOfDll); // Immediately unload to keep memory low
	return TRUE;
}

#define check(x) { if (x == 0) return -1; }

int wmain(int argc, const wchar_t* argv[])
{
	int dryRun, pID, workerId, numTotalWorkers;
	swscanf_s(argv[1], L"%d", &dryRun);
	swscanf_s(argv[2], L"%d", &pID);
	swscanf_s(argv[3], L"%d", &workerId);
	swscanf_s(argv[4], L"%d", &numTotalWorkers);
	std::wstring cacheAndServerPaths(dryRun == 1 ? L"" : argv[5]);

	check(SymSetOptions(SYMOPT_DEBUG | SYMOPT_DEFERRED_LOADS | SYMOPT_INCLUDE_32BIT_MODULES));
    HANDLE hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, pID);

	UserContext userContext{ hProcess, workerId, numTotalWorkers, 0, dryRun == 1 };
	check(SymInitializeW(hProcess, cacheAndServerPaths.c_str(), false));
	// Uncomment to enable verbose debug log
	// check(SymRegisterCallbackW64(hProcess, DebugInfoCallback, 0));
	check(SymRefreshModuleList(hProcess));
	check(SymEnumerateModulesW64(hProcess, EnumModules, &userContext));
}
