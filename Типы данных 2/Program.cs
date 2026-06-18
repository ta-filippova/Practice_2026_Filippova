using System;

int diagonalLength = 9;
DrawDiamond(diagonalLength);

static void DrawDiamond(int n)
{
    if (n <= 0 || n % 2 == 0)
    {
        Console.WriteLine("Ошибка: N должно быть положительным нечётным числом.");
        return;
    }

    int center = n / 2;

    for (int row = 0; row < n; row++)
    {
        for (int col = 0; col < n; col++)
        {
            if (row == center && col == center)
            {
                Console.Write(" ");
                continue;
            }

            int distanceRow = Math.Abs(row - center);
            int distanceCol = Math.Abs(col - center);

            if (distanceRow + distanceCol == center)
            {
                Console.Write("X");
            }
            else
            {
                Console.Write(" ");
            }
        }

        Console.WriteLine();
    }
}