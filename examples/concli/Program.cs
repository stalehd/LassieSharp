using System;

namespace concli
{
    /// <summary>
    /// A simple Congress CLI that uses the C# libraries. Experience tells us that
    /// an actual utility will do wonders when it comes to testing and debugging
    /// the client library. You can use "cd" and "ls" to look at the resources in
    /// the REST API.
    /// </summary>
    class Program
    {
        void MainLoop(Lassie.Client client)
        {
            var ctx = new NodeTree(client);
            while (true)
            {
                Console.Write("[congress:{0}]: ", ctx.CurrentEntry.FullPath);
                var cmd = Console.ReadLine().ToLower();
                var param = cmd.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                switch (param[0])
                {
                    case "quit":
                        return;
                    case "exit":
                        return;
                    case "ls":
                        foreach (Node entry in ctx.CurrentEntry.Children())
                        {
                            Console.WriteLine("{0}/", entry.Name);
                        }
                        Console.WriteLine();
                        foreach (string attr in ctx.CurrentEntry.Attributes())
                        {
                            Console.WriteLine("{0}", attr);
                        }
                        break;
                    case "cd":
                        var newEntry = ctx.ChangeEntry(cmd.Substring(param[0].Length));
                        if (newEntry == null)
                        {
                            Console.WriteLine("Couldn't cd to {0}", cmd.Substring(param[0].Length));
                        }
                        break;
                    case "help":
                        Console.WriteLine("{0,-20}: Show help (you figured out this one)", "help");
                        Console.WriteLine("{0,-20}: List applications, deviecs and gateways", "ls");
                        Console.WriteLine("{0,-20}: Select resource", "cd <resource>");
                        break;
                    default:
                        Console.WriteLine("**** Don't know how to {0}", cmd);
                        break;
                }
            }

        }
        static void Main()
        {
            var p = new Program();
            var client = new Lassie.Client();
            p.MainLoop(client);
        }
    }
}
