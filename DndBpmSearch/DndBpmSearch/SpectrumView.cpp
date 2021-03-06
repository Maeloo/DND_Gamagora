#include "SpectrumView.h"

FMOD_DSP_PARAMETER_FFT *SpectrumView::spectrum;

int SpectrumView::spectrumSize;

/*
* argc, argv : entry parameter
* spectrum : array 
* spectrum size : size of spectrum firt dimension
*/
SpectrumView::SpectrumView(int argc, char ** argv, FMOD_DSP_PARAMETER_FFT * spectrum_)
{
	spectrum = spectrum_;
	//spectrumSize = spectrumSize_;
	glutInitWindowSize(400, 400);
	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DOUBLE);
	glutCreateWindow("Spectrum");
	init();
	glutReshapeFunc(reshape);
	glutDisplayFunc(display);
	glutMainLoop();
}


void SpectrumView::init()
{
	glClearColor(0.0, 0.0, 0.0, 0.0);
	// Si vous avez des choses � initialiser, c est ici.	
	glEnable(GL_DEPTH_TEST);
	glEnable(GL_CULL_FACE);
}


/* Au cas ou la fenetre est modifiee ou deplacee */
void SpectrumView::reshape(int w, int h)
{
	glViewport(0, 0, (GLsizei)w, (GLsizei)h);
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	glOrtho(-0.5, 1.5, -1, 8, -1, 1);
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
}

void SpectrumView::display(void)
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT); //effacement du buffer
	drawSpectrum();
	glLoadIdentity();
	glFlush();
	glutSwapBuffers();// echange des buffers
	glutPostRedisplay();
}

/*
* Draw spectrum tab
*/
void SpectrumView::drawSpectrum()
{
	float step_x = 0.02f;
	float step_y = 50.f;
	float output_rate = 24000.f;
	glColor3f(0.9f, 0.9f, 0.9f);

	spectrum = BPMCalculator::_spectrum_data;
	
	for (int channel = 0; channel < spectrum->numchannels; ++channel)
	{
		glBegin(GL_LINE_STRIP);
		for (int bin = 0; bin < spectrum->length * 0.1f; ++bin)
		{
			float hz = bin * output_rate / (spectrum->length * 0.5f - 1.f);
			glVertex3f(step_x * channel + bin * step_x, step_y * spectrum->spectrum[channel][bin], 0.f);
		}
		glEnd();
	}
}
