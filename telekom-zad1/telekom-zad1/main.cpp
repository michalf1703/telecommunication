#include "telekomzad1.h"
#include "QtWidgetsApplication1.h"
#include <QtWidgets/QApplication>
#include <iostream>
#include <fstream>
#include <cmath>
#include <vector>
#include<iostream>
#include<string>
#include "DoubleErrorCode.h"
#include <windows.h>

using namespace std;

vector<bool> string_to_bin(string str) {
    vector<bool> msg;
    for (int i = 0; i < str.size(); i++) {
        if (str[i] == '1') msg.push_back(1);
        else msg.push_back(0);
    }
    return msg;
}

vector<bool> char_to_bin(string str) {
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

char bin_to_char(vector<bool> bin) {
    char chr = 0;
    for (int i = 0; i < 8; i++) {
        chr += bin[i] * pow(2, i);
    }
    return chr;
}

int parity(vector<bool> msg, int n) {
    int p = 0;
    for (int i = 0; i < msg.size(); i++) {
        p += DoubleErrorMatrix[n][i] * msg[i];
    }
    p = p % 2;
    return p;
}

void addParity(vector<bool>& msg) {
    vector<bool> msgcopy = msg;
    for (int i = 0; i < 16; i++) {
        msg.push_back(parity(msgcopy, i));
    }
}

vector<bool> encode(vector<bool> msg) {
    vector<bool> code;
    vector<bool> bits;

    for (int i = 0; i < msg.size(); i++) {
        code.push_back(msg[i]);
        bits.push_back(msg[i]);

        if ((i + 1) % 8 == 0 && i != 0) {
            addParity(bits);
            for (int k = 0; k < 8; k++) {
                code.push_back(bits[k + 8]);
            }
            bits.clear();
        }
    }
    return code;
}

void adjust(vector<bool>& msg, vector<bool> err) {
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


void verify(vector<bool>& msg, int len) {
    bool verified = true;
    if (msg.size() != len) {
        cout << "Nieprawidlowa ilosc bitow (" << msg.size() << ")! Konczenie pracy progamu.\n";
        return;
    }
    vector<bool> err;
    int num;
    for (int i = 0; i < 8; i++) {
        num = parity(msg, i);
        err.push_back(num);
        if (num == 1) verified = false;
    }

    if (verified) cout << "OK" << endl;
    else {
        cout << "ERROR - ADJUSTING" << endl;
        adjust(msg, err);
    }
}

void callFunctions() {
   
    AllocConsole();
    // Ustawienie strumienia wyjœcia na konsolê
    freopen("CONOUT$", "w", stdout);
    vector<bool> message;
    vector<bool> encoded;
    vector<bool> err;
    string text;

    ifstream input_file;
    ofstream output_file;
    fstream encoded_file;

    input_file.open("message.txt");
    output_file.open("binary.txt");
    encoded_file.open("encoded.txt", ios::out);

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

    encoded = encode(message);
    int t = 0;
    cout << "\nZakodowana wiadomosc:" << endl;
    for (int i = 0; i < encoded.size(); i++) {
        encoded_file << encoded[i];
        cout << encoded[i] << " ";
        if ((i + 1) % 8 == 0) {
            cout << "\n";
            t++;
        }
    }
    cout << "Bity: " << encoded.size();
    string pobierz;
    input_file.close();
    output_file.close();
    encoded_file.close();
    cout << "\n\nMozna teraz wprowadzic zmiany w pliku.\nWcisnij Enter, aby kontynuowac." << endl;
    while (!GetAsyncKeyState(VK_LBUTTON)) {
        while (!GetAsyncKeyState(VK_RBUTTON)) {
            Sleep(100); // odczekaj 100ms
        }
        Sleep(100); // odczekaj 100ms
    }

    encoded_file.open("encoded.txt", ios::in);
    while (!encoded_file.eof()) {
        getline(encoded_file, text);
    }
    err = string_to_bin(text);
    cout << "Po zmianie:" << endl;
    for (int i = 0; i < err.size(); i++) {
        cout << err[i];
        if ((i + 1) % 8 == 0) cout << "\n";
    }
    cout << "\nBity: " << err.size() << endl;

    vector<vector<bool>> multivector;
    for (int i = 0; i < text_size; i++) {
        vector<bool> row;
        multivector.push_back(row);
    }
    vector<bool> fixed;
    int amount = 0;
    string ans;


    for (int i = 0; i < err.size(); i++) {
        multivector[amount].push_back(err[i]);
        if ((i + 1) % 16 == 0) amount++;
    }
    err.clear();

    fstream out_file;
    out_file.open("received.txt", ios::out);

    for (int i = 0; i < text_size; i++) {
        verify(multivector[i], encoded.size() / text_size);
        err.insert(err.end(), multivector[i].begin(), multivector[i].end());
        ans += bin_to_char(multivector[i]);
        out_file << bin_to_char(multivector[i]);
    }

    encoded_file.close();
    out_file.close();
    cout << "\nPoprawione:" << endl;
    for (int i = 0; i < err.size(); i++) {
        cout << err[i];
        if ((i + 1) % 8 == 0) cout << "\n";
    }
    cout << "Bity:\n" << err.size() << endl;
    cout << "\nOdebrana wiadomosc: " << ans << endl;

}



int main(int argc, char* argv[])
{
    
    QApplication a(argc, argv);
    QApplication b(argc, argv);
    QtWidgetsApplication1 x;
    telekomzad1 w;
    x.show();
    w.show();
    callFunctions();
    return a.exec();    
    return b.exec();

}
