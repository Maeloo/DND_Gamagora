#include "BPMCalculator.h"
#include "SpectrumView.h"
int main(int argc, char ** argv)
{
	BPMCalculator bpmc = BPMCalculator ( );
	bpmc.loadSound ( "didjedelik.mp3" );
	//bpmc.playSound ( );
	//bpmc.calculateSpectrum ( );
	int size = 0;
	float ** tmp = bpmc.getSpectrum(&size);
	SpectrumView windows(argc, argv, tmp, size);
	std::cin.get ( );
	return 0;
}