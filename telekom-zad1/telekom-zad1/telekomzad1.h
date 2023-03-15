#pragma once

#include <QtWidgets/QWidget>
#include "ui_telekomzad1.h"

class telekomzad1 : public QWidget
{
    Q_OBJECT

public:
    telekomzad1(QWidget *parent = nullptr);
    ~telekomzad1();
private:
    Ui::telekomzad1Class ui;
};
