﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

public class Concurrency
{
    public TimeSpan ConcurrentTimeElapsed { get; set; }

    public double TotalSum { get;set; }

    // This method will concurrently run 100 threads summing 100 million numbers
    public async void AddToTenBillionConcurrently()
    {
        Task[] taskList = new Task[100];

        double start = 1.0;
        double end = 100000000.0;

        for (int i = 0; i < 100; i++)
        { 
            taskList[i] = (new Task(() => AddToOneHundredMillion(start, end)));
            double s = AddToOneHundredMillion(start, end);
            TotalSum += s;
            start += 100000000.0; // add 100 million
            end += 100000000.0; // add 100 million
        }

        // sum in increments of 100 million 100 times concurrently up to 10 billion
        Stopwatch sw = Stopwatch.StartNew();
        foreach (Task t in taskList)
        {
            t.Start();
        }
        Task.WaitAll(taskList);
        sw.Stop();

        ConcurrentTimeElapsed += sw.Elapsed;

        Console.WriteLine("Time elapsed: " + ConcurrentTimeElapsed);
        Console.WriteLine("Total sum: " + TotalSum);
    } // end method

    // This method will add the sum of each number from 0 to 100 million
    public double AddToOneHundredMillion(double start, double end)
    {
        //Stopwatch sw = Stopwatch.StartNew();
        double sum = 0.0;
        for (double i = start; i <= end; i++)
        {
            sum += i;
        }

        //ConcurrentTimeElapsed += sw.Elapsed;
        //Console.WriteLine("Time elapsed: " + ConcurrentTimeElapsed);

        return sum;
    } // end method 

    // This method syncrhonously sums to 10 billion.
    public void SumTo10Billion()
    {
        Stopwatch sw = Stopwatch.StartNew();
        double sum = 0.0;
        for (double i = 1.0; i <= 10000000000.0; i++)
        {
            sum += i;
        }
        sw.Stop();

        ConcurrentTimeElapsed += sw.Elapsed;
        Console.WriteLine("Time elapsed: " + ConcurrentTimeElapsed);

        TotalSum = sum;
        Console.WriteLine("Total Sum: " + TotalSum);
    } // end method

    static void Main(string[] args)
    {
        Concurrency c = new();
        //c.AddToTenBillionConcurrently(); // Time elapsed: 00:00:04.7342673     Total sum:  5.000000000006786E+19

        //c.AddToOneHundredMillion(1, 100000000); // Time elapsed: 00:00:00.2705015

        //c.SumTo10Billion(); // Time elapsed: 00:00:27.5756065                   Total sum:  5.000000000006786E+19


    } // end Main
} // end class