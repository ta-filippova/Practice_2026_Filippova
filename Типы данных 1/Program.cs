using System;
using System.Text;

double deposit = 5000.00;
int years = 5;
int rate = 5;

string resultString = CalculateCashSavings(deposit, years, rate);
Console.WriteLine(resultString);


static string CalculateCashSavings(double initialDeposit, int years, int interestRate)
{
    StringBuilder resultBuilder = new StringBuilder();
    double cash = initialDeposit;

    for (int i = 0; i < years; i++)
    {
        cash += cash * interestRate * 0.01;
        resultBuilder.AppendLine($"Год {i + 1}: {cash:F2} руб.");
    }

    return resultBuilder.ToString();
}