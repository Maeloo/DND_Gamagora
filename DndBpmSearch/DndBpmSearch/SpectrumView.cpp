#include "SpectrumView.h"



SpectrumView::SpectrumView(int argc, char ** argv)
{
	glutInitWindowSize(400, 400);
	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DOUBLE);
	glutCreateWindow("Spectrum");
	init();
	glutReshapeFunc(reshape);
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
	glOrtho(-20, 20, -20, 20, -10, 10);
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

void SpectrumView::drawSpectrum()
{

}
