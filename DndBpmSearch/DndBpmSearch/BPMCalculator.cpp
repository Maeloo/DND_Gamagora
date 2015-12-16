#include "BPMCalculator.h"


BPMCalculator::BPMCalculator ( ) {
	size = 10;
	spectrum = new float*[size];
	for (int i = 0; i < size; ++i)
	{
		spectrum[i] = new float[Constantes::bytes];
	}
	for (int i = 0; i < size; ++i)
	{
		for (int j = 0; j < Constantes::bytes; ++j)
		{
			spectrum[i][j] = Global::randf(0.f, 10.f);
		}
	}
	const int maxChannelCount = 32;

	FMODError ( FMOD::System_Create ( &system ) );

	unsigned int version;
	FMODError ( system->getVersion ( &version ) );

	if ( version < FMOD_VERSION ) {
		printf ( "FMOD lib version %08x doesn't match header version %08x", version, FMOD_VERSION );
		exit ( -1 );
	}

	FMODError ( system->init ( maxChannelCount, FMOD_INIT_NORMAL, 0 ) );
	FMODError ( system->createChannelGroup ( NULL, &channelGroup ) );
}

BPMCalculator::~BPMCalculator ( ) {
	FMODError ( sound->release ( ) );

	sound = NULL;

	FMODError ( channelGroup->release ( ) );
	
	channelGroup = NULL;

	FMODError ( masterChannelGroup->release ( ) );

	masterChannelGroup = NULL;

	FMODError ( system->close ( ) );
	FMODError ( system->release ( ) );

	system = NULL;
}


void BPMCalculator::loadSound ( const char *path ) {
	FMODError ( system->createSound ( path, FMOD_DEFAULT, 0, &sound ) );
	//FMODError ( system->createStream ( path, FMOD_CREATESTREAM, 0, &sound ) );
}


void BPMCalculator::playSound ( ) {
	FMODError ( system->playSound ( sound, channelGroup, false, &channel ) );		
}


void BPMCalculator::calculateSpectrum ( ) {
	clock_t begin = clock ( );
	clock_t end;

	FMODError ( system->playSound ( sound, 0, false, &channel ) );
	FMODError ( system->getMasterChannelGroup ( &masterChannelGroup ) );
	FMODError ( system->createDSPByType ( FMOD_DSP_TYPE_FFT, &fftdsp ) );
	
	FMODError ( masterChannelGroup->addDSP ( 0, fftdsp ) );
	
	FMODError ( fftdsp->setBypass ( false ) );
	FMODError ( fftdsp->setActive ( true ) );

	unsigned int maxTime;
	sound->getLength ( &maxTime, FMOD_TIMEUNIT_MS );
	maxTime /= 1000;
	++maxTime;
	
	//channel->setMute ( true );
	do {
		FMODError ( system->update ( ) );

		FMOD_DSP_PARAMETER_FFT *fftparameter;
		float val;
		char s[256];
		unsigned int len;
		float *data = 0;
		float freq[32];
		int rate, chan, nyquist;
		int windowsize = 4096;

		FMODError ( system->getSoftwareFormat ( &rate, 0, 0 ) );

		FMODError ( fftdsp->setParameterInt ( FMOD_DSP_FFT_WINDOWTYPE, FMOD_DSP_FFT_WINDOW_TRIANGLE ) );
		FMODError ( fftdsp->setParameterInt ( FMOD_DSP_FFT_WINDOWSIZE, windowsize ) );
		FMODError ( fftdsp->getParameterFloat ( FMOD_DSP_FFT_DOMINANT_FREQ, &val, 0, 0 ) );
		FMODError ( fftdsp->getParameterData ( FMOD_DSP_FFT_SPECTRUMDATA, ( void ** ) &fftparameter, &len, s, 256 ) );

		data = fftparameter->spectrum[0];

		nyquist = windowsize / 2;

		for ( chan = 0; chan < 1; chan++ ) {
			float average = 0.0f;
			float power = 0.0f;

			for ( int i = 0; i < nyquist - 1; ++i ) {
				float hz = i * ( rate * 0.5f ) / ( nyquist - 1 );
				int index = i + ( 16384 * chan );

				if ( fftparameter->spectrum[chan][i] > 0.0001f ) // arbitrary cutoff to filter out noise
				{
					average += data[index] * hz;
					power += data[index];
				}
			}

			if ( power > 0.001f ) {
				freq[chan] = average / power;
			}
			else {
				freq[chan] = 0;
			}
		}
		printf ( "\ndom freq = %d : %.02f %.02f\n", ( int ) val, freq[0], freq[1] );

		Sleep ( 10.f );
		end = clock ( );
	} while ( double ( end - begin ) / CLOCKS_PER_SEC < maxTime );
}

float ** BPMCalculator::getSpectrum(int * size_)
{
	*size_ = size;
	return spectrum;
}