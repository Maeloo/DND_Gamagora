#pragma once
#include <glut.h>

class SpectrumView
{
	static void init();
	static void reshape(int w, int h);
	void display();
	void drawSpectrum();
public:
	SpectrumView(int argc, char ** argv);
	
};

