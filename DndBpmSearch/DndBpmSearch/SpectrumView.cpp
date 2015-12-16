#include "SpectrumView.h"

float ** SpectrumView::spectrum;
int SpectrumView::spectrumSize;

/*
* argc, argv : entry parameter
* spectrum : array 
* spectrum size : size of spectrum firt dimension
*/
SpectrumView::SpectrumView(int argc, char ** argv, float ** spectrum_, int spectrumSize_)
{
	spectrum = spectrum_;
	spectrumSize = spectrumSize_;
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
	// Si vous avez des choses à initialiser, c est ici.	
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
	float step = 0.01f;
	glColor3f(0.9f, 0.9f, 0.9f);
	
	for (int i = 0; i < spectrumSize; ++i)
	{
		glBegin(GL_LINE_STRIP);
		for (int j = 0; j < 44200; ++j)
		{
			glVertex3f(step * i, spectrum[i][j] * 10.f, 0.f);
		}
		glEnd();
	}
}
