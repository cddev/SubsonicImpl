using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SubsonicAPI;

/*
 this Console App allows us to test the api methods
 
 */
namespace Consoletest
{
	class Program
	{

		static ManualResetEvent resetEvent = new ManualResetEvent(false); //allow to block and release threads manually, this make possible the execution of an async function in console app
		static void Main()
		{
			MainAsync();
			resetEvent.WaitOne(); //wait until .set() is call
		}
		static async void MainAsync()
		{
			bool result = false;

			Console.WriteLine("Subsonic Server url (http/https):");
			string srv = Console.ReadLine();
			Console.WriteLine("username:");
			string usr = Console.ReadLine();
			Console.WriteLine("password:");
			string pwd = Console.ReadLine();

			Subsonic sub = new Subsonic();
			Console.WriteLine("connecting...");
			try
			{
				result = await sub.LogIn(srv,usr, pwd); 
			}
			catch (Exception ex) 
			{
				Console.WriteLine(ex.Message);
			}

			if(result)
			{
				Lyrics pl = await sub.getLyrics("Muse", "Blackout");
				Console.Write(pl.ToString());
			}

           
			
			 Console.ReadKey();
			 resetEvent.Set(); // tell the main theard to continue...
			
		}
		
	}
}
