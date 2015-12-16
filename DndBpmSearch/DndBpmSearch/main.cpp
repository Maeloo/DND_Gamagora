#include "BPMCalculator.h"

int main(int argc, char ** argv)
{
	BPMCalculator bpmc = BPMCalculator ( );
	bpmc.loadSound ( "didjedelik.mp3" );
	//bpmc.playSound ( );
	bpmc.calculateSpectrum ( );
	
	std::cin.get ( );

	return 0;
}