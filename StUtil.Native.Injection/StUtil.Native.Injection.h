// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the STUTILNATIVEINJECTION_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// STUTILNATIVEINJECTION_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef STUTILNATIVEINJECTION_EXPORTS
#define STUTILNATIVEINJECTION_API __declspec(dllexport)
#else
#define STUTILNATIVEINJECTION_API __declspec(dllimport)
#endif

typedef struct INITARGS {
	wchar_t * dll;
	wchar_t * typeName;
	wchar_t * method;
	wchar_t * args;
} INITARGS;