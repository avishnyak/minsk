using System;

namespace EV2.EV2LanguageServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new App(Console.OpenStandardInput(), Console.OpenStandardOutput());

            Logger.Instance.Attach(app);

            try
            {
                app.Listen().Wait();
            }
            catch (AggregateException ex)
            {
                Console.Error.WriteLine(ex.InnerExceptions[0]);
                Environment.Exit(-1);
            }
        }
    }
}
