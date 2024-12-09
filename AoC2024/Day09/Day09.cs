using System;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework.Constraints;

namespace AoC2024.Day09;

public class Day09
{
    [TestCase("Day09/input.txt", 6288707484810L)]
    [TestCase("Day09/example.txt", 1928)]
    public void Task1(string filePath, long expected)
    {
        var map = File.ReadAllText(filePath);
        var decompressed = new List<int>();
        for (var i = 0; i < map.Length; i++)
        {
            decompressed.AddRange(Enumerable.Repeat(i % 2 == 0 ? i / 2 : -1, map[i] - '0'));
        }

        var freeSpaceIndex = 0;
        for (var i = decompressed.Count - 1; i >= 0; i--)
        {
            if (decompressed[i] == -1)
                continue;

            while (freeSpaceIndex < decompressed.Count && decompressed[freeSpaceIndex] != -1)
                freeSpaceIndex++;

            if (freeSpaceIndex > i)
                break;

            decompressed[freeSpaceIndex] = decompressed[i];
            decompressed[i] = -1;
        }

        Checksum(decompressed)
            .Should()
            .Be(expected);
    }

    private static long Checksum(IEnumerable<int> decompressed)
    {
        return decompressed
                    .Select((i, f) => i == -1 ? 0 : (long)i * f)
                    .Sum();
    }

    [TestCase("Day09/input.txt", 6311837662089L)]
    [TestCase("Day09/example.txt", 2858)]
    public void Task2(string filePath, long expected)
    {
        var filesToCompress = new Stack<Block>();
        var input = File.ReadAllText(filePath);

        int i;
        var decompressedIndex = 0;
        Block? previous = null;
        Block? previousFree = null;
        Block root = null!;
        Block rootFree = null!;
        for (i = 0; i < input.Length; i++)
        {
            var block = new Block
            {
                FileId = i % 2 == 0 ? i / 2 : -1,
                Start = decompressedIndex,
                Length = input[i] - '0',
                Previous = previous,
                Next = null,
            };

            if (previous != null)
                previous.Next = block;
            else
                root = block;

            previous = block;

            decompressedIndex += block.Length;

            if (block.FileId == -1)
            {
                block.PreviousFree = previousFree;
                if (previousFree != null)
                    previousFree.NextFree = block;
                else
                    rootFree = block;

                previousFree = block;
            }
            else
            {
                filesToCompress.Push(block);
            }

        }

        while (filesToCompress.TryPop(out var file))
        {
            var freeBlock = rootFree;
            while (freeBlock != null && freeBlock.Length < file.Length)
                freeBlock = freeBlock.NextFree;

            if (freeBlock == null || freeBlock.Start >= file.Start)
                continue;

            var rest = new Block
            {
                FileId = -1,
                Start = freeBlock.Start + file.Length,
                Length = freeBlock.Length - file.Length,
                Previous = freeBlock,
                PreviousFree = freeBlock.PreviousFree,
                Next = freeBlock.Next,
                NextFree = freeBlock.NextFree,
            };

            freeBlock.Next = rest;
            if (rest.Next != null)
                rest.Next.Previous = rest;

            if (rest.PreviousFree == null)
                rootFree = rest;
            else
                rest.PreviousFree.NextFree = rest;

            if (rest.NextFree != null)
                rest.NextFree.PreviousFree = rest;

            freeBlock.FileId = file.FileId;
            freeBlock.Length = file.Length;

            file.FileId = -1;

            file.PreviousFree = file.Previous;
            while (file.PreviousFree != null && file.PreviousFree.FileId != -1)
                file.PreviousFree = file.PreviousFree.Previous;
            if (file.PreviousFree == null)
                rootFree = file;

            file.NextFree = file.Next;
            while (file.NextFree != null && file.NextFree.FileId != -1)
                file.NextFree = file.NextFree.Next;

            var leftMostFree = file;
            if (file.PreviousFree != null)
                leftMostFree = file.PreviousFree;

            while (leftMostFree.Next != null && leftMostFree.Next.FileId == -1)
            {
                Block next = leftMostFree.Next;
                leftMostFree.Length += next.Length;
                leftMostFree.Next = next.Next;
                leftMostFree.NextFree = next.NextFree;
                if (next.Next != null)
                {
                    next.Next.Previous = leftMostFree;
                    next.Next.PreviousFree = leftMostFree;
                }
            }
        }

        var map = new List<Block>();
        var current = root;
        while (current != null)
        {
            map.Add(current);
            current = current.Next;
        }
        Checksum(map.SelectMany(t => Enumerable.Repeat(t.FileId, t.Length)))
        .Should()
        .Be(expected);
    }

    private class Block
    {
        public int FileId { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }

        public Block? Previous { get; set; }
        public Block? Next { get; set; }
        public Block? PreviousFree { get; set; }
        public Block? NextFree { get; set; }

    }
}
