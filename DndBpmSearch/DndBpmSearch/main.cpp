#include "BPMCalculator.h"

int main(int argc, char ** argv)
{
	BPMCalculator bpmc = BPMCalculator ( );
	bpmc.loadSound ( "didjedelik.mp3" );
	//bpmc.playSound ( );
	bpmc.calculateSpectrum ( 100 );

	for (int i = 0; i < 100; ++i) {
		printf("\n %d \n", bpmc._spectrum_sizes[i]);
		for (int j = 0; j < bpmc._spectrum_sizes[i]; ++j) {
			printf("%d", bpmc._spectrum_data[j]);
		}		
	}
	
	std::cin.get ( );

	return 0;
}