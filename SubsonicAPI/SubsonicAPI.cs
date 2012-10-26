/*
This is a C# implementation of the Subsonic API (http://www.subsonic.org/pages/api.jsp)
 * 
 * Copyright (C) <2012>  <Clement devillez>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * 
 * 
By Clément Devillez (http://cd-dev.fr) 
 */


using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SubsonicAPI
{

	#region Utilities
		#region Classes
		/// <summary>
		/// To simplify data manipulation in front
		/// </summary>
		public class SubsonicItem
		{
		
			public string Name;
			public string id;

			public enum SubsonicItemType
			{
				Folder, Song
			}

			public SubsonicItemType ItemType;

			public override string ToString()
			{
				return Name;
			}
		}
		public class MusicFolder : SubsonicItem
		{
			#region private vars

			private List<MusicFolder> _Folders;
			private List<Song> _Songs;
			private List<Artist> _Artists;
			private List<Album> _Albums;

			#endregion private vars

			#region properties

			public List<MusicFolder> Folders
			{
				get { return _Folders; }
				set { _Folders = value; }
			}

			public List<Song> Songs
			{
				get { return _Songs; }
				set { _Songs = value; }
			}
			public List<Artist> Artists
			{
				get{return _Artists; }
				set{_Artists = value; }
			}
			public List<Album> Albums
			{
				get { return _Albums; }
				set { _Albums = value; }
			}
			#endregion properties

			public MusicFolder()
			{
				_Folders = new List<MusicFolder>();
				_Songs = new List<Song>();

				base.ItemType = SubsonicItemType.Folder;
			}

			public MusicFolder(string theName, string theId)
			{
				_Folders = new List<MusicFolder>();
				_Songs = new List<Song>();

				base.Name = theName;
				base.id = theId;

				base.ItemType = SubsonicItemType.Folder;
			}

			~MusicFolder() { }

			public void AddSong(Song newSong)
			{
			
				_Songs.Add(newSong);
			}

			public void AddFolder(string name, string id)
			{
				MusicFolder newFolder = new MusicFolder(name, id);
				_Folders.Add(newFolder);
			}

			public Song FindSong(string theTitle)
			{
				Song theSong = _Songs.Find(
					delegate(Song sng)
					{
						return sng.Name == theTitle;
					}
				);

				return theSong;
			}

			public MusicFolder FindFolder(string theFolderName)
			{
				MusicFolder theFolder = _Folders.Find(
					delegate(MusicFolder fldr)
					{
						return fldr.Name == theFolderName;
					}
				);

				return theFolder;
			}
		}
		public class Song
		{
			public string Id { get; set; }
			public string Name { get; set; }
			public string Parent { get; set; }
			public string Album { get; set; }
			public string Artist { get; set; }
			public string CoverArt { get; set; }
			public string Created { get; set; }
			public string Duration { get; set; }
			public string BitRate { get; set; }
			public string Track { get; set; }
			public string Year { get; set; }
			public string Genre { get; set; }
			public string Size { get; set; }
			public string Suffix { get; set; }
			public string ContentType { get; set; }
			public string AlbumId { get; set; }
			public string ArtistId { get; set; }
			public string Username{get;set;}
			public string PlayerId{get;set;}
			public string PlayerName{get;set;}
			public string MinutesAgo{get;set;}
			public string Starred{get;set;}

			public override string ToString()
			{


				return string.Format(@"Id:{0}|
										Name:{1}|
										Parent:{2}|
										Album:{3}|
										Artist:{4}|
										CoverArt:{5}|
										Created:{6}|
										Duration:{7}|
										BitRate:{8}|
										Track:{9}|
										Year:{10}|
										Genre:{11}|
										Size:{12}|
										Suffix:{13}|
										ContentType:{14}|
										AlbumId:{15}|
										ArtistId:{16}|
										Username:{17}|
										PlayerId:{18}|
										PlayerName:{19}|
										MinutesAgo:{20}|
										Starred:{21}", 
										 Id,
										 Name,
										 Parent,
										 Album,
										 Artist,
										 CoverArt,
										 Created,
										 Duration,
										 BitRate,
										 Track,
										 Year,
										 Genre,
										 Size,
										 Suffix,
										 ContentType,
										 AlbumId,
										 ArtistId,
										 Username,
										 PlayerId,
										 PlayerName,
										 MinutesAgo,
										 Starred);
			}

		
		}
		public class Artist
		{	
			public string Id{get;set;}
			public string Name{get;set;}
			public string CoverArt { get; set;}
			public string AlbumCount{get;set;}
			public List<Album> AlbumList{get;set;}

			public override string ToString()
			{
				return string.Format(@"id:{0}|
										Name:{1}|
										CoverArt:{2}|
										AlbumCount:{3}", Id, Name, CoverArt, AlbumCount);
			}
		}
		public class Album 
		{
			public string id{get;set;}
			public string Name{get;set;}
			public string CoverArt{get;set;}
			public string SongCount{get;set;}
			public string Created{get;set;}
			public string Duration{get;set;}
			public string Artist{get;set;}
			public string ArtistId{get;set;}	
			public List<Song> SongList{get;set;}
			public string userRating{get;set;}
			public string averageRating{get;set;}
			public string starred{get;set;}

			public override string ToString()
			{
				return string.Format(@"id:{0}|
										Name:{1}|
										CoverArt:{2}|
										SongCount:{3}|
										Created:{4}|
										Duration:{5}|
										Artist:{6}|
										ArtistId:{7}|
										userRating:{8}|
										averageRating:{9}|
										starred:{10}", id, Name, CoverArt, SongCount,Created,Duration,Artist,ArtistId,userRating,averageRating,starred);
			}
		}
		public class Playlist
		{
			public string Id{get;set;}
			public string Name{get;set;}
			public string Comment{get;set;}
			public string Owner{get;set;}
			public string IsPublic{get;set;}
			public string SongCount{get;set;}
			public string Duration{get;set;}
			public string Created{get;set;}
			private List<User> _AllowedUser = new List<User>();
			private List<Song> _Songs = new List<Song>();

			public List<Song> Songs
			{
				get{return _Songs;}
				set{ _Songs = value;}		
			}
			public List<User> AllowedUser
			{
				get{return _AllowedUser;}
				set{_AllowedUser = value;}
			}
			public Playlist()
			{			
			
			}
			public override string ToString()
			{
				string alU = "";
				string alS = "";
				foreach(Song s in _Songs)
				{
					alS += s.ToString();
				}
				foreach (User u in _AllowedUser)
				{
					alU += u.ToString();
				}

				return string.Format(@"id:{0},Name:{1},Comment:{2},Owner:{3},Public:{4},SongCount:{5},Duration:{6},Created:{7}
					AllowedUser:
						{8}
					Songs:
						{9}",
									 Id, Name, Comment, Owner, IsPublic, SongCount, Duration, Created,alU,alS);
			}
		}
		public class User
		{
		
			public string Name{get;set;}
			public string email { get; set; }
			public string scrobblingEnabled { get; set; }
			public string adminRole { get; set; }
			public string settingsRole { get; set; }
			public string downloadRole { get; set; }
			public string uploadRole { get; set; }
			public string playlistRole { get; set; }
			public string coverArtRole { get; set; }
			public string commentRole { get; set; }
			public string podcastRole { get; set; }
			public string streamRole { get; set; }
			public string jukeboxRole { get; set; }
			public string shareRole { get; set; }

			public override string ToString()
			{
				return string.Format(@"Name={0} email={1} scrobblingEnabled={2} adminRole={3} settingsRole={4} downloadRole={5} uploadRole={6} playlistRole={7} coverArtRole={8} commentRole={9} podcastRole={10} streamRole={11} jukeboxRole={12} shareRole={13}", Name,email,scrobblingEnabled,adminRole, settingsRole, downloadRole, uploadRole, playlistRole, coverArtRole, commentRole, podcastRole, streamRole, jukeboxRole, shareRole);  
			}
		}
		public class License
		{
			public string IsValid{get;set;}
			public string Email{get;set;}
			public string Key{get;set;}
			public string Date{get;set;}
			public override string ToString()
			{
				return string.Format(@"valid:{0},email:{1},key:{2},date:{3}",IsValid,Email,Key,Date);
			}
		}
		public class Lyrics
		{
			public string artist{get;set;}
			public string title{get;set;}
			public string Text{get;set;}

			public override string ToString()
			{
				return string.Format(@"artist:{0},title{1},text:{2}",artist,title,Text);
			}
		}
		public class Share
		{
			public string Id{get;set;}
			public string Url{get;set;}
			public string Description{get;set;}
			public string Username{get;set;}
			public string Created{get;set;}
			public string LastVisited{get;set;}
			public string Expires{get;set;}
			public string VisitCount{get;set;}
			private List<Song> _Songs;
		
			public List<Song> SharedSongs
			{
				get{return _Songs;}
				set{_Songs = value;}
			} 
			public Share()
			{
				_Songs = new List<Song>();
			}
			public override string ToString()
			{
				string Als="";
				foreach(Song s in _Songs)
				{
					Als += s.ToString();
 				}

				return string.Format(@"id:{0},url:{1},description:{2},username:{3},created:{4},lastVisited:{5},Expires:{6},VisitCount:{7},song:{8}", Id, Url, Description, Username, Created, LastVisited, Expires, VisitCount, Als);
			}
		}
		public class Channel
		{	
			public string Id{get;set;}
			public string Url{get;set;}
			public string Title{get;set;}
			public string Description{get;set;}
			public string Status{get;set;}
			private List<Episode> _Episodes = new List<Episode>();

			public List<Episode> Episodes
			{
				get{return _Episodes;}
				set{_Episodes = value;}
			}

			public override string ToString()
			{
				string Ale ="";
				foreach(Episode e in _Episodes)
				{
					Ale += e.ToString();
				}
				return string.Format(@"
				id:{0},
				url:{1},
				title:{2},
				description:{3},
				status:{4},
				episodes:{5}",Id,Url,Title,Description,Status,Ale);
			}
		}
		public class Episode
		{
			public string Id{get;set;}
			public string StreamId{get;set;}
			public string Title{get;set;}
			public string Description{get;set;}
			public string PublishDate{get;set;}
			public string Status{get;set;}
			public string Parent{get;set;}
			public string Year{get;set;}
			public string Genre{get;set;}
			public string CoverArt{get;set;}
			public string Size{get;set;}
			public string ContentType{get;set;}
			public string Suffix{get;set;}
			public string Duration{get;set;}
			public string BitRate{get;set;}

			public override string ToString()
			{
				return string.Format(@"
				id:{0},
				streamid:{1},
				title:{2},
				description:{3},
				publishDate:{4},
				status:{5},
				parent:{6},
				year:{7},
				genre:{8},
				coverart:{9},
				size:{10},
				contentType:{11},
				Suffix:{12},
				duration:{13},
				bitrate:{14}", Id, StreamId, Title, Description, PublishDate, Status, Parent, Year, Genre, CoverArt, Size, ContentType, Suffix, Duration, BitRate);
			}

		}
		public class JukeBoxStatut
		{
			public string CurrentIndex { get; set; }		
			public string Playing { get; set; }
			public string gain { get; set; }
			public string position{get;set;}
			private List<Song> _Songs;
			public List<Song> Entry
			{
				get{return _Songs;}
				set{_Songs = value;}

			}
			public JukeBoxStatut()
			{
				_Songs = new List<Song>();
			}
			public override string ToString()
			{
					string als="";

					foreach(Song s in _Songs)
					{
						als += s.ToString();			
					}

				return string.Format(@"currentIndex={0} playing={1} gain={2} position={3} song={4}",CurrentIndex,Playing,gain,position,als);
			}
		}
		public class ChatMessage
		{
			public string Username{get;set;}
			public string time{get;set;}
			public string message{get;set;}
			public override string ToString()
			{
				return string.Format(@"username:{0},time:{1},message:{2}",Username,time,message);
			}
		}
		#endregion Classes

		#region Exception
		public class GenericException : Exception
		{
		 
			public GenericException()
			{			
			}

			public GenericException(string message)
				: base(message)
			{
			
			}

			public GenericException(string message, Exception inner)
				: base(message, inner)
			{
			}
		}
		public class MissingParameterException : Exception
		{

			public MissingParameterException()
			{
			}

			public MissingParameterException(string message)
				: base(message)
			{

			}

			public MissingParameterException(string message, Exception inner)
				: base(message, inner)
			{
			}
		}
		public class ClientProtocolVersionException : Exception
		{

			public ClientProtocolVersionException()
			{
			}

			public ClientProtocolVersionException(string message)
				: base(message)
			{

			}

			public ClientProtocolVersionException(string message, Exception inner)
				: base(message, inner)
			{
			}
		}
		public class ServerProtocolVersionException : Exception
		{

			public ServerProtocolVersionException()
			{
			}

			public ServerProtocolVersionException(string message)
				: base(message)
			{

			}

			public ServerProtocolVersionException(string message, Exception inner)
				: base(message, inner)
			{
			}
		}
		public class LogInException : Exception
		{

			public LogInException()
			{
			}

			public LogInException(string message)
				: base(message)
			{

			}

			public LogInException(string message, Exception inner)
				: base(message, inner)
			{
			}
		}
		public class UserPermissionException : Exception
		{

			public UserPermissionException()
			{
			}

			public UserPermissionException(string message)
				: base(message)
			{

			}

			public UserPermissionException(string message, Exception inner)
				: base(message, inner)
			{
			}
		}
		public class TrialPeriodException : Exception
		{

			public TrialPeriodException()
			{
			}

			public TrialPeriodException(string message)
				: base(message)
			{

			}

			public TrialPeriodException(string message, Exception inner)
				: base(message, inner)
			{
			}
		}
		public class MissingRequestedDatadException : Exception
		{

			public MissingRequestedDatadException()
			{
			}

			public MissingRequestedDatadException(string message)
				: base(message)
			{

			}

			public MissingRequestedDatadException(string message, Exception inner)
				: base(message, inner)
			{
			}
		}
		#endregion

		#region Enum
			public enum List_type
			{
				random, newest, highest, frequent, recent, alphabeticalByName, alphabeticalByArtist, starred
			}
			public enum action_type
			{
				get,status,set,start,stop,skip,add,clear,remove,shuffle,setGain
			}
		#endregion
	#endregion

    public class Subsonic
    {
		#region field
			
			public  string appName="Subm8"; // Subsonic app name

			// Version of the REST API implemented
			private string apiVersion = "1.8.0";



			private string responseHead = @"<subsonic-response xmlns=""http://subsonic.org/restapi"" status=""ok"" version=""1.8.0"">"; //basic Xml header response to api query
			private string responseHeadCustom = @"<subsonic-response xmlns=""http://subsonic.org/restapi"" status=""{0}"" version=""1.8.0"">"; //custom to handle error
			private string responseFoot = "</subsonic-response>";	//last element

			private string server; 	// Server url defined by user
			private AuthenticationHeaderValue authHeader; //http header (for basic http auth)
			private HttpClient httpClient;	
			private HttpClientHandler hcHandler;	//to handle httpclient proprietes such as header
		
		#endregion

		#region System Method
			/// <summary>
			/// Takes parameters for server, username and password to generate an auth header
			/// and Pings the server
			/// </summary>
			/// <param name="theServer"></param>
			/// <param name="user"></param>
			/// <param name="password"></param>
			/// <returns>true if logged</returns>
			public async Task<bool> LogIn(string theServer, string user, string password)
			{
				
				
				server = theServer;
				authHeader = CreateBasicAuthenticationHeader(user, password);

				hcHandler = new HttpClientHandler();
				httpClient = new HttpClient(hcHandler);
				httpClient.DefaultRequestHeaders.Authorization = authHeader;



				Stream theStream = await this.MakeGenericRequest("ping", null);				


				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();
								

				return this.ErrorHandler(result);
			}
			/// <summary>
			/// Convert a string to Hex value
			/// </summary>
			/// <param name="text"></param>
			/// <returns>Hex value</returns>
			private string ConvertStringToHex(string text)
			{
        
				char[] chars = text.ToCharArray();
				StringBuilder stringBuilder = new StringBuilder();
				foreach (char c in chars)
				{
					stringBuilder.Append(((Int16)c).ToString("x"));
				}
				String textAsHex = stringBuilder.ToString();
				return textAsHex;
			}
			/// <summary>
			/// Handle Subsonic Api Error by throwing the corresponding exception
			/// </summary>
			/// <param name="RequestResult">Xml result of a random api request</param>
			/// <returns>true if no error</returns>
			private bool ErrorHandler(string RequestResult)
			{
				bool _return = false;
				XElement Xel = XElement.Parse(RequestResult);

				string statut = (string)Xel.Attribute("status");
				if (statut != "ok")
				{
					RequestResult = RequestResult.Replace(string.Format(responseHeadCustom, statut), "").Replace(responseFoot, "");
					XElement Xel2 = XElement.Parse(RequestResult);
					string ErrorCode = (string)Xel2.Attribute("code");
					string ErrorMessage = (string)Xel2.Attribute("message");
					if (ErrorCode == "0")
					{
						throw new GenericException("A generic error: " + ErrorMessage);
					}
					if (ErrorCode == "10")
					{
						throw new MissingParameterException("Required parameter is missing: " + ErrorMessage);
					}
					if (ErrorCode == "20")
					{
						throw new ClientProtocolVersionException("Incompatible Subsonic REST protocol version. Client must upgrade: " + ErrorMessage);
					}
					if (ErrorCode == "30")
					{
						throw new ServerProtocolVersionException("Incompatible Subsonic REST protocol version. Server must upgrade: " + ErrorMessage);
					}
					if (ErrorCode == "40")
					{
						throw new LogInException("Wrong username or password: " + ErrorMessage);
					}
					if (ErrorCode == "50")
					{
						throw new UserPermissionException("User is not authorized for the given operation: " + ErrorMessage);
					}
					if (ErrorCode == "60")
					{
						throw new TrialPeriodException("The trial period for the Subsonic server is over. Please donate to get a license key. Visit subsonic.org for details: " + ErrorMessage);
					}
					if (ErrorCode == "70")
					{
						throw new MissingRequestedDatadException("The requested data was not found: " + ErrorMessage);
					}
				}
				else
				{
					_return = true;
				}

				return _return;
			}
			/// <summary>
			/// Uses the Auth Header for logged in user to make an HTTP request to the server 
			/// with the given Subsonic API method and parameters
			/// </summary>
			/// <param name="method"></param>
			/// <param name="parameters"></param>
			/// <returns>Datastream of the server response</returns>
			public async Task<Stream> MakeGenericRequest(string method, Dictionary<string, string> parameters)
			{
				// Check to see if Logged In yet
				if (httpClient == null)
				{
					// Throw a Not Logged In exception
					Exception e = new Exception("No Authorization header.  Must Log In first");
					return null;
				}
				else
				{
					if (!method.EndsWith(".view"))
						method += ".view";

					string requestURL = BuildRequestURL(method, parameters);

					Stream webStream = await httpClient.GetStreamAsync(requestURL);					

					return webStream;
				}
			}
		   /// <summary>
		   /// build the Auth Header
		   /// </summary>
		   /// <param name="username"></param>
		   /// <param name="password"></param>
			/// <returns>the Basic Authorization Header</returns>
			private AuthenticationHeaderValue CreateBasicAuthenticationHeader(string username, string password)
			{
				return new AuthenticationHeaderValue(
					"Basic",
					System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(
					string.Format("{0}:{1}", username, password)))
				);
			}
			/// <summary>
			/// Creates a URL for a request but does not make the actual request using set login credentials an dmethod and parameters
			/// </summary>
			/// <param name="method"></param>
			/// <param name="parameters"></param>
			/// <returns>Proper Subsonic API URL for a request</returns>
			private string BuildRequestURL(string method, Dictionary<string, string> parameters)
			{
				string requestURL =  server + "/rest/" + method + "?v=" + apiVersion + "&c=" + appName; // + "&u=" + User + "&p=" + Password;
				if (parameters != null)
				{
					foreach (KeyValuePair<string, string> parameter in parameters)
					{
						requestURL += "&" + parameter.Key + "=" + parameter.Value;
					}
				}
				return requestURL;
			}			
			/// <summary>
			/// Get details about the software license. Takes no extra parameters. 
			/// Please note that access to the REST API requires that the server has a valid license (after a 30-day trial period). 
			/// To get a license key you can give a donation to the Subsonic project.
			/// </summary>
			/// <returns>A licence Object</returns>
			public async Task<License> getLicense()
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				Stream theStream = await this.MakeGenericRequest("getLicense", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();
				License li = new License();
				if(this.ErrorHandler(result))
				{
					result = result.Replace(responseHead, "").Replace(responseFoot, "");

					XElement Xel = XElement.Parse(result);	
					
					li.IsValid = (string)Xel.Attribute("license");
					li.Email = (string)Xel.Attribute("email");
					li.Key = (string)Xel.Attribute("key");
					li.Date = (string)Xel.Attribute("date");	
				}

				return li;
			}

		#endregion

		#region Browsing Method
			/// <summary>
			/// Returns all configured top-level music folders
			/// </summary>
			/// <returns>A list of music folder</returns>
			public async Task<List<MusicFolder>> getMusicFolders()
			{			

				Dictionary<string, string> parameters = new Dictionary<string, string>();

				Stream theStream = await this.MakeGenericRequest("getMusicFolders", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();
				result = result.Replace(responseHead, "").Replace(responseFoot, "");
				XElement musicFolders = XElement.Parse(result);

				List<MusicFolder> musicFolderList = (from mf in musicFolders.Elements("musicFolder")
													select new MusicFolder
													{
														id =(string)mf.Attribute("id"),
														Name = (string)mf.Attribute("name")
													}).ToList();

				return musicFolderList;
			}

			/// <summary>
			/// Returns an indexed structure of all artists
			/// </summary>
			/// <param name="musicFolderId" required="no">If specified, only return artists in the music folder with the given ID. See getMusicFolders</param>
			/// <param name="ifModifiedSince" required="no">If specified, only return a result if the artist collection has changed since the given time (in milliseconds since 1 Jan 1970).</param>
			/// <returns></returns>
			public async Task<MusicFolder> getIndexes(string musicFolderId = null, string ifModifiedSince = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();
				 if (!string.IsNullOrEmpty(musicFolderId))
					parameters.Add("musicFolderId", musicFolderId);

				 if (!string.IsNullOrEmpty(ifModifiedSince))
					parameters.Add("ifModifiedSince", ifModifiedSince);

				Stream theStream = await this.MakeGenericRequest("getIndexes", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				result = result.Replace(responseHead, "").Replace(responseFoot, "");

				XElement indexes = XElement.Parse(result);

				MusicFolder Indexes = new MusicFolder("Root","-1");

				Indexes.Folders.AddRange((from sh in indexes.Elements("shortcut")
												select new MusicFolder 
												{
													id = (string)sh.Attribute("id"),
													Name = (string)sh.Attribute("name"),
													ItemType = SubsonicItem.SubsonicItemType.Folder 

												}).ToList());

				IEnumerable<XElement> ind = indexes.Elements("index");

				Indexes.Folders.AddRange((from art in ind.Elements("artist")
											select new MusicFolder 
											{
												id = (string)art.Attribute("id"),
												Name = (string)art.Attribute("name"),
												ItemType = SubsonicItem.SubsonicItemType.Folder 
											}).ToList());



				Indexes.Songs.AddRange((from nm in indexes.Elements("child")
									select new Song
									{
										Id = (string)nm.Attribute("id"),
										Name = (string)nm.Attribute("title"),
										Album = (string)nm.Attribute("album"),
										Artist = (string)nm.Attribute("artist"),
										Parent = (string)nm.Attribute("parent"),
										CoverArt = (string)nm.Attribute("coverArt"),
										Created = (string)nm.Attribute("created"),
										Duration = (string)nm.Attribute("duration"),
										BitRate = (string)nm.Attribute("bitRate"),
										Track = (string)nm.Attribute("track"),
										Year = (string)nm.Attribute("year"),
										Genre = (string)nm.Attribute("genre"),
										Size = (string)nm.Attribute("size"),
										Suffix = (string)nm.Attribute("suffix"),
										ContentType = (string)nm.Attribute("contentType")
										
									}).ToList());

				return Indexes;
			}

			/// <summary>
			/// Returns a listing of all files in a music directory. Typically used to get list of albums for an artist, or list of songs for an album.
			/// </summary>
			/// <param name="id" required="yes">A string which uniquely identifies the music folder. Obtained by calls to getIndexes or getMusicDirectory.</param>
			/// <returns>A music folder</returns>
			public async Task<MusicFolder> getMusicDirectory(string id)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();
		
				parameters.Add("id", id);			

				Stream theStream = await this.MakeGenericRequest("getMusicDirectory", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				result = result.Replace(responseHead, "").Replace(responseFoot, "");

				XElement XDirectory = XElement.Parse(result);

				MusicFolder Directory = new MusicFolder((string)XDirectory.Attribute("name"),(string)XDirectory.Attribute("id"));

				Directory.Folders.AddRange((from dir in XDirectory.Elements("child")
							where dir.Attribute("isDir").Value == "true"
							select new MusicFolder{
								id = (string)dir.Attribute("id"),
								Name = (string)dir.Attribute("name")
							}).ToList());

				Directory.Songs.AddRange((from nm in XDirectory.Elements("child")
										  where nm.Attribute("isDir").Value == "false"
											select new Song 
											{

												Id = (string)nm.Attribute("id"),
												Name = (string)nm.Attribute("title"),
												Album = (string)nm.Attribute("album"),
												Artist = (string)nm.Attribute("artist"),
												Parent = (string)nm.Attribute("parent"),
												CoverArt = (string)nm.Attribute("coverArt"),
												Created = (string)nm.Attribute("created"),
												Duration = (string)nm.Attribute("duration"),
												BitRate = (string)nm.Attribute("bitRate"),
												Track = (string)nm.Attribute("track"),
												Year = (string)nm.Attribute("year"),
												Genre = (string)nm.Attribute("genre"),
												Size = (string)nm.Attribute("size"),
												Suffix = (string)nm.Attribute("suffix"),
												ContentType = (string)nm.Attribute("contentType"),
												AlbumId = (string)nm.Attribute("albumId"),
												ArtistId = (string)nm.Attribute("artistId")

											}).ToList());

				return Directory;
			}

			/// <summary>
			/// Similar to getIndexes, but organizes music according to ID3 tags
			/// </summary>
			/// <returns>An artists List</returns>
			/// 
			public async Task<List<Artist>> getArtists()
			{

				Dictionary<string, string> parameters = new Dictionary<string, string>();
	

				Stream theStream = await this.MakeGenericRequest("getArtists", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				result = result.Replace(responseHead,"").Replace(responseFoot,"");
		
		
				XElement artists = XElement.Parse(result);
				IEnumerable<XElement> idn = artists.Elements("index");
				List<Artist> ListArtist = (from nm in idn.Elements("artist")
										   select new Artist
										   {
												Id = (string)nm.Attribute("id"),
												Name = (string)nm.Attribute("name"),
												CoverArt = (string)nm.Attribute("coverArt"),
												AlbumCount = (string)nm.Attribute("albumCount")
											}).ToList();			   
				
				return ListArtist;
			}

			/// <summary>
			/// Returns details for an artist, including a list of albums. This method organizes music according to ID3 tags.
			/// </summary>
			/// <param name="id" required="yes">The artist ID</param>
			/// <returns>An artist</returns>
			public async Task<Artist> getArtist(string id)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				parameters.Add("id", id);

				Stream theStream = await this.MakeGenericRequest("getArtist", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				result = result.Replace(responseHead,"").Replace(responseFoot,"");

				XElement a = XElement.Parse(result);
				Artist art = new Artist();
				art.Id = (string)a.Attribute("id");
				art.Name = (string)a.Attribute("name");
				art.CoverArt = (string)a.Attribute("coverArt");
				art.AlbumCount = (string)a.Attribute("albumCount");
				art.AlbumList = (from nm in a.Elements("album")
											select new Album   
											{
												id = (string)nm.Attribute("id"),
												Name = (string)nm.Attribute("name"),
												CoverArt = (string)nm.Attribute("covertArt"),
												SongCount = (string)nm.Attribute("songCount"),
												Created = (string)nm.Attribute("created"),
												Duration = (string)nm.Attribute("duration"),
												Artist = (string)nm.Attribute("artist"),
												ArtistId = (string)nm.Attribute("artistId")
											}).ToList();				
						


				return art;	
			}

			/// <summary>
			/// Returns details for an album, including a list of songs. This method organizes music according to ID3 tags
			/// </summary>
			/// <param name="id" required="yes">The album ID.</param>
			/// <returns>An album</returns>
			public async Task<Album> getAlbum(string id)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				parameters.Add("id", id);

				Stream theStream = await this.MakeGenericRequest("getAlbum", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();
				result = result.Replace(responseHead, "").Replace(responseFoot, "");

				XElement Xel = XElement.Parse(result);

				Album alb = new Album();

				alb.id = (string)Xel.Attribute("id");
				alb.Name = (string)Xel.Attribute("name");
				alb.CoverArt = (string)Xel.Attribute("coverArt");
				alb.SongCount = (string)Xel.Attribute("songCount");
				alb.Created = (string)Xel.Attribute("created");
				alb.Duration = (string)Xel.Attribute("duration");
				alb.Artist = (string)Xel.Attribute("artist");
				alb.ArtistId = (string)Xel.Attribute("artistId");
				alb.SongList = (from nm in Xel.Elements("song")
								select new Song
								{
									Id =(string)nm.Attribute("id"),
									Name =(string)nm.Attribute("title"),
									Album = (string)nm.Attribute("album"),
									Artist = (string) nm.Attribute("artist"),
									Parent =(string)nm.Attribute("parent"),								
									CoverArt =(string)nm.Attribute("coverArt"),
									Created =(string)nm.Attribute("created"),
									Duration =(string)nm.Attribute("duration"),
									BitRate =(string)nm.Attribute("bitRate"),
									Track =(string)nm.Attribute("track"),
									Year =(string)nm.Attribute("year"),
									Genre =(string)nm.Attribute("genre"),
									Size =(string)nm.Attribute("size"),
									Suffix =(string)nm.Attribute("suffix"),
									ContentType =(string)nm.Attribute("contentType"),
									AlbumId =(string)nm.Attribute("albumId"),
									ArtistId =(string)nm.Attribute("artistId")
								}).ToList();
							
				return alb;			
			}

			/// <summary>
			/// Returns details for a song.
			/// </summary>
			/// <param name="id" required="yes">The song ID.</param>
			/// <returns>a song</returns>
			public async Task<Song> getSong(string id)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				parameters.Add("id", id);

				Stream theStream = await this.MakeGenericRequest("getSong", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();
				result = result.Replace(responseHead, "").Replace(responseFoot, "");

				XElement nm = XElement.Parse(result);

				Song sng =new Song();

				sng.Id = (string)nm.Attribute("id");
				sng.Name = (string)nm.Attribute("title");
				sng.Album = (string)nm.Attribute("album");
				sng.Artist = (string)nm.Attribute("artist");
				sng.Parent = (string)nm.Attribute("parent");
				sng.CoverArt = (string)nm.Attribute("coverArt");
				sng.Created = (string)nm.Attribute("created");
				sng.Duration = (string)nm.Attribute("duration");
				sng.BitRate = (string)nm.Attribute("bitRate");
				sng.Track = (string)nm.Attribute("track");
				sng.Year = (string)nm.Attribute("year");
				sng.Genre = (string)nm.Attribute("genre");
				sng.Size = (string)nm.Attribute("size");
				sng.Suffix = (string)nm.Attribute("suffix");
				sng.ContentType = (string)nm.Attribute("contentType");
				sng.AlbumId = (string)nm.Attribute("albumId");
				sng.ArtistId = (string)nm.Attribute("artistId");

				return sng;
			}

			/// <summary>
			/// Not Implemented
			/// </summary>
			/// <returns></returns>			
			public async Task<string> getVideos()
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				Stream theStream = await this.MakeGenericRequest("getVideos", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				return result;
			}
		#endregion

		#region Album/Song Lists Method
			public async Task<List<Album>> getAlbumList(List_type enumlist_type, string size = null, string offset = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();
				parameters.Add("type", enumlist_type.ToString());

				if (!string.IsNullOrEmpty(size))
					parameters.Add("size", size);

				if (!string.IsNullOrEmpty(offset))
					parameters.Add("offset", offset);

				Stream theStream = await this.MakeGenericRequest("getAlbumList", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				result = result.Replace(responseHead, "").Replace(responseFoot, "");

				XElement Xel = XElement.Parse(result);

				List<Album>  AlbumList = (from nm in Xel.Elements("album")
											select new Album {
												id = (string)nm.Attribute("id"),
												Name = (string)nm.Attribute("name"),
												CoverArt = (string)nm.Attribute("coverArt"),
												SongCount = (string)nm.Attribute("songCount"),
												Created = (string)nm.Attribute("created"),
												Duration = (string)nm.Attribute("duration"),
												Artist = (string)nm.Attribute("artist"),
												ArtistId = (string)nm.Attribute("artistId"),
												starred = (string)nm.Attribute("starred"),
												userRating = (string)nm.Attribute("userRating"),
												averageRating = (string)nm.Attribute("averageRating")
											}).ToList();

				return AlbumList;
			}
			public async Task<List<Album>> getAlbumList2(List_type enumlist_type, string size = null, string offset = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();
				parameters.Add("type", enumlist_type.ToString());

				if (!string.IsNullOrEmpty(size))
					parameters.Add("size", size);

				if (!string.IsNullOrEmpty(offset))
					parameters.Add("offset", offset);

				Stream theStream = await this.MakeGenericRequest("getAlbumList2", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				result = result.Replace(responseHead, "").Replace(responseFoot, "");

				XElement Xel = XElement.Parse(result);

				List<Album> AlbumList = (from nm in Xel.Elements("album")
										 select new Album
										 {
											 id = (string)nm.Attribute("id"),
											 Name = (string)nm.Attribute("name"),
											 CoverArt = (string)nm.Attribute("coverArt"),
											 SongCount = (string)nm.Attribute("songCount"),
											 Created = (string)nm.Attribute("created"),
											 Duration = (string)nm.Attribute("duration"),
											 Artist = (string)nm.Attribute("artist"),
											 ArtistId = (string)nm.Attribute("artistId"),
											 starred = (string)nm.Attribute("starred"),
											 userRating = (string)nm.Attribute("userRating"),
											 averageRating = (string)nm.Attribute("averageRating")
										 }).ToList();

				return AlbumList;
			}
			public async Task<List<Song>> getRandomSongs(string size = null, string genre = null, string fromYear = null, string toYear = null, string musicFolderId = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();			
			
				if (!string.IsNullOrEmpty(size))
					parameters.Add("size", size);
				if (!string.IsNullOrEmpty(genre))
					parameters.Add("genre", genre);
				if (!string.IsNullOrEmpty(fromYear))
					parameters.Add("fromYear", fromYear);
				if (!string.IsNullOrEmpty(toYear))
					parameters.Add("toYear", toYear);
				if (!string.IsNullOrEmpty(musicFolderId))
					parameters.Add("musicFolderId", musicFolderId);

				Stream theStream = await this.MakeGenericRequest("getRandomSongs", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();
				result = result.Replace(responseHead, "").Replace(responseFoot, "");

				XElement Xel = XElement.Parse(result);
				List<Song> RandomSonglist = (from nm in Xel.Elements("song")
											 select new Song
											 {
												 Id = (string)nm.Attribute("id"),
												 Name = (string)nm.Attribute("title"),
												 Album = (string)nm.Attribute("album"),
												 Artist = (string)nm.Attribute("artist"),
												 Parent = (string)nm.Attribute("parent"),
												 CoverArt = (string)nm.Attribute("coverArt"),
												 Created = (string)nm.Attribute("created"),
												 Duration = (string)nm.Attribute("duration"),
												 BitRate = (string)nm.Attribute("bitRate"),
												 Track = (string)nm.Attribute("track"),
												 Year = (string)nm.Attribute("year"),
												 Genre = (string)nm.Attribute("genre"),
												 Size = (string)nm.Attribute("size"),
												 Suffix = (string)nm.Attribute("suffix"),
												 ContentType = (string)nm.Attribute("contentType"),
												 AlbumId = (string)nm.Attribute("albumId"),
												 ArtistId = (string)nm.Attribute("artistId")
											 }).ToList();

				return RandomSonglist;
			}
			public async Task<List<Song>> getNowPlaying()
			{			
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				Stream theStream = await this.MakeGenericRequest("getNowPlaying", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();
				result = result.Replace(responseHead, "").Replace(responseFoot, "");

				XElement Xel = XElement.Parse(result);
				List<Song> NowPlayinglist = (from nm in Xel.Elements("entry")
											 select new Song
											 {
												 Id = (string)nm.Attribute("id"),
												 Name = (string)nm.Attribute("title"),
												 Album = (string)nm.Attribute("album"),
												 Artist = (string)nm.Attribute("artist"),
												 Parent = (string)nm.Attribute("parent"),
												 CoverArt = (string)nm.Attribute("coverArt"),
												 Created = (string)nm.Attribute("created"),
												 Duration = (string)nm.Attribute("duration"),
												 BitRate = (string)nm.Attribute("bitRate"),
												 Track = (string)nm.Attribute("track"),
												 Year = (string)nm.Attribute("year"),
												 Genre = (string)nm.Attribute("genre"),
												 Size = (string)nm.Attribute("size"),
												 Suffix = (string)nm.Attribute("suffix"),
												 ContentType = (string)nm.Attribute("contentType"),
												 AlbumId = (string)nm.Attribute("albumId"),
												 ArtistId = (string)nm.Attribute("artistId"),
												 Username = (string)nm.Attribute("username"),
												 PlayerId = (string)nm.Attribute("playerId"),
												 PlayerName = (string)nm.Attribute("playerName"),
												 MinutesAgo = (string)nm.Attribute("minutesAgo")
											 }).ToList();
				return NowPlayinglist;
			}
			public async Task<MusicFolder> getStarred()
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				Stream theStream = await this.MakeGenericRequest("getStarred", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();
				result = result.Replace(responseHead, "").Replace(responseFoot, "");

				XElement Xel = XElement.Parse(result);
				MusicFolder starred = new MusicFolder();
				starred.Artists.AddRange((from nm in Xel.Elements("artist")
											select new Artist{
												Id = (string)nm.Attribute("id"),
												Name = (string)nm.Attribute("name")
											}).ToList());
				starred.Albums.AddRange((from nm in Xel.Elements("album")
										  select new Album
										  {
											  id = (string)nm.Attribute("id"),
											  Name = (string)nm.Attribute("name"),
											  CoverArt = (string)nm.Attribute("coverArt"),
											  SongCount = (string)nm.Attribute("songCount"),
											  Created = (string)nm.Attribute("created"),
											  Duration = (string)nm.Attribute("duration"),
											  Artist = (string)nm.Attribute("artist"),
											  ArtistId = (string)nm.Attribute("artistId"),
											  starred = (string)nm.Attribute("starred")
										  }).ToList());
				starred.Songs.AddRange((from nm in Xel.Elements("song")
										 select new Song
										 {
											Id = (string)nm.Attribute("id"),
											Name = (string)nm.Attribute("title"),
											Album = (string)nm.Attribute("album"),
											Artist = (string)nm.Attribute("artist"),
											Parent = (string)nm.Attribute("parent"),
											CoverArt = (string)nm.Attribute("coverArt"),
											Created = (string)nm.Attribute("created"),
											Duration = (string)nm.Attribute("duration"),
											BitRate = (string)nm.Attribute("bitRate"),
											Track = (string)nm.Attribute("track"),
											Year = (string)nm.Attribute("year"),
											Genre = (string)nm.Attribute("genre"),
											Size = (string)nm.Attribute("size"),
											Suffix = (string)nm.Attribute("suffix"),
											ContentType = (string)nm.Attribute("contentType"),
											AlbumId = (string)nm.Attribute("albumId"),
											ArtistId = (string)nm.Attribute("artistId"),
											Starred = (string)nm.Attribute("starred")
										 }).ToList());

				return starred;
			}
			public async Task<MusicFolder> getStarred2()
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				Stream theStream = await this.MakeGenericRequest("getStarred2", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				result = result.Replace(responseHead, "").Replace(responseFoot, "");

				XElement Xel = XElement.Parse(result);
				MusicFolder starred = new MusicFolder();
				starred.Artists.AddRange((from nm in Xel.Elements("artist")
										  select new Artist
										  {
											  Id = (string)nm.Attribute("id"),
											  Name = (string)nm.Attribute("name")
										  }).ToList());
				starred.Albums.AddRange((from nm in Xel.Elements("album")
										 select new Album
										 {
											 id = (string)nm.Attribute("id"),
											 Name = (string)nm.Attribute("name"),
											 CoverArt = (string)nm.Attribute("coverArt"),
											 SongCount = (string)nm.Attribute("songCount"),
											 Created = (string)nm.Attribute("created"),
											 Duration = (string)nm.Attribute("duration"),
											 Artist = (string)nm.Attribute("artist"),
											 ArtistId = (string)nm.Attribute("artistId"),
											 starred = (string)nm.Attribute("starred")
										 }).ToList());
				starred.Songs.AddRange((from nm in Xel.Elements("song")
										select new Song
										{
											Id = (string)nm.Attribute("id"),
											Name = (string)nm.Attribute("title"),
											Album = (string)nm.Attribute("album"),
											Artist = (string)nm.Attribute("artist"),
											Parent = (string)nm.Attribute("parent"),
											CoverArt = (string)nm.Attribute("coverArt"),
											Created = (string)nm.Attribute("created"),
											Duration = (string)nm.Attribute("duration"),
											BitRate = (string)nm.Attribute("bitRate"),
											Track = (string)nm.Attribute("track"),
											Year = (string)nm.Attribute("year"),
											Genre = (string)nm.Attribute("genre"),
											Size = (string)nm.Attribute("size"),
											Suffix = (string)nm.Attribute("suffix"),
											ContentType = (string)nm.Attribute("contentType"),
											AlbumId = (string)nm.Attribute("albumId"),
											ArtistId = (string)nm.Attribute("artistId"),
											Starred = (string)nm.Attribute("starred")
										}).ToList());

				return starred;
			}
		#endregion

		#region Searching Method
			public async Task<string> search(string artist = null, string album = null, string title = null, string any = null, string count = null, string offset = null, string newerThan = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				if (!string.IsNullOrEmpty(artist))
					parameters.Add("artist", artist);
				if (!string.IsNullOrEmpty(album))
					parameters.Add("album", album);
				if (!string.IsNullOrEmpty(title))
					parameters.Add("title", title);
				if (!string.IsNullOrEmpty(count))
					parameters.Add("count", count);
				if (!string.IsNullOrEmpty(offset))
					parameters.Add("offset", offset);
				if (!string.IsNullOrEmpty(newerThan))
					parameters.Add("newerThan", newerThan);

				Stream theStream = await this.MakeGenericRequest("search", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				return result;
			}
			public async Task<MusicFolder> search2(string query, string artistCount = null, string artistOffset = null, string albumCount = null, string albumOffset = null, string songCount = null, string songOffset = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();
				parameters.Add("query", query);
				if (!string.IsNullOrEmpty(artistCount))
					parameters.Add("artistCount", artistCount);
				if (!string.IsNullOrEmpty(artistOffset))
					parameters.Add("artistOffset", artistOffset);
				if (!string.IsNullOrEmpty(albumCount))
					parameters.Add("albumCount", albumCount);
				if (!string.IsNullOrEmpty(albumOffset))
					parameters.Add("albumOffset", albumOffset);
				if (!string.IsNullOrEmpty(songCount))
					parameters.Add("songCount", songCount);
				if (!string.IsNullOrEmpty(songOffset))
					parameters.Add("songOffset", songOffset);

				Stream theStream = await this.MakeGenericRequest("search2", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				result = result.Replace(responseHead, "").Replace(responseFoot, "");

				XElement Xel = XElement.Parse(result);
				MusicFolder SearchResult = new MusicFolder();
				SearchResult.Artists.AddRange((from nm in Xel.Elements("artist")
										  select new Artist
										  {
											  Id = (string)nm.Attribute("id"),
											  Name = (string)nm.Attribute("name")
										  }).ToList());
				SearchResult.Albums.AddRange((from nm in Xel.Elements("album")
										 select new Album
										 {
											 id = (string)nm.Attribute("id"),
											 Name = (string)nm.Attribute("name"),
											 CoverArt = (string)nm.Attribute("coverArt"),
											 SongCount = (string)nm.Attribute("songCount"),
											 Created = (string)nm.Attribute("created"),
											 Duration = (string)nm.Attribute("duration"),
											 Artist = (string)nm.Attribute("artist")
										 }).ToList());
				SearchResult.Songs.AddRange((from nm in Xel.Elements("song")
										select new Song
										{
											Id = (string)nm.Attribute("id"),
											Name = (string)nm.Attribute("title"),
											Album = (string)nm.Attribute("album"),
											Artist = (string)nm.Attribute("artist"),
											Parent = (string)nm.Attribute("parent"),
											CoverArt = (string)nm.Attribute("coverArt"),
											Created = (string)nm.Attribute("created"),
											Duration = (string)nm.Attribute("duration"),
											BitRate = (string)nm.Attribute("bitRate"),
											Track = (string)nm.Attribute("track"),
											Year = (string)nm.Attribute("year"),
											Genre = (string)nm.Attribute("genre"),
											Size = (string)nm.Attribute("size"),
											Suffix = (string)nm.Attribute("suffix"),
											ContentType = (string)nm.Attribute("contentType"),
											AlbumId = (string)nm.Attribute("albumId"),
											ArtistId = (string)nm.Attribute("artistId")
										}).ToList());

				return SearchResult;
			}
			public async Task<MusicFolder> search3(string query, string artistCount = null, string artistOffset = null, string albumCount = null, string albumOffset = null, string songCount = null, string songOffset = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();
				parameters.Add("query", query);
				if (!string.IsNullOrEmpty(artistCount))
					parameters.Add("artistCount", artistCount);
				if (!string.IsNullOrEmpty(artistOffset))
					parameters.Add("artistOffset", artistOffset);
				if (!string.IsNullOrEmpty(albumCount))
					parameters.Add("albumCount", albumCount);
				if (!string.IsNullOrEmpty(albumOffset))
					parameters.Add("albumOffset", albumOffset);
				if (!string.IsNullOrEmpty(songCount))
					parameters.Add("songCount", songCount);
				if (!string.IsNullOrEmpty(songOffset))
					parameters.Add("songOffset", songOffset);

				Stream theStream = await this.MakeGenericRequest("search3", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				result = result.Replace(responseHead, "").Replace(responseFoot, "");

				XElement Xel = XElement.Parse(result);
				MusicFolder SearchResult = new MusicFolder();
				SearchResult.Artists.AddRange((from nm in Xel.Elements("artist")
											   select new Artist
											   {
												   Id = (string)nm.Attribute("id"),
												   Name = (string)nm.Attribute("name"),
												   CoverArt = (string)nm.Attribute("coverArt"),
												   AlbumCount = (string)nm.Attribute("albumCount")
											   }).ToList());
				SearchResult.Albums.AddRange((from nm in Xel.Elements("album")
											  select new Album
											  {
												  id = (string)nm.Attribute("id"),
												  Name = (string)nm.Attribute("name"),
												  CoverArt = (string)nm.Attribute("coverArt"),
												  SongCount = (string)nm.Attribute("songCount"),
												  Created = (string)nm.Attribute("created"),
												  Duration = (string)nm.Attribute("duration"),
												  Artist = (string)nm.Attribute("artist"),
												  ArtistId = (string)nm.Attribute("artistId")
											  }).ToList());
				SearchResult.Songs.AddRange((from nm in Xel.Elements("song")
											 select new Song
											 {
												 Id = (string)nm.Attribute("id"),
												 Name = (string)nm.Attribute("title"),
												 Album = (string)nm.Attribute("album"),
												 Artist = (string)nm.Attribute("artist"),
												 Parent = (string)nm.Attribute("parent"),
												 CoverArt = (string)nm.Attribute("coverArt"),
												 Created = (string)nm.Attribute("created"),
												 Duration = (string)nm.Attribute("duration"),
												 BitRate = (string)nm.Attribute("bitRate"),
												 Track = (string)nm.Attribute("track"),
												 Year = (string)nm.Attribute("year"),
												 Genre = (string)nm.Attribute("genre"),
												 Size = (string)nm.Attribute("size"),
												 Suffix = (string)nm.Attribute("suffix"),
												 ContentType = (string)nm.Attribute("contentType"),
												 AlbumId = (string)nm.Attribute("albumId"),
												 ArtistId = (string)nm.Attribute("artistId")
											 }).ToList());

				return SearchResult;
			}
		#endregion

		#region Playlist Method
            public async Task<List<Playlist>> getPlaylists(string username = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				if (!string.IsNullOrEmpty(username))
					parameters.Add("username", username);

				Stream theStream = await this.MakeGenericRequest("getPlaylists", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();
				result = result.Replace(responseHead, "").Replace(responseFoot, "");
				XElement Xel = XElement.Parse(result);
               

                List<Playlist> pl = (from nm in Xel.Elements("playlist")
                                    select new Playlist
                                    {

                                        Id = (string)nm.Attribute("id"),
                                        Name = (string)nm.Attribute("name"),
                                        Comment = (string)nm.Attribute("comment"),
                                        Owner = (string)nm.Attribute("owner"),
                                        IsPublic = (string)nm.Attribute("public"),
                                        SongCount = (string)nm.Attribute("songCount"),
                                        Duration = (string)nm.Attribute("duration"),
                                        Created = (string)nm.Attribute("created"),
                                        AllowedUser = (from au in nm.Elements("allowedUser")
                                                            select new User
                                                            {
                                                                Name = (string)au
                                                            }).ToList()
                                    }).ToList();



                return pl;
			    
			}
			public async Task<Playlist> getPlaylist(string id)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				parameters.Add("id", id);

				Stream theStream = await this.MakeGenericRequest("getPlaylist", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				result = result.Replace(responseHead, "").Replace(responseFoot, "");

				XElement Xel = XElement.Parse(result);
				Playlist pl = new Playlist();
				pl.Id = (string)Xel.Attribute("id");
				pl.Name = (string)Xel.Attribute("name");
				pl.Comment = (string)Xel.Attribute("comment");
				pl.Owner = (string)Xel.Attribute("owner");
				pl.IsPublic = (string)Xel.Attribute("public");
				pl.SongCount = (string)Xel.Attribute("songCount");
				pl.Duration = (string)Xel.Attribute("duration");
				pl.Created = (string)Xel.Attribute("created");
				pl.AllowedUser.AddRange((from au in Xel.Elements("allowedUser")
                                                            select new User
                                                            {
                                                                Name = (string)au
                                                            }).ToList());
				pl.Songs.AddRange((from so in Xel.Elements("entry")
									select new Song {
										Id = (string)so.Attribute("id"),
										Name = (string)so.Attribute("title"),
										Album = (string)so.Attribute("album"),
										Artist = (string)so.Attribute("artist"),
										Parent = (string)so.Attribute("parent"),
										CoverArt = (string)so.Attribute("coverArt"),
										Created = (string)so.Attribute("created"),
										Duration = (string)so.Attribute("duration"),
										BitRate = (string)so.Attribute("bitRate"),
										Track = (string)so.Attribute("track"),
										Year = (string)so.Attribute("year"),
										Genre = (string)so.Attribute("genre"),
										Size = (string)so.Attribute("size"),
										Suffix = (string)so.Attribute("suffix"),
										ContentType = (string)so.Attribute("contentType")									
									}).ToList());

				return pl;
			}
			public async Task<bool> createPlaylist(string playlistId, string name, string songId)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				parameters.Add("playlistId", playlistId);
				parameters.Add("name", name);
				parameters.Add("songId", songId);

				Stream theStream = await this.MakeGenericRequest("createPlaylist", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();		


				return this.ErrorHandler(result);
			}			
			public async Task<bool> updatePlaylist(string playlistId, string name = null, string comment = null, string songIdToAdd = null, string songIndexToRemove = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				parameters.Add("playlistId", playlistId);
				if (!string.IsNullOrEmpty(name))
					parameters.Add("name", name);
				if (!string.IsNullOrEmpty(comment))
					parameters.Add("comment", comment);
				if (!string.IsNullOrEmpty(songIdToAdd))
					parameters.Add("songIdToAdd", songIdToAdd);
				if (!string.IsNullOrEmpty(songIndexToRemove))
					parameters.Add("songIndexToRemove", songIndexToRemove);

				Stream theStream = await this.MakeGenericRequest("updatePlaylist", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				return this.ErrorHandler(result);
			}
			public async Task<bool> deletePlaylist(string id)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				parameters.Add("id", id);

				Stream theStream = await this.MakeGenericRequest("deletePlaylist", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				return this.ErrorHandler(result);
			}		
		#endregion

		#region Media Retrieval Method
			/// <summary>
			/// Streams a given music file. (Renamed from request name "stream")
			/// </summary>
			/// <param name="id">Required: Yes; A string which uniquely identifies the file to stream. 
			/// Obtained by calls to getMusicDirectory.</param>
			/// <param name="maxBitRate">Required: No; If specified, the server will attempt to 
			/// limit the bitrate to this value, in kilobits per second. If set to zero, no limit 
			/// is imposed. Legal values are: 0, 32, 40, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256 and 320. </param>
			/// <returns></returns>
			public string StreamSong(string id, int? maxBitRate = null, string format = null, string timeOffset = null, string size = null, string estimateContentLength = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();
				parameters.Add("id", id);
				if (maxBitRate.HasValue)
					parameters.Add("maxBitRate", maxBitRate.ToString());
				if (!string.IsNullOrEmpty(format))
					parameters.Add("format", format);
				if (!string.IsNullOrEmpty(timeOffset))
					parameters.Add("timeOffset", timeOffset);
				if (!string.IsNullOrEmpty(size))
					parameters.Add("size", size);
				if (!string.IsNullOrEmpty(estimateContentLength))
					parameters.Add("estimateContentLength", estimateContentLength);
				return BuildRequestURL("stream", parameters);
			}
			public string download(string id)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				parameters.Add("id", id);

				return BuildRequestURL("download", parameters);
			}
			public string hls(string id, string bitRate = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				parameters.Add("id", id);
				if(!string.IsNullOrEmpty(bitRate))
					parameters.Add("bitRate", bitRate);
				

				return BuildRequestURL("hls.m3u8", parameters);
			}
			public string getCoverArt(string id, string size = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				parameters.Add("id", id);
				if (!string.IsNullOrEmpty(size))
					parameters.Add("size", size);				

				return BuildRequestURL("getCoverArt", parameters);
			}
			public async Task<Lyrics> getLyrics(string artist = null, string title = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();
				if (!string.IsNullOrEmpty(artist))
					parameters.Add("artist", artist);
				if (!string.IsNullOrEmpty(title))
					parameters.Add("title", title);
				Stream theStream = await this.MakeGenericRequest("getLyrics", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();
				result = result.Replace(responseHead, "").Replace(responseFoot, "");
				Lyrics ly = new Lyrics();
				XElement Xel = XElement.Parse(result);
				ly.artist = (string)Xel.Attribute("artist");
				ly.title = (string)Xel.Attribute("title");
				ly.Text = (string)Xel;


				return ly;
			}
			public string getAvatar(string username)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				parameters.Add("username", username);
				
				return BuildRequestURL("getCoverArt", parameters);
			}
		#endregion

		#region Media Annotation Method
			public async Task<bool> star(string id = null, string albumId = null, string artistId = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				if (!string.IsNullOrEmpty(id))
					parameters.Add("id", id);
				if (!string.IsNullOrEmpty(albumId))
					parameters.Add("albumId", albumId);
				if (!string.IsNullOrEmpty(artistId))
					parameters.Add("artistId", artistId);
				Stream theStream = await this.MakeGenericRequest("star", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				return this.ErrorHandler(result);
			}
			public async Task<bool> unstar(string id = null, string albumId = null, string artistId = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				if (!string.IsNullOrEmpty(id))
					parameters.Add("id", id);
				if (!string.IsNullOrEmpty(albumId))
					parameters.Add("albumId", albumId);
				if (!string.IsNullOrEmpty(artistId))
					parameters.Add("artistId", artistId);
				Stream theStream = await this.MakeGenericRequest("unstar", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				return this.ErrorHandler(result);
			}
			public async Task<bool> setRating(string id = null, string rating = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();
		
				parameters.Add("id", id);			
				parameters.Add("rating", rating);

				Stream theStream = await this.MakeGenericRequest("setRating", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				return this.ErrorHandler(result); 
			}
			public async Task<bool> scrobble(string id, string submission = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();
						
				parameters.Add("id", id);
				if (!string.IsNullOrEmpty(submission))
					parameters.Add("submission", submission);
				Stream theStream = await this.MakeGenericRequest("scrobble", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				return this.ErrorHandler(result);
			}
		#endregion

		#region Sharing Method
			public async Task<Share> getShares()
			{
                Dictionary<string, string> parameters = new Dictionary<string, string>();
			
				Stream theStream = await this.MakeGenericRequest("getShares", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				result = result.Replace(responseHead, "").Replace(responseFoot, "");

				XElement Xel = XElement.Parse(result);
				Share sh = new Share();

				sh.Id = (string)Xel.Attribute("id");
				sh.Url = (string)Xel.Attribute("url");
				sh.Description = (string)Xel.Attribute("description");
				sh.Username = (string)Xel.Attribute("username");
				sh.Created = (string)Xel.Attribute("created");
				sh.LastVisited = (string)Xel.Attribute("lastVisited");
				sh.Expires = (string)Xel.Attribute("expires");
				sh.VisitCount = (string)Xel.Attribute("visitCount");
				sh.SharedSongs.AddRange((from nm in Xel.Elements("entry")
										select new Song 
										{
											Id = (string)nm.Attribute("id"),
											Name = (string)nm.Attribute("title"),
											Album = (string)nm.Attribute("album"),
											Artist = (string)nm.Attribute("artist"),
											Parent = (string)nm.Attribute("parent"),
											CoverArt = (string)nm.Attribute("coverArt"),
											Created = (string)nm.Attribute("created"),
											Duration = (string)nm.Attribute("duration"),
											BitRate = (string)nm.Attribute("bitRate"),
											Track = (string)nm.Attribute("track"),
											Year = (string)nm.Attribute("year"),
											Genre = (string)nm.Attribute("genre"),
											Size = (string)nm.Attribute("size"),
											Suffix = (string)nm.Attribute("suffix"),
											ContentType = (string)nm.Attribute("contentType"),
											AlbumId = (string)nm.Attribute("albumId"),
											ArtistId = (string)nm.Attribute("artistId")
										
										}).ToList());

				return sh;
			}
			public async Task<Share> createShare(string id, string description = null, string expires = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				parameters.Add("id", id);
				if (!string.IsNullOrEmpty(description))
					parameters.Add("description", description);
				if (!string.IsNullOrEmpty(expires))
					parameters.Add("expires", expires);
				Stream theStream = await this.MakeGenericRequest("createShare", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				result = result.Replace(responseHead, "").Replace(responseFoot, "");

				XElement Xel = XElement.Parse(result);
				Share sh = new Share();

				sh.Id = (string)Xel.Attribute("id");
				sh.Url = (string)Xel.Attribute("url");
				sh.Description = (string)Xel.Attribute("description");
				sh.Username = (string)Xel.Attribute("username");
				sh.Created = (string)Xel.Attribute("created");
				sh.LastVisited = (string)Xel.Attribute("lastVisited");
				sh.Expires = (string)Xel.Attribute("expires");
				sh.VisitCount = (string)Xel.Attribute("visitCount");
				sh.SharedSongs.AddRange((from nm in Xel.Elements("entry")
										 select new Song
										 {
											 Id = (string)nm.Attribute("id"),
											 Name = (string)nm.Attribute("title"),
											 Album = (string)nm.Attribute("album"),
											 Artist = (string)nm.Attribute("artist"),
											 Parent = (string)nm.Attribute("parent"),
											 CoverArt = (string)nm.Attribute("coverArt"),
											 Created = (string)nm.Attribute("created"),
											 Duration = (string)nm.Attribute("duration"),
											 BitRate = (string)nm.Attribute("bitRate"),
											 Track = (string)nm.Attribute("track"),
											 Year = (string)nm.Attribute("year"),
											 Genre = (string)nm.Attribute("genre"),
											 Size = (string)nm.Attribute("size"),
											 Suffix = (string)nm.Attribute("suffix"),
											 ContentType = (string)nm.Attribute("contentType"),
											 AlbumId = (string)nm.Attribute("albumId"),
											 ArtistId = (string)nm.Attribute("artistId")

										 }).ToList());

				return sh;
			}
			public async Task<bool> updateShare(string id, string description = null, string expires = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				parameters.Add("id", id);
				if (!string.IsNullOrEmpty(description))
					parameters.Add("description", description);
				if (!string.IsNullOrEmpty(expires))
					parameters.Add("expires", expires);
				Stream theStream = await this.MakeGenericRequest("updateShare", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				return this.ErrorHandler(result);
			}
			public async Task<bool> deleteShare(string id)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				parameters.Add("id", id);

				Stream theStream = await this.MakeGenericRequest("deleteShare", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				return this.ErrorHandler(result);

			}
		#endregion

		#region Podcast Method
			public async Task<List<Channel>> getPodcasts()
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>();

			Stream theStream = await this.MakeGenericRequest("getPodcasts", parameters);
			

			StreamReader sr = new StreamReader(theStream);

			string result = sr.ReadToEnd();

			result = result.Replace(responseHead, "").Replace(responseFoot, "");
			XElement Xel = XElement.Parse(result);

			List<Channel> channellist = (from nm in Xel.Elements("channel")
										select new Channel 
										{
											Id = (string)nm.Attribute("id"),
											Url = (string)nm.Attribute("url"),
											Title = (string)nm.Attribute("title"),
											Description = (string)nm.Attribute("description"),
											Status  = (string)nm.Attribute("status"),
											Episodes = (from ep in nm.Elements("episode")
														select new Episode{
															Id=(string)ep.Attribute("id"),
															StreamId=(string)ep.Attribute("streamId"),
															Title=(string)ep.Attribute("title"),
															Description=(string)ep.Attribute("description"),
															PublishDate=(string)ep.Attribute("publishDate"),
															Status=(string)ep.Attribute("status"),
															Parent=(string)ep.Attribute("parent"),
															Year=(string)ep.Attribute("year"),
															Genre=(string)ep.Attribute("genre"),
															CoverArt=(string)ep.Attribute("coverArt"),
															Size=(string)ep.Attribute("size"),
															ContentType=(string)ep.Attribute("contentType"),
															Suffix=(string)ep.Attribute("suffix"),
															Duration=(string)ep.Attribute("duration"),
															BitRate=(string)ep.Attribute("bitRate")
										
														}).ToList()
										}).ToList();

			return channellist;
		}
		#endregion

		#region Jukebox Method
		public async Task<JukeBoxStatut> jukeboxControl(action_type action, string index = null, string offset = null, string id = null, string gain = null)
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>();

			parameters.Add("action", action.ToString());
			if (!string.IsNullOrEmpty(index))
				parameters.Add("index", index);
			if (!string.IsNullOrEmpty(id))
				parameters.Add("id", id);
			if (!string.IsNullOrEmpty(gain))
				parameters.Add("gain", gain);
			
			Stream theStream = await this.MakeGenericRequest("jukeboxControl", parameters);
			

			StreamReader sr = new StreamReader(theStream);

			string result = sr.ReadToEnd();
			result = result.Replace(responseHead, "").Replace(responseFoot, "");
			XElement Xel = XElement.Parse(result);
			JukeBoxStatut jbs = new JukeBoxStatut();
			
			jbs.CurrentIndex = (string)Xel.Attribute("currentIndex");
			jbs.Playing = (string)Xel.Attribute("playing");
			jbs.gain = (string)Xel.Attribute("gain");
			jbs.position = (string)Xel.Attribute("position");
			jbs.Entry.AddRange((from nm in Xel.Elements("entry")
								select new Song 
								{
									Id = (string)nm.Attribute("id"),
									Name = (string)nm.Attribute("title"),
									Album = (string)nm.Attribute("album"),
									Artist = (string)nm.Attribute("artist"),
									Parent = (string)nm.Attribute("parent"),
									CoverArt = (string)nm.Attribute("coverArt"),
									Created = (string)nm.Attribute("created"),
									Duration = (string)nm.Attribute("duration"),
									BitRate = (string)nm.Attribute("bitRate"),
									Track = (string)nm.Attribute("track"),
									Year = (string)nm.Attribute("year"),
									Genre = (string)nm.Attribute("genre"),
									Size = (string)nm.Attribute("size"),
									Suffix = (string)nm.Attribute("suffix"),
									ContentType = (string)nm.Attribute("contentType"),
									AlbumId = (string)nm.Attribute("albumId"),
									ArtistId = (string)nm.Attribute("artistId")
								}).ToList());

			return jbs;
		}
		#endregion
		
		#region Chat Method
		public async Task<List<ChatMessage>> getChatMessages(string since = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				if (!string.IsNullOrEmpty(since))
					parameters.Add("since", since);

				Stream theStream = await this.MakeGenericRequest("getChatMessages", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();
				result = result.Replace(responseHead, "").Replace(responseFoot, "");
				XElement Xel = XElement.Parse(result);
			List<ChatMessage> Lcm = (from cm in Xel.Elements("chatMessage")
									select new ChatMessage {
										Username = (string)cm.Attribute("username"),
										time = (string)cm.Attribute("time"),
										message = (string)cm.Attribute("message")						
									}).ToList();

			return Lcm;
			}
		public async Task<bool> addChatMessage(string message)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();
						
				parameters.Add("message", message);

				Stream theStream = await this.MakeGenericRequest("addChatMessage", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				return this.ErrorHandler(result);
			}
		#endregion

		#region User Management Method
			public async Task<User> getUser(string username)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				parameters.Add("username", username);

				Stream theStream = await this.MakeGenericRequest("getUser", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();
				result = result.Replace(responseHead, "").Replace(responseFoot, "");
				XElement Xel = XElement.Parse(result);
				User u = new User();
				u.Name = (string)Xel.Attribute("username");
				u.email = (string)Xel.Attribute("email");
				u.scrobblingEnabled = (string)Xel.Attribute("scrobblingEnabled");
				u.adminRole = (string)Xel.Attribute("adminRole");
				u.settingsRole = (string)Xel.Attribute("settingsRole");
				u.uploadRole = (string)Xel.Attribute("uploadRole");
				u.playlistRole = (string)Xel.Attribute("playlistRole");
				u.coverArtRole = (string)Xel.Attribute("coverArtRole");
				u.commentRole = (string)Xel.Attribute("commentRole");
				u.podcastRole = (string)Xel.Attribute("podcastRole");
				u.streamRole = (string)Xel.Attribute("streamRole");
				u.jukeboxRole = (string)Xel.Attribute("jukeboxRole");
				u.shareRole = (string)Xel.Attribute("shareRole");
				return u;
			}
			public async Task<string> getUsers()
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				Stream theStream = await this.MakeGenericRequest("getUsers", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				return result;
			}
			public async Task<string> createUser(string username, string password, string email, string ldapAuthenticated = null, string adminRole = null, string settingsRole = null, string streamRole = null, string jukeboxRole = null, string downloadRole = null, string uploadRole = null, string playlistRole = null, string coverArtRole = null, string commentRole = null, string podcastRole = null, string shareRole = null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				parameters.Add("username", username);
				parameters.Add("password", password);
				parameters.Add("email", email);
				if (!string.IsNullOrEmpty(ldapAuthenticated))
					parameters.Add("ldapAuthenticated", ldapAuthenticated);
				if (!string.IsNullOrEmpty(adminRole))
					parameters.Add("adminRole", adminRole);
				if (!string.IsNullOrEmpty(settingsRole))
					parameters.Add("settingsRole", settingsRole);
				if (!string.IsNullOrEmpty(streamRole))
					parameters.Add("streamRole", streamRole);
				if (!string.IsNullOrEmpty(jukeboxRole))
					parameters.Add("jukeboxRole", jukeboxRole);
				if (!string.IsNullOrEmpty(downloadRole))
					parameters.Add("downloadRole", downloadRole);
				if (!string.IsNullOrEmpty(uploadRole))
					parameters.Add("uploadRole", uploadRole);
				if (!string.IsNullOrEmpty(playlistRole))
					parameters.Add("playlistRole", playlistRole);
				if (!string.IsNullOrEmpty(coverArtRole))
					parameters.Add("coverArtRole", coverArtRole);
				if (!string.IsNullOrEmpty(commentRole))
					parameters.Add("commentRole", commentRole);
				if (!string.IsNullOrEmpty(podcastRole))
					parameters.Add("podcastRole", podcastRole);
				if (!string.IsNullOrEmpty(shareRole))
					parameters.Add("shareRole", shareRole);

				Stream theStream = await this.MakeGenericRequest("createUser", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				return result;
			}
			public async Task<string> deleteUser(string username)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				parameters.Add("username", username);

				Stream theStream = await this.MakeGenericRequest("deleteUser", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				return result;
			}
			public async Task<string> changePassword(string username, string password)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();

				parameters.Add("username", username);
				parameters.Add("password", string.Format("enc:{0}", ConvertStringToHex(password)));

				Stream theStream = await this.MakeGenericRequest("changePassword", parameters);
				

				StreamReader sr = new StreamReader(theStream);

				string result = sr.ReadToEnd();

				return result;
			}
		#endregion
    }

}

