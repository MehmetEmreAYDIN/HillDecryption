char[] alphabet = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ".ToCharArray();	//Enter the letters of alphabet in this field.
char[] cipherText = "GJŞVZEĞHOÖRG".ToUpper().ToCharArray();	//Enter the cipher text in this field.
int[] matrix = { 3, 2, 4, 1, 3, 5, 0, 2, 1 };   //Enter the numbers in the key matrix with commas between them in this field. Warning! The key matrix must be a regular matrix.

int n = (int)Math.Sqrt(matrix.Length);
int[,] keyMatrix = new int[n, n];
for (int i = 0; i < n; i++)
{
    for (int j = 0; j < n; j++)
    {
        keyMatrix[i, j] = matrix[i * n + j];
    }
}

int det = Determinant(keyMatrix);
int invDet = 1;
if (det < 0)
{
    invDet = -1;
    while ((det * invDet) % alphabet.Length != 1) invDet--;
}
else while ((det * invDet) % alphabet.Length != 1) invDet++;

int[,] invKeyMatrix = new int[n, n];
for (int i = 0; i < n; i++)
{
    for (int j = 0; j < n; j++)
    {
        int[,] minorMatrix = new int[n - 1, n - 1];
        int mRow = 0, mCol = 0;
        for (int k = 0; k < n; k++)
        {
            if (k == i) continue;
            mCol = 0;
            for (int l = 0; l < n; l++)
            {
                if (l == j) continue;
                minorMatrix[mRow, mCol] = keyMatrix[k, l];
                mCol++;
            }
            mRow++;
        }
        invKeyMatrix[j, i] = Determinant(minorMatrix) * (((i + j) % 2 == 0) ? 1 : -1) * invDet ;
    }
}

string plainText = string.Empty;
for (int i = 0; i < cipherText.Length; i += n)
{
    int[] matrix2 = new int[n];
    for (int j = 0; j < n; j++)
    {
            matrix2[j] = Array.IndexOf(alphabet, cipherText[i + j]);
    }
    for (int l = 0; l < n; l++)
    {
        int sum = 0;
        for (int k = 0; k < n; k++)
        {
            sum += matrix2[k] * invKeyMatrix[l, k];
        }
        if (sum < 0) while (sum < 0) sum += alphabet.Length;
        else sum %= alphabet.Length;
        plainText += alphabet[sum];
    }
}
Console.WriteLine(plainText);

static int Determinant(int[,] keyMatrix)
{
    int n = keyMatrix.GetLength(0);
    int det = 0;

    if (n == 1)
    {
        return keyMatrix[0, 0];
    }
    else if (n == 2)
    {
        return keyMatrix[0, 0] * keyMatrix[1, 1] - keyMatrix[0, 1] * keyMatrix[1, 0];
    }

    for (int i = 0; i < n; i++)
    {
        int[,] matrix2 = new int[n - 1, n - 1];
        for (int row = 1; row < n; row++)
        {
            int k = 0;
            for (int column = 0; column < n; column++)
            {
                if (column == i)
                {
                    continue;
                }
                matrix2[row - 1, k] = keyMatrix[row, column];
                k++;
            }
        }
        det += keyMatrix[0, i] * Determinant(matrix2) * ((i % 2 == 0) ? 1 : -1);
    }
    return det;
}
//This determinant method was taken from the YouTube channel of Selçuk University Computer Engineering student Muhammed Lütfi AKBAŞ.
//Here is the link: https://www.youtube.com/watch?v=zXmhmZTbuWY