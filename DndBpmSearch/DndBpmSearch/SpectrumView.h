#pragma once

#include "Constantes.h"
#include <glut.h>
/*
* Debug class, singleton,
* Spectum view
*/

class SpectrumView
{
	/*
	* Glut functions
	*/
	static void init();
	static void reshape(int w, int h);
	static void display();
	/*
	* Draw spectrum tab
	*/
	static void drawSpectrum();
	/*
	* Spectrum tab 
	* size : spectrumSize, Constante::bytes
	*/
	static float ** spectrum;
	static int spectrumSize;
public:
	/*
	* Init spectrum
	*/
	SpectrumView(int argc, char ** argv, float * spectrum_[Constantes::bytes], int spectrumSize);
	
};

