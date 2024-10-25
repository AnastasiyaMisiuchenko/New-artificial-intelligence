using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace NewAI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LearningChatBot bot = new LearningChatBot();
            bot.Start();
        }

        class LearningChatBot
        {
            private Dictionary<string, List<string>> knowledgeBase;
            private Random rand;  

            public LearningChatBot()
            {
                knowledgeBase = new Dictionary<string, List<string>>();
                rand = new Random();  
                LoadKnowledge();
            }

            public void Start()
            {
                Console.WriteLine("Привет! Я обучаемый чат-бот. Спрашивайте меня о чем угодно.");

                while (true)
                {
                    Console.Write("Вы: ");
                    string userInput = Console.ReadLine().ToLower();

                    if (userInput == "пока" || userInput == "выход")
                    {
                        Console.WriteLine("Бот: До свидания!");
                        Thread.Sleep(1000);
                        break;
                    }

                    if (knowledgeBase.ContainsKey(userInput))
                    {
                        var possibleResponses = knowledgeBase[userInput];

                        string response = possibleResponses[rand.Next(possibleResponses.Count)];

                        Console.WriteLine("Бот: " + response);
                    }
                    else
                    {
                        Console.WriteLine("Бот: Я не знаю ответа. Как мне ответить на это?");
                        string userResponse = Console.ReadLine();

                        if (!knowledgeBase.ContainsKey(userInput))
                        {
                            knowledgeBase[userInput] = new List<string>();
                        }

                        knowledgeBase[userInput].Add(userResponse);
                        SaveKnowledge();

                        Console.WriteLine("Бот: Спасибо, я запомню это!");
                    }
                }
            }

            private void LoadKnowledge()
            {
                if (File.Exists("knowledgeBase.txt"))
                {
                    string[] lines = File.ReadAllLines("knowledgeBase.txt");
                    foreach (string line in lines)
                    {
                        var parts = line.Split(new[] { "||" }, StringSplitOptions.None);
                        if (parts.Length >= 2)
                        {
                            string question = parts[0];
                            for (int i = 1; i < parts.Length; i++)
                            {
                                string answer = parts[i];
                                if (!knowledgeBase.ContainsKey(question))
                                {
                                    knowledgeBase[question] = new List<string>();
                                }

                                knowledgeBase[question].Add(answer);
                            }
                        }
                    }
                }
            }

            private void SaveKnowledge()
            {
                List<string> lines = new List<string>();
                foreach (var entry in knowledgeBase)
                {
                    foreach (var answer in entry.Value)
                    {
                        lines.Add(entry.Key + "||" + answer);
                    }
                }
                File.WriteAllLines("knowledgeBase.txt", lines);
            }
        }
    }
}
