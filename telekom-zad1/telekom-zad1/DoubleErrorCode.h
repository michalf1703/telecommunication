//kod korygujacy jeden blad wymaga: 
// -nie posiada kolumny zerowej
// -wszystkie kolumny sa rozne
// -dodatkowo zadna kolumna nie moze byc suma dwoch pozostalych!!!
// dwa bledy korygowane s przez osiem bitow parzystosci
// jeden wiersz ---> jeden bit parzystosci

bool DoubleErrorMatrix[8][16] =
{
	{0, 1, 1, 1, 1, 1, 1, 1,	1, 0, 0, 0, 0, 0, 0, 0},
	{1, 0, 1, 1, 1, 1, 1, 1,	0, 1, 0, 0, 0, 0, 0, 0},
	{1, 1, 0, 1, 1, 1, 1, 1,	0, 0, 1, 0, 0, 0, 0, 0},
	{1, 1, 1, 0, 1, 1, 1, 1,	0, 0, 0, 1, 0, 0, 0, 0},
	{1, 1, 1, 1, 0, 1, 1, 1,	0, 0, 0, 0, 1, 0, 0, 0},
	{1, 1, 1, 1, 1, 0, 1, 1,	0, 0, 0, 0, 0, 1, 0, 0},
	{1, 1, 1, 1, 1, 1, 0, 1,	0, 0, 0, 0, 0, 0, 1, 0},
	{1, 1, 1, 1, 1, 1, 1, 0,	0, 0, 0, 0, 0, 0, 0, 1},
};
