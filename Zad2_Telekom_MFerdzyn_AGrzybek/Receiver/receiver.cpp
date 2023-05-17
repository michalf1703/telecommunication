
#include "const.h"
using namespace std;

ofstream plik;

char dataBlock[128];
char charek;
unsigned long charSize= sizeof(charek);
int charCounter=1;      //potrzebne przy czytaniu i pisaniu
bool transmission=false;
bool correctPackage;
int blockNumber;
char to255Bits;
char CRCChecksum[2];      //odebrane CRCChecksum

int CalculateCRC(char *wsk, int count) {
    int checkSum = 0;
    while (--count >= 0)
    {   /* Dopełnienie znaku ośmioma zerami. */
        checkSum = checkSum ^ (int)*wsk++ << 8;

        for (int i = 0; i < 8; ++i)
/*  Jeśli lewy bit jest równy 1 wykonujemy operację XOR generatorem 1021.
    Jeśli jest równy 0, to XORujemy przez 0000. */
            if (checkSum & 0x8000) checkSum = checkSum << 1 ^ 0x1021;
            else checkSum = checkSum << 1;
    }
    return (checkSum & 0xFFFF); //1111 1111 1111 1111
}

int IsEven(int x, int y) {
    if( y == 0 ) return 1;
    if( y == 1 ) return x;

    int result = x;
    for( int i = 2; i <= y; i++ ) result = result * x;

    return result;
}


char CalculateCRCChar(int n, int charNumber) //przeliczanie CRC na postac binarna
{
    int x, binary[16];

    for(int z=0; z<16; z++) binary[z]=0;

    for(int i=0; i<16; i++)
    {
        x=n%2;
        if (x==1) n=(n-1)/2;
        if (x==0) n=n/2;
        binary[15 - i]=x;
    }

    //obliczamy poszczegolne znaki CRCChecksum (1-szy lub 2-gi)
    x=0;
    int k;

    if(charNumber == 1) k=7;
    if(charNumber == 2) k=15;

    for (int i=0; i<8; i++)
        x= x + IsEven(2, i) * binary[k - i];

    return (char)x;//zwraca 1 lub 2 charek (bo 2 znaki to 2 bajty, czyli 16 bitów)
}

int main()
{
    system("cls");
    cout << "Telekomunikacja XMODEM" << endl;
    cout << "Receiver" << endl;

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
    getchar();

    cout << "Do you want to use file transfer?" << endl
        << "1 - tak" << endl //ACK
        << "2 - nie" << endl; //NAK

    switch(getchar()) {
        case '1': {
            charek = 'C';
            break;
        }
        case '2': {
            charek = NAK;
            break;
        }
        default: {
            charek = 'C';
        }
    }

    for(int i=0;i<6;i++) {
        //HANDLE, LPVOID, DWORD, LPDWORD, LPOVERLAPPED
        WriteFile(portHandle, &charek, charCounter, &charSize, NULL);
        cout<<"Waiting for SOH message...\n";
        ReadFile(portHandle, &charek, charCounter, &charSize, NULL);
        cout << (int)charek << endl;
        if(charek == SOH) {
            cout <<  "Connection successful!\n" ;
            transmission = true;
            break;
        }
    }

    if(!transmission) {
        cout << "Connection failed.\n";
        exit(1);
    }
    plik.open("received.txt",ios::binary);
    cout<<"File is being received, please wait...\n";

    ReadFile(portHandle, &charek, charCounter, &charSize, NULL);
    blockNumber=(int)charek;

    ReadFile(portHandle, &charek, charCounter, &charSize, NULL);
    to255Bits=charek;

    for(int i=0;i<128;i++) {
        ReadFile(portHandle, &charek, charCounter, &charSize, NULL);
        dataBlock[i] = charek;
    }

    ReadFile(portHandle, &charek, charCounter, &charSize, NULL);
    CRCChecksum[0]=charek;
    ReadFile(portHandle, &charek, charCounter, &charSize, NULL);
    CRCChecksum[1]=charek;
    correctPackage=true;


    if ((char)(255 - blockNumber) != to255Bits) {
        cout << "Invalid package number.\n";
        WriteFile(portHandle, &NAK, charCounter, &charSize, NULL);
        correctPackage = false;
    }
    else {
        tmpCRC= CalculateCRC(dataBlock, 128); // sprawdzanie czy sumy kontrole sa poprawne

        if(CalculateCRCChar(tmpCRC, 1) != CRCChecksum[0] || CalculateCRCChar(tmpCRC, 2) != CRCChecksum[1]) {
            cout << "Invalid checksum.\n" ;
            WriteFile(portHandle, &NAK, charCounter, &charSize, NULL); //NAK
            correctPackage=false;
        }
    }

    if(correctPackage) {
        for(int i=0;i<128;i++) {
            if(dataBlock[i] != 26)
            plik << dataBlock[i];
        }
        cout << "Receiving a packet..." << blockNumber ;
        cout << "Packet received correctly.\n";
        WriteFile(portHandle, &ACK, charCounter, &charSize, NULL);
    }

    while(1) {
        ReadFile(portHandle, &charek, charCounter, &charSize, NULL);
        if(charek == EOT || charek == CAN) break;
        cout << "Receiving a packet...";
        cout << blockNumber + 1 ;

        ReadFile(portHandle, &charek, charCounter, &charSize, NULL);
        blockNumber=(int)charek;

        ReadFile(portHandle, &charek, charCounter, &charSize, NULL);
        to255Bits=charek;

        for(int i=0;i<128;i++) {
            ReadFile(portHandle, &charek, charCounter, &charSize, NULL);
            dataBlock[i] = charek;
        }


        ReadFile(portHandle, &charek, charCounter, &charSize, NULL);
        CRCChecksum[0]=charek;
        ReadFile(portHandle, &charek, charCounter, &charSize, NULL);
        CRCChecksum[1]=charek;
        correctPackage=true;

        if((char)(255 - blockNumber) != to255Bits) {
            cout << "Invalid package number.\n" ;
            WriteFile(portHandle, &NAK, charCounter, &charSize, NULL);
            correctPackage=false;
        }
        else {
            tmpCRC= CalculateCRC(dataBlock, 128);

            if(CalculateCRCChar(tmpCRC, 1) != CRCChecksum[0] || CalculateCRCChar(tmpCRC, 2) != CRCChecksum[1]) {
                cout <<  "Invalid checksum.\n" ;
                WriteFile(portHandle, &NAK, charCounter, &charSize, NULL);
                correctPackage=false;
            }
        }
        if(correctPackage) {
            for(int i=0;i<128;i++) {
                if(dataBlock[i] != 26)
                plik << dataBlock[i];
            }

            cout << "Packet received correctly.\n";
            WriteFile(portHandle, &ACK, charCounter, &charSize, NULL);
        }
    }
    WriteFile(portHandle, &ACK, charCounter, &charSize, NULL);

    plik.close();
    CloseHandle(portHandle);

    if(charek == CAN) cout << "Transmission error - the connection has been interrupted.\n" ;
    else cout <<  "File transfer completed successfully.\n" ;

 return 0;
}
