// PMBCPPLibrary.cpp : Defines the exported functions for the DLL application.
//
#include "PMBCPPLibrary.h"


extern "C" {
	float DllFunc ( ) {
		return 987654321.0f;
	}
}
