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

using namespace std;

telekomzad1::telekomzad1(QWidget *parent)
    : QWidget(parent)
{
    ui.setupUi(this);


    
    
}

telekomzad1::~telekomzad1()
{
   
}

vector<bool> telekomzad1::string_to_bin(string str) {
    vector<bool> msg;
    for (int i = 0; i < str.size(); i++) {
        if (str[i] == '1') msg.push_back(1);
        else msg.push_back(0);
    }
    return msg;
}

vector<bool> telekomzad1::char_to_bin(string str) {
    vector<bool> bin;
    int chr;
    for (int i = 0; i < str.size(); i++) {
        chr = str[i];
        for (int k = 0; k < 8; k++) {
            bin.push_back(chr % 2);
            chr = chr / 2;
        }
    }
    return bin;
}

char telekomzad1::bin_to_char(vector<bool> bin) {
    char chr = 0;
    for (int i = 0; i < 8; i++) {
        chr += bin[i] * pow(2, i);
    }
    return chr;
}

int telekomzad1::bitKontrolny(vector<bool> msg, int n) {
    int p = 0;
    for (int i = 0; i < msg.size(); i++) {
        p += DoubleErrorMatrix[n][i] * msg[i];
    }
    p = p % 2;
    return p;
}

void telekomzad1::dodajBitKontrolny(vector<bool>& msg) {
    vector<bool> msgcopy = msg;
    for (int i = 0; i < 16; i++) {
        msg.push_back(bitKontrolny(msgcopy, i));
    }
}

vector<bool> telekomzad1::kodowanie(vector<bool> msg) {
    vector<bool> code;
    vector<bool> bits;

    for (int i = 0; i < msg.size(); i++) {
        code.push_back(msg[i]);
        bits.push_back(msg[i]);

        if ((i + 1) % 8 == 0 && i != 0) {
            dodajBitKontrolny(bits);
            for (int k = 0; k < 8; k++) {
                code.push_back(bits[k + 8]);
            }
            bits.clear();
        }
    }
    return code;
}

void telekomzad1::regulacja(vector<bool>& msg, vector<bool> err) {
    bool adjusted = false;
    for (int i = 0; i < 16; i++) {
        for (int k = 0; k < 8; k++) {
            if (err[k] == DoubleErrorMatrix[k][i]) adjusted = true;
            else {
                adjusted = false;
                break;
            }
        }
        if (adjusted) {
            msg[i] = (~msg[i]) % 2;
        }
    }
}


void telekomzad1::weryfikacja(vector<bool>& msg, int len) {
    bool verified = true;
    if (msg.size() != len) {
       std::cout << "Nieprawidlowa ilosc bitow (" << msg.size() << ")! Konczenie pracy progamu.\n";
        return;
    }
    vector<bool> err;
    int num;
    for (int i = 0; i < 8; i++) {
        num = bitKontrolny(msg, i);
        err.push_back(num);
        if (num == 1) verified = false;
    }

    if (verified) std::cout << "OK" << endl;
    else {
        std::cout << "ERROR - ADJUSTING" << endl;
        regulacja(msg, err);
    }
}

void telekomzad1::zapsiszZakodowanyDoASCII(std::vector<bool> bits, std::string filename) {
    std::string str = "";
    for (int i = 0; i < bits.size(); i += 8) {
        int code = 0;
        for (int j = 0; j < 8; j++) {
            code <<= 1;
            code |= bits[i + j];
        }
        str += char(code);
    }
    std::ofstream file(filename);
    if (file.is_open()) {
        file << str;
        file.close();
        std::cout << "Zakodowany plik zapisano pomyslnie." << std::endl;
    }
    else {
        std::cerr << "Blad. Nie udalo sie zapisac pliku." << std::endl;
    }
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


void telekomzad1::callFunctions() {

    AllocConsole();
    // Ustawienie strumienia wyjœcia na konsolê
    freopen("CONOUT$", "w", stdout);
    vector<bool> message;
    vector<bool> encoded;
    //vector<char> err;
    string text;

    ifstream input_file;
    ofstream output_file;
  //  fstream encoded_file;

    input_file.open("wiadomosc.txt");
    output_file.open("binarnie.txt");
  //  encoded_file.open("encoded.txt", ios::out);

    while (!input_file.eof()) {
        getline(input_file, text);
    }
    int text_size = text.size();

    message = char_to_bin(text);
    cout << "Wiadomosc: " << text << "\nBinarnie:\n";
    for (int i = 0; i < message.size(); i++) {
        cout << message[i];
        output_file << message[i];
        if ((i + 1) % 8 == 0) cout << "\n";
    }

    cout << "Bity:" << message.size() << endl;

    encoded = kodowanie(message);
    //int t = 0;
   // for (int i = 0; i < encoded.size(); i++) {
      //  encoded_file << encoded[i];
       // cout << encoded[i] << " ";
       // if ((i + 1) % 8 == 0) {
       //     cout << "\n";
        //    t++;
       // }
   // }
    zapsiszZakodowanyDoASCII(encoded, "zakodowane.txt");

   // string pobierz;
    input_file.close();
    output_file.close();
   // encoded_file.close();
    cout << "\n\nMozna teraz wprowadzic zmiany w pliku zakodowane.txt .\nWcisnij enter, aby kontynuowac." << endl;
    while (!GetAsyncKeyState(VK_RETURN)) {
        Sleep(100); // odczekaj 100ms
    }
    ifstream plik("zakodowane.txt"); // otwarcie pliku do odczytu
    vector<char> err;
    char znak;

    // odczytanie kolejnych znaków z pliku
    while (plik.get(znak)) {
        err.push_back(znak);
    }

    // zamiana na postaæ binarn¹
    vector<bool> binarny = asciiToBinary(err);

    cout << "Po modyfikacji: " << endl;
    for (int i = 0; i < binarny.size(); i++) {
        cout << binarny[i];
        if ((i + 1) % 8 == 0) cout << "\n";
    }
    cout << "\nBity: " <<binarny.size() << endl;
    vector<vector<bool>> multivector;
    for (int i = 0; i < text_size; i++) {
        vector<bool> row;
        multivector.push_back(row);
    }
    vector<bool> fixed;
    int amount = 0;
    string ans;


    for (int i = 0; i < binarny.size(); i++) {
        multivector[amount].push_back(binarny[i]);
        if ((i + 1) % 16 == 0) amount++;
    }
    binarny.clear();

    fstream out_file;
    out_file.open("odzyskana_wiadomosc.txt", ios::out);

    for (int i = 0; i < text_size; i++) {
        weryfikacja(multivector[i], encoded.size() / text_size);
        binarny.insert(binarny.end(), multivector[i].begin(), multivector[i].end());
        ans += bin_to_char(multivector[i]);
        out_file << bin_to_char(multivector[i]);
    }

    out_file.close();
    cout << "\nPoprawione:" << endl;
    for (int i = 0; i < binarny.size(); i++) {
        cout << binarny[i];
        if ((i + 1) % 8 == 0) cout << "\n";
    }
    cout << "\nOdebrana wiadomosc: " << ans << endl;
    
}
