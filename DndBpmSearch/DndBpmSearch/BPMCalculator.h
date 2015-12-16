#pragma once

#include "fmod.hpp"
#include "fmod.h"
#include "fmod_errors.h"
#include "fmod_common.h"
#include "fmod_codec.h"
#include "fmod_output.h"
#include "Constantes.h"
#include "Global.h"
#include <iostream>
#include <ctime>
#include <windows.h>
#include <vector>
#include <array>
#include <thread>


class BPMCalculator 
{
private:
	void Run();
	bool thread_starded;
	bool thread_exit;
	std::thread *thread;


	static FMOD::System		*system;
	FMOD::Sound			*sound = NULL;
	FMOD::Channel		*channel = 0;
	FMOD::ChannelGroup	*channelGroup = NULL;
	FMOD::ChannelGroup	*masterChannelGroup = NULL;
	static FMOD::DSP			*fftdsp;

	static clock_t end, begin;
public:
	static FMOD_DSP_PARAMETER_FFT* _spectrum_data;

	BPMCalculator (  );

	void loadSound ( const char *path );
	void playSound ( );

	void calculateSpectrum();

	void StartThread();

	static void FMODError ( FMOD_RESULT result ) {
		if ( result != FMOD_OK ) {
			printf ( "FMOD error! (%d) %s\n", result, FMOD_ErrorString ( result ) );
			std::getchar ( );
			//exit ( -1 );
		}
	}

	~BPMCalculator ( );
};

