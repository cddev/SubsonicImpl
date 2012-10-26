using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SubsonicAPI;

namespace Consoletest
{
	class Program
	{

		static ManualResetEvent resetEvent = new ManualResetEvent(false);
		static void Main()
		{
			MainAsync();
			resetEvent.WaitOne();
		}
		static async void MainAsync()
		{
			Subsonic sub = new Subsonic();
			Console.WriteLine("connecting...");
			Task<bool> taskLogin = sub.LogIn("serv", "usr", "pwd");

			bool result = await taskLogin;

			Task<Lyrics> taskGetArtist = sub.getLyrics("Muse","Blackout");
			Lyrics pl = await taskGetArtist;

           Console.Write(pl.ToString());
			
			 Console.ReadKey();
			 resetEvent.Set();
			
		}
		
	}
}
