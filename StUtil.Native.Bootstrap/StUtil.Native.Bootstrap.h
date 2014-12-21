typedef struct CLRINITARGS {
	wchar_t * dll;
	wchar_t * typeName;
	wchar_t * method;
	wchar_t * args;
} CLRINITARGS;

DWORD WINAPI CLRBootstrap(CLRINITARGS*);
