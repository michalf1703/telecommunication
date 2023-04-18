#include "telekomzad1.h"
#include <QtWidgets/QApplication>
#include "QtWidgetsApplication1.h"
#include <iostream>
#include <iostream>
#include <fstream>
#include <cmath>
#include <vector>
#include<iostream>
#include<string>
#include <windows.h>
#include "telekomzad1.h"
#include "DoubleErrorCode.h"
#include <bitset>
#include <locale.h>

using namespace std;
const int h = 8;
const int w = 16;


telekomzad1::telekomzad1(QWidget* parent)
    : QWidget(parent)
{
    ui.setupUi(this);




}

telekomzad1::~telekomzad1()
{

}



vector<bool> telekomzad1::zamienSlowoNaPostacBitowa(std::string slowo) {
    std::vector<char> znaki(slowo.length());
    std::copy(slowo.begin(), slowo.end(), znaki.begin());
    std::vector<bool> bity;
    for (int i = 0; i < znaki.size(); i++) {
        int helper = int(znaki.at(i));
        std::vector<bool> pomocniczy = ASCIItoBinary(helper);
        pomocniczy = dodajBityParzystosci(pomocniczy);
        for (int j = 0; j < 16; j++) {
            bity.push_back(pomocniczy.at(j));
        }
    }
    std::string wynik;
    for (int i = 0; i < bity.size(); i++) {
        if (bity.at(i) == 1) wynik += '1';
        else wynik += '0';
    }
    return zamienStringBitowyNaBity(wynik);
}

vector<bool> telekomzad1::zamienStringBitowyNaBity(std::string stringbitowy) {
    std::vector<bool> wektorek;
    for (int i = 0; i < stringbitowy.size(); i++) {
        if (stringbitowy.at(i) == '1') {
            wektorek.push_back(1);
        }
        else {
            wektorek.push_back(0);
        }
    }
    return wektorek;
}

vector<bool> telekomzad1::dodajBityParzystosci(std::vector<bool>wiadomosc) {
    int suma = 0;
    for (int i = 0; i < 8; i++) {
        int suma = 0;
        for (int j = 0; j < 8; j++) {
            if (DoubleErrorMatrix[i][j] == 1) suma += wiadomosc[j];
        }
        if (suma % 2 == 1) wiadomosc.push_back(1);
        else wiadomosc.push_back(0);
    }
    return wiadomosc;
}
vector<bool> telekomzad1::ASCIItoBinary(int znak) {
    std::vector<bool> binarka;
    while (znak > 0) {
        binarka.push_back(znak % 2);
        znak = znak / 2;
    }
    while (binarka.size() < 8) {
        binarka.push_back(0);
    }
    reverse(binarka.begin(), binarka.end());
    return binarka;
}



vector<bool> telekomzad1::asciiToBinary(const vector<char>& data) {
    vector<bool> binary;

    for (char znak : data) {
        bitset<8> b(znak); // konwersja znaku ASCII na bitset o rozmiarze 8 bitów
        for (int i = 0; i < 8; i++) {
            binary.push_back(b[7 - i]); // dodanie kolejnych bitów do wektora
        }
    }

    return binary;
}

string telekomzad1::zamienNaZakodowanyString(std::vector<bool> bity) {
    std::string wynik;
    for (int i = 0; i < bity.size() / 8; i++) {
        std::vector<bool> pomocniczy;
        for (int j = 0; j < 8; j++) {
            pomocniczy.push_back(bity.at(i * 8 + j));
        }
        wynik += char(BinaryToAscii(pomocniczy));
        pomocniczy.clear();
    }
    return wynik;
}
int telekomzad1::BinaryToAscii(std::vector<bool> bity) {
    int wynik = 0;
    for (int i = 7; i >= 0; i--) {
        if (bity.at(i) == 1)
            wynik += int(pow(2, (7 - i)));
    }
    return wynik;
}

string telekomzad1::poprawSlowo(std::string doPoprawienia) {
    std::vector<std::vector<bool>> vector;
    for (int i = 0; i < doPoprawienia.length() / 16; i++) {
        std::vector<bool> pomocniczy;
        for (int j = 0; j < 16; j++) {
            if (doPoprawienia.at(i * 16 + j) == '1') pomocniczy.push_back(1);
            else pomocniczy.push_back(0);
        }
        vector.push_back(pomocniczy);
    }



    std::string wynik;
    for (int i = 0; i < vector.size(); i++) {
        std::vector<bool> errors = iloczynHX(vector.at(i));
        std::vector<bool> fixedWord = fix(vector.at(i), findWrongColumns(errors));
        int ascii = BinaryToAscii(fixedWord);
        wynik += char(ascii);
    }
    return wynik;
}
string telekomzad1::konwertujZakodowaneSlowoNaPostacBitowa(std::string test) {         
    std::vector<unsigned char> znaki(test.begin(), test.end());
    std::vector<bool> bity;
    for (int i = 0; i < znaki.size(); i++) {
        int helper = int(znaki.at(i));
        std::vector<bool> pomocniczy = ASCIItoBinary(helper);       
        for (int j = 0; j < 8; j++) {
            bity.push_back(pomocniczy.at(j));
        }
    }
    std::string wynik;
    for (int i = 0; i < bity.size(); i++) {
        if (bity.at(i) == 1) wynik += '1';
        else wynik += '0';
    }
    return wynik;
}

