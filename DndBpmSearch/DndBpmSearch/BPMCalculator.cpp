#include "BPMCalculator.h"


BPMCalculator::BPMCalculator ( ) {
	// Constantes
	const int maxChannelCount = 512;

	unsigned int version;
	FMODError ( FMOD::System_Create ( &system ) );

	FMODError ( system->getVersion ( &version ) );

	if ( version < FMOD_VERSION ) {
		printf ( "FMOD lib version %08x doesn't match header version %08x", version, FMOD_VERSION );
		exit ( -1 );
	}
	FMODError ( system->init ( maxChannelCount, FMOD_INIT_NORMAL, 0 ) );
}

BPMCalculator::~BPMCalculator ( ) {
	FMODError ( sound->release ( ) );

	sound = NULL;

	FMODError ( system->close ( ) );
	FMODError ( system->release ( ) );

	system = NULL;
}


void BPMCalculator::loadSound ( const char *path ) {
	FMODError ( system->createSound ( path, FMOD_DEFAULT, 0, &sound ) );
}


void BPMCalculator::playSound ( ) {
	clock_t begin = clock ( );
	clock_t end;

	FMOD::Channel *channel = 0;
	FMOD::ChannelGroup *channelMusic;

	FMODError ( system->createChannelGroup ( NULL, &channelMusic ) );
	FMODError ( system->playSound ( sound, channelMusic, false, &channel ) );

	unsigned int maxTime;
	sound->getLength ( &maxTime, FMOD_TIMEUNIT_MS );
	maxTime /= 1000;
	maxTime += 1;

	do {
		FMODError ( system->update ( ) );
		Sleep ( 10.f );
		end = clock ( );
	} while ( double ( end - begin ) / CLOCKS_PER_SEC < maxTime );
}
