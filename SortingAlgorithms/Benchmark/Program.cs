using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Helpers;

using BubbleSorting = BubbleSort.BubbleSort;
using InsertSorting = InsertSort.InsertSort;
using MergeSorting = MergeSort.MergeSort;
using QuickSorting = QuickSort.QuickSort;
using ShellSorting = ShellSort.ShellSort;

namespace Benchmark
{
    public class BenchmarkResult
    {
        public string Algorithm { get; set; }
        public int ItemsCount { get; set; }
        public long ElapsedTicks { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public long BytesUsed { get; set; }
        public bool Correct { get; set; }
    }

    internal class Program
    {
        public static BenchmarkResult Sort(Action<int[]> sortAction, int[] array, string alg)
        {
            array = array.ToArray();

            var sw = new Stopwatch();

            var startBytes = GC.GetTotalMemory(true);
            sw.Start();
            sortAction(array);
            sw.Stop();
            var endBytes = GC.GetTotalMemory(false);

            return new BenchmarkResult()
            {
                Algorithm = alg,
                ItemsCount = array.Length,
                ElapsedMilliseconds = sw.ElapsedMilliseconds,
                ElapsedTicks = sw.ElapsedTicks,
                BytesUsed = endBytes - startBytes,
                Correct = array.IsAscSort()
            };
        }

        public static IReadOnlyList<BenchmarkResult> StartBenchmark(
            Dictionary<string, Action<int[]>> sortAlgorithmsDict, int[] counts)
        {
            var result = new List<BenchmarkResult>();
            foreach (var count in counts)
            {

                var array = Enumerable.Range(1, count).Shuffle().ToArray();
                foreach (var kv in sortAlgorithmsDict)
                {
                    result.Add(Sort(kv.Value, array, kv.Key));
                }

            }

            return result;
        }

        public static void Main(string[] args)
        {
            var sortAlgorithmsDict =
                new Dictionary<string, Action<int[]>>
                {
                    [nameof(BubbleSorting)] = BubbleSorting.Start,
                    [nameof(InsertSorting)] = InsertSorting.Start,
                    [nameof(MergeSorting)] = MergeSorting.Start,
                    [nameof(QuickSorting)] = QuickSorting.StartSerial,
                    [nameof(ShellSorting)] = ShellSorting.Start,
                    ["OrderBySorting"] = (array) =>
                    {
                        var sorted = array.OrderBy(x => x).ToArray();
                        
                    }
                };

            var testsCount = 4;
            
            var counts = new int[testsCount];
            counts[0] = 100;
            for (int i = 1; i < testsCount; i++)
            {
                counts[i] = 10 * counts[i - 1];
            }
            
            var results = StartBenchmark(sortAlgorithmsDict, counts);

            AlgorithmAnalisys(results);

            Console.WriteLine("=====================");
            
            ElapsedTimeAnalisys(results);
        }

        private static void ElapsedTimeAnalisys(IReadOnlyList<BenchmarkResult> results)
        {
            foreach (var g in results.GroupBy(r => r.ItemsCount))
            {
                Console.WriteLine();
                Console.WriteLine(g.Key + " items results:");
                var algResults = g.OrderBy(x => x.ElapsedTicks).ToList();
                foreach (var algResult in algResults)
                {
                    if (!algResult.Correct)
                    {
                     //   throw new InvalidOperationException();
                    }

                    Console.WriteLine($"Algorithm = {algResult.Algorithm}\tTicks = {algResult.ElapsedTicks}\t\t" +
                                      $"ms = {algResult.ElapsedMilliseconds}\t\tBytes = {algResult.BytesUsed}");
                }
            }
        }

        private static void AlgorithmAnalisys(IReadOnlyList<BenchmarkResult> results)
        {
            foreach (var g in results.GroupBy(r => r.Algorithm))
            {
                Console.WriteLine();
                Console.WriteLine(g.Key + " results:");
                var algResults = g.OrderBy(x => x.ItemsCount).ToList();
                foreach (var algResult in algResults)
                {
                    if (!algResult.Correct)
                    {
                     //   throw new InvalidOperationException();
                    }

                    Console.WriteLine($"Count = {algResult.ItemsCount}\t\tTicks = {algResult.ElapsedTicks}\t\t" +
                                      $"ms = {algResult.ElapsedMilliseconds}\t\tBytes = {algResult.BytesUsed}");
                }
            }
        }
    }
}