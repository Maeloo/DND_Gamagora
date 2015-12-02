// PMBCPPLibrary.h

#ifdef PMBDLL_EXPORT
#define PMBDLL_API __declspec(dllexport) 
#else
#define PMBDLL_API __declspec(dllimport) 
#endif

extern "C" {
	PMBDLL_API float DllFunc ( );
}
