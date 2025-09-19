using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TextProcessorApp
{
    class TextProcessor
    {
        public string RawText { get; private set; }
        public string ProcessedText { get; private set; }

        public void Input()
        {
            Console.WriteLine("Nhập đoạn văn bản:");
            RawText = Console.ReadLine();
            ProcessedText = RawText;
        }

        public void Normalize()
        {
            if (string.IsNullOrWhiteSpace(RawText))
            {
                ProcessedText = "";
                return;
            }
            string text = Regex.Replace(RawText, @"\s+", " ").Trim();
           
            string[] sentences = Regex.Split(text, @"(?<=[\.!\?])\s+");
            for (int i = 0; i < sentences.Length; i++)
            {
                string s = sentences[i].Trim();
                if (s.Length > 0)
                    sentences[i] = char.ToUpper(s[0]) + s.Substring(1);
            }
            ProcessedText = string.Join(" ", sentences);
        }

        public int GetWordCount()
        {
            if (string.IsNullOrWhiteSpace(ProcessedText)) return 0;
            string[] words = ProcessedText.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }

        public int GetDistinctWordCount()
        {
            if (string.IsNullOrWhiteSpace(ProcessedText)) return 0;
            string[] words = ProcessedText
                .ToLower()
                .Split(new char[] { ' ', '\t', '\n', '\r', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

            return words.Distinct().Count();
        }

        public Dictionary<string, int> GetWordFrequencies()
        {
            Dictionary<string, int> freq = new Dictionary<string, int>();

            if (string.IsNullOrWhiteSpace(ProcessedText)) return freq;

            string[] words = ProcessedText
                .ToLower()
                .Split(new char[] { ' ', '\t', '\n', '\r', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                if (freq.ContainsKey(word)) freq[word]++;
                else freq[word] = 1;
            }

            return freq;
        }

        public void DisplayResults()
        {
            Console.WriteLine("\n--- Văn bản chuẩn hóa ---");
            Console.WriteLine(ProcessedText);

            Console.WriteLine($"\nTổng số từ: {GetWordCount()}");
            Console.WriteLine($"Số từ khác nhau: {GetDistinctWordCount()}");

            Console.WriteLine("\n--- Bảng tần suất từ ---");
            foreach (var kv in GetWordFrequencies().OrderByDescending(k => k.Value))
            {
                Console.WriteLine($"{kv.Key} : {kv.Value}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            TextProcessor tp = new TextProcessor();
            tp.Input();

            tp.Normalize();
            tp.DisplayResults();
        }
    }
}