vector<bool> telekomzad1::iloczynHX(std::vector<bool> tab) {
    std::vector<bool> wynik;
    for (int i = 0; i < 16; i++) {
        wynik.push_back(0);
    }
    for (int i = 0; i < h; i++) {
        for (int j = 0; j < w; j++) {
            wynik.at(i) = (wynik.at(i) + (DoubleErrorMatrix[i][j] * tab.at(j) % 2)) % 2;
        }
        wynik.at(i) = wynik.at(i) % 2;
    }
    return wynik;
}


vector<bool> telekomzad1::findWrongColumns(std::vector<bool> E) {
    bool gites = true;
    std::vector<bool> wrongColumns;
    for (int i = 0; i < 8; i++) {
        wrongColumns.push_back(0);
        wrongColumns.push_back(0);
        if (E.at(i) == 1) gites = false;
    }
    if (gites) return wrongColumns;
    for (int i = 0; i < 16; i++) {
        for (int j = 0; j < 8; j++) {
            if (DoubleErrorMatrix[j][i] != E[j]) { break; }
            if (j == 7) {
                wrongColumns.at(i) = 1; return wrongColumns;
            }
        }
    }

    for (int i = 0; i < E.size(); i++) {
        for (int j = 0; j < E.size(); j++) {
            for (int k = 0; k < 16; k++) {
                if ((DoubleErrorMatrix[k][j] + DoubleErrorMatrix[k][i]) % 2 != E[k]) { break; }
                if (k == 7) { wrongColumns.at(j) = 1; wrongColumns.at(i) = 1; return wrongColumns; }
            }
        }

    }

    return wrongColumns;
}
vector<bool> telekomzad1::fix(std::vector<bool> slowo, std::vector<bool> wrongColumns) {
    for (int i = 0; i < 16; i++) {
        if (wrongColumns.at(i) == 1) {
            slowo.at(i) = !slowo.at(i);
        }
    }
    return slowo;
}

void telekomzad1::callFunctions() {

    AllocConsole();
    // Ustawienie strumienia wyjœcia na konsole
    freopen("CONOUT$", "w", stdout);
    //ODCZYT
    std::string zPliku;
    std::fstream original;
    original.open("original.txt", std::ios::in);
    std::getline(original, zPliku);
    original.close();

    cout << "Wiadomosc do zakodowania:  " << zPliku << endl;


    //KODOWANIE SLOWA Z BITAMI PARZYSTOSCI
    std::vector<bool> binarka = zamienSlowoNaPostacBitowa(zPliku);
    std::string zakodowanyString = zamienNaZakodowanyString(binarka);
    cout << "Binarnie: " << endl;
    for (int i = 0; i < binarka.size(); i++) {
        cout << binarka[i];
        if ((i + 1) % 8 == 0) cout << "\n";
    }
    cout << "Ilosc bitow:" << binarka.size() << endl;

    fstream bitowo;
    bitowo.open("bitowo.txt", std::ios::out);
    bitowo << zakodowanyString;
    bitowo.close();
    cout << "\n\nMozna teraz wprowadzic zmiany w pliku bitowo.txt .\nWcisnij enter, aby kontynuowac." << endl;
    while (!GetAsyncKeyState(VK_RETURN)) {
        Sleep(100); // odczekaj 100ms
    }
    
    fstream odczytbitowy;
    odczytbitowy.open("bitowo.txt", std::ios::in);
    string biciki;
    std::getline(odczytbitowy, biciki);
    odczytbitowy.close();
    
    //POPRAWA SLOWA
    string zamienioneBiciki = konwertujZakodowaneSlowoNaPostacBitowa(biciki);
    string ostateczny = poprawSlowo(zamienioneBiciki);

    //ZAPIS
    std::fstream zapisStringowy;
    zapisStringowy.open("odkodowane.txt", std::ios::out);
    zapisStringowy << ostateczny;
    zapisStringowy.close();
    cout << endl;
    cout << "Odzyskana wiadomosc: " << ostateczny;
}