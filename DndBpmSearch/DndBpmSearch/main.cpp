#include "fmod.hpp"
#include "fmod.h"
#include "fmod_errors.h"
#include "fmod_common.h"
#include "fmod_codec.h"
#include "fmod_output.h"
#include <iostream>
#include <ctime>
#include<windows.h>

void FMODError(FMOD_RESULT result)
{
	if (result != FMOD_OK)
	{
		printf("FMOD error! (%d) %s\n", result, FMOD_ErrorString(result));
		exit(-1);
	}
}

int main(int argc, char ** argv)
{
	// Helloworld 
	FMOD::DSP  *dsp;
	FMOD::Channel   *channel = 0;
	clock_t begin = clock();
	clock_t end;
	// Constantes
	const int maxChannelCount = 512;
	const float maxTime = 1.f;
	// Initialisation
	FMOD::System *system = NULL;

	unsigned int     version;
	FMODError(FMOD::System_Create(&system));

	FMODError(system->getVersion(&version));

	if (version < FMOD_VERSION)
	{
		printf("FMOD lib version %08x doesn't match header version %08x", version, FMOD_VERSION);
		exit(-1);
	}
	FMODError(system->init(maxChannelCount, FMOD_INIT_NORMAL, 0));

	/*
	Create an oscillator DSP units for the tone.
	*/
	FMODError(system->createDSPByType(FMOD_DSP_TYPE_OSCILLATOR, &dsp)); 
	FMODError(dsp->setParameterFloat(FMOD_DSP_OSCILLATOR_RATE, 440.0f)); /* Musical note 'A' */

	
	/*
	Main loop
	*/
	do
	{ 
		FMODError(system->playDSP(dsp, 0, true, &channel));
		if (channel)
		{
				FMODError(channel->stop());
		}
		FMODError(system->playDSP(dsp, 0, true, &channel));
		FMODError(channel->setPaused(false));
		
	
		FMODError(system->update());
		Sleep(10.f);
		 end = clock();
	} while (double(end - begin) / CLOCKS_PER_SEC < maxTime);

	FMODError(dsp->release());
	FMODError(system->close());
	FMODError(system->release());

	return 0;
}