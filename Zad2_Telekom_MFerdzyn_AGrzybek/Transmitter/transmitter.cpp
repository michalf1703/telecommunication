
#include "const.h"

int CalculateCRC(char *wsk, int count) {
    int checksumCRC = 0;
    while (--count >= 0)
    {   /* Dopełnienie znaku ośmioma zerami. */
        checksumCRC = checksumCRC ^ (int)*wsk++ << 8;

        for (int i = 0; i < 8; ++i)
/*  Jeśli lewy bit jest równy 1 wykonujemy operację XOR generatorem 1021.
    Jeśli jest równy 0, to XORujemy przez 0000. */
            if (checksumCRC & 0x8000) checksumCRC = checksumCRC << 1 ^ 0x1021;
            else checksumCRC = checksumCRC << 1;
    }
    return (checksumCRC & 0xFFFF); //1111 1111 1111 1111
}

int IsEven(int x, int y) {
    if( y == 0 ) return 1;
    if( y == 1 ) return x;

    int result = x;
    for( int i = 2; i <= y; i++ ) result = result * x;

    return result;
}

/* Przeliczanie sumy CRC na postać binarną. */
char CalculateCRCChar(int n, int nrZnaku)
{
    int x, bin[16];
    for( int z = 0; z<16; z++ ) bin[z] = 0;

    for( int i = 0; i<16; i++ ) {
        x = n % 2;
        if( x == 1 ) n = (n-1)/2;
        if( x == 0 ) n = n/2;
        bin[15-i] = x;
    }

    //obliczamy poszczegolne znaki sumaKontrolnaCRC (1-szy lub 2-gi)
    x = 0;
    int k;

    if( nrZnaku == 1 ) k = 7;
    if( nrZnaku == 2 ) k = 15;

    for (int i=0; i<8; i++)
        x = x + IsEven(2, i) * bin[k - i];

    return (char)x;//zwraca 1 lub 2 znak (bo 2 znaki to 2 bajty, czyli 16 bitów)
}

int main()
{
    system("cls");
    cout << "Telekomunikacja XMODEM" << endl;
    cout << "Transmitter" << endl;
    ifstream plik;
    char charek;
    int charCounter = 1;
    unsigned long charSize = sizeof(charek);
    int kod;

    bool transmission = false;
    bool correctPackage;
    int blockNumber = 1;
    char dataBlock[128]; /* Plik dzielimy na bloki o długości 128 bajtów. */

/* Otwieranie portu do transmisji. */
    cout << "Select port number:" << endl
         << "1- COM1\n"
            "2- COM2\n"
            "3- COM3\n"
            "4- COM4\n"
            "(Default COM1)" << endl;

    switch(getchar()) {
        case '1': { portNumber = "COM1"; break; }
        case '2': { portNumber = "COM2"; break; }
        case '3': { portNumber = "COM3"; break; }
        case '4': { portNumber = "COM4"; break; }
        default: { portNumber = "COM1"; break; }
    }
    cout << portNumber << "Start transferring\n";
    if(ustawieniaPortu(portNumber) == false )  return 0;

    cout << "\nAwaiting permission to transmit... ";
    for(int i=0;i<6;i++) {
        ReadFile(portHandle, &charek, charCounter, &charSize, NULL);

        if(charek == 'C') {
            cout << "OK.\n";
            kod = 1;
            transmission = true;
            break;
        }
        else if(charek == NAK ){
            cout << "No agreement\n"; //Brak zgody
            kod = 2;
            transmission = true;
            break;
        }
    }
    if(!transmission) exit(1);

/* Transmisja pliku. */
    plik.open("transmitted.txt",ios::binary);
    while(!plik.eof())
    {
        /* Pusta tablica do zapisywania danych z jednego bloku. (char)26 - spacja */
        for( int i = 0; i < 128; i++ ) dataBlock[i] = (char)26;

        int w = 0;

        while( w<128 && !plik.eof() ) {
            dataBlock[w] = plik.get();
            if(plik.eof()) dataBlock[w] = (char)26;
            w++;
        }
        correctPackage = false;

        while(!correctPackage)
        {
            cout << "Packet sending";

            cout << blockNumber ;
            /* Wysłanie znaku początku nagłówka. */
            WriteFile(portHandle, &SOH, charCounter, &charSize, NULL);
            charek=(char)blockNumber;
            /* Wysłanie numeru bloku (1 bajt). */
            WriteFile(portHandle, &charek, charCounter, &charSize, NULL);
            charek= (char)255 - blockNumber;
            /* Wysłanie dopełnienia (255 - dataBlock). */
            WriteFile(portHandle, &charek, charCounter, &charSize, NULL);


            for( int i=0; i<128; i++ ) WriteFile(portHandle, &dataBlock[i], charCounter, &charSize, NULL);
            if( kod == 2 ) //suma kontrolna
            {
                char suma_kontrolna=(char)26;
                for(int i=0;i<128;i++)
                suma_kontrolna+= dataBlock[i] % 256;
                WriteFile(portHandle, &suma_kontrolna, charCounter, &charSize, NULL);
                cout<<"Checksum = " << (int)suma_kontrolna << endl;
            }
            else if(kod==1) //obliczanie CRC i transfer
            {
                tmpCRC= CalculateCRC(dataBlock, 128);
                charek= CalculateCRCChar(tmpCRC, 1);
                WriteFile(portHandle, &charek, charCounter, &charSize, NULL);
                charek= CalculateCRCChar(tmpCRC, 2);
                WriteFile(portHandle, &charek, charCounter, &charSize, NULL);
            }

            while(1) {
                charek=' ';
                ReadFile(portHandle, &charek, charCounter, &charSize, NULL);

                if(charek == ACK) {
                    correctPackage=true;
                    cout<<"Packet received correctly.\n";
                    break;
                }

                if(charek == NAK) {
                    cout << "Packet dispatch cancelled(NAK).\n" ;
                    break;
                }

                if(charek == CAN) {
                    cout << "Connection aborted.\n";
                    return 1;
                }
            }
        }
/* [ZABEZPIECZENIE] Jeśli nr bloku przekroczy 255 zaczynamy inkrementować od nowa. */
        if(blockNumber == 255 ) blockNumber = 1;
        else blockNumber++;
    }
    plik.close();

/* Zakończenie transmisji. */
    while(1) {
        charek = EOT;
        WriteFile(portHandle, &charek, charCounter, &charSize, NULL);
        ReadFile(portHandle, &charek, charCounter, &charSize, NULL);
        if(charek == ACK) break;
    }
    CloseHandle(portHandle);
    cout <<  "File transfer completed successfully.\n" ;

    return 0;
}
