#include "BPMCalculator.h"
#include "SpectrumView.h"
int main(int argc, char ** argv)
{
	BPMCalculator bpmc = BPMCalculator ( );
	bpmc.loadSound ( "Inception_Time_PianoSolo.mp3" );

	bpmc.StartThread();

	SpectrumView windows(argc, argv, bpmc._spectrum_data);

	std::cin.get ( );
	return 0;
}