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
    telekomzad1(QWidget *parent = nullptr);
    ~telekomzad1();
    vector<bool> string_to_bin(string str);

    vector<bool> char_to_bin(string str);
    char bin_to_char(vector<bool> bin);
    int parity(vector<bool> msg, int n);
    void addParity(vector<bool>& msg);
    vector<bool> encode(vector<bool> msg);
    void adjust(vector<bool>& msg, vector<bool> err);
    void verify(vector<bool>& msg, int len);

private slots:
    void callFunctions();
         
private:
    Ui::telekomzad1Class ui;
};
