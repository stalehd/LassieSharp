using System;
using System.Threading.Tasks;

namespace listapps
{
    class Program
    {
       static async Task ListApps(Lassie.Client client) {
            var apps = await client.ListApplicationsAsync();
            foreach (Lassie.Application app in apps.Applications ) {
                Console.WriteLine("App EUI: {0} name: {1}", app.ApplicationEUI, app.Tags["name"]);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Getting applications....");

            var client = new Lassie.Client();
            var t = Task.Run(() => ListApps(client));
            t.Wait();
        }
    }
}
