#include <QtWidgets/QApplication>
#include <iostream>
#include <fstream>
#include <cmath>
#include <vector>
#include<iostream>
#include<string>
#include <windows.h>
#include "telekomzad1.h"
#include "QtWidgetsApplication1.h"


using namespace std;


int main(int argc, char* argv[])
{
    
    QApplication b(argc, argv);
    QtWidgetsApplication1 x;
    telekomzad1 w;
    x.show();
    w.show();  
    return b.exec();

}
