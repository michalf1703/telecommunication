#ifndef CONST_H
#define CONST_H

#include <fstream>
#include <string.h>
#include <windows.h>
#include "iostream"
using namespace std;

/* KODY STERUJĄCE */
const char SOH = 0x1;   // Start Of Heading
const char EOT = 0x4;   // End Of Transmission
const char ACK = 0x6;   // Acknowledge              Zgoda na przesylanie danych
const char NAK = 0xF;   // Negative Acknowledge     Brak zgody na przesłanie danych
const char CAN=(char)18; // Cancel                  Flaga do przerwania połączenia (24?) 

/* INFORMACJE O PORCIE */
HANDLE          portHandle;            // identyfikator portu
LPCTSTR         portNumber;             // przechowuje nazwę portu
DCB             settings;   // struktura kontroli portu szeregowego
COMSTAT         portResources;            // dodatkowa informacja o zasobach portu
DWORD           codeError;                   // reprezentuje typ ewentualnego błędu
COMMTIMEOUTS    timeSettings;
USHORT tmpCRC;

/* NADANIE PORTOWI PARAMETROW + OTWORZENIE GO */
bool ustawieniaPortu( LPCTSTR numerPortu ) {

    portHandle = CreateFile(numerPortu, GENERIC_READ | GENERIC_WRITE, 0, NULL, OPEN_EXISTING, 0, NULL);
    if (portHandle != INVALID_HANDLE_VALUE)
    {
        settings.DCBlength = sizeof(settings);
        GetCommState(portHandle, &settings);
        settings.BaudRate=CBR_9600;     /* predkosc transmisji */
        settings.Parity = NOPARITY;     /* bez bitu parzystosci */
        settings.StopBits = ONESTOPBIT; /* ustawienie bitu stopu (jeden bit) */
        settings.ByteSize = 8;          /* liczba wysylanych bitów */

        settings.fParity = TRUE;
        settings.fDtrControl = DTR_CONTROL_DISABLE; /* Kontrola linii DTR: sygnal nieaktywny */
        settings.fRtsControl = RTS_CONTROL_DISABLE; /* Kontrola linii RTS: sygnal nieaktywny */
        settings.fOutxCtsFlow = FALSE;
        settings.fOutxDsrFlow = FALSE;
        settings.fDsrSensitivity = FALSE;
        settings.fAbortOnError = FALSE;
        settings.fOutX = FALSE;
        settings.fInX = FALSE;
        settings.fErrorChar = FALSE;
        settings.fNull = FALSE;

        timeSettings.ReadIntervalTimeout = 10000;
        timeSettings.ReadTotalTimeoutMultiplier = 10000;
        timeSettings.ReadTotalTimeoutConstant = 10000;
        timeSettings.WriteTotalTimeoutMultiplier = 100;
        timeSettings.WriteTotalTimeoutConstant = 100;

        SetCommState(portHandle, &settings);
        SetCommTimeouts(portHandle, &timeSettings);
        ClearCommError(portHandle, &codeError , &portResources);
        return true;
    }
    else { cout << "Port " << numerPortu << " is not available.\n" ; return false; }
}

#endif