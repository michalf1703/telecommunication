#pragma once

#include <QtWidgets/QWidget>
#include "ui_telekomzad1.h"
#include <iostream>
#include <fstream>
#include <cmath>
#include <vector>
#include<iostream>
#include<string>
#include <windows.h>

using namespace std;

class telekomzad1 : public QWidget
{
    Q_OBJECT

public:
    telekomzad1(QWidget* parent = nullptr);
    ~telekomzad1();
    vector<bool> asciiToBinary(const vector<char>& data);
    vector<bool> zamienSlowoNaPostacBitowa(std::string slowo);
    vector<bool> ASCIItoBinary(int znak);
    vector<bool> dodajBityParzystosci(std::vector<bool>wiadomosc);
    vector<bool> zamienStringBitowyNaBity(std::string stringbitowy);
    string zamienNaZakodowanyString(std::vector<bool> bity);
    int BinaryToAscii(std::vector<bool> bity);
    string poprawSlowo(std::string doPoprawienia);
    string konwertujZakodowaneSlowoNaPostacBitowa(std::string test);
    vector<bool> iloczynHX(std::vector<bool> tab);
    vector<bool> findWrongColumns(std::vector<bool> E);
    vector<bool> fix(std::vector<bool> slowo, std::vector<bool> wrongColumns);

private slots:
    void callFunctions();

private:
    Ui::telekomzad1Class ui;
};
