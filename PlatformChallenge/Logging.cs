using System;

namespace PlatformChallenge
{
    class Logging
    {
        public static void HandleError(string callingModule, Exception e)
        {
            //In a full scale application this would log to a database, email or text the dev team, and maybe redirect to a custom error page.
            //This will print to the console, so that its a doing something with the error

            Console.WriteLine("**Error encontered in " + callingModule + ": " + e.Message);

            // e.StackTrace is also useful, but its verbose and I don't want to overload the console.
        }
    }
}
