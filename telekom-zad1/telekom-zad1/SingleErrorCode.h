//kod korygujacy jeden blad wymaga: 
// -nie posiada kolumny zerowej
// -wszystkie kolumny sa rozne
// jeden blad korygowany jest przez cztery bity parzystosci
// jeden wiersz ---> jeden bit parzystosci


bool SingleErrorMatrix[4][12] =
{
    {0, 1, 1, 1, 0, 1, 1, 0,     1, 0, 0, 0},
    {1, 0, 1, 1, 0, 0, 1, 1,     0, 1, 0, 0},
    {1, 1, 0, 1, 1, 0, 0, 1,     0, 0, 1, 0},
    {1, 1, 1, 0, 1, 1, 0, 0,     0, 0, 0, 1},
};
