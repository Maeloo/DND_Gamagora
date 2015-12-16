#pragma once

#include "fmod.hpp"
#include "fmod.h"
#include "fmod_errors.h"
#include "fmod_common.h"
#include "fmod_codec.h"
#include "fmod_output.h"

#include "Constantes.h"
#include<iostream>
#include <GL/glut.h>
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
	static FMOD_DSP_PARAMETER_FFT * spectrum;
	static int spectrumSize;
public:
	/*
	* Init spectrum
	*/
	SpectrumView(int argc, char ** argv, FMOD_DSP_PARAMETER_FFT * spectrum_);
	
};

