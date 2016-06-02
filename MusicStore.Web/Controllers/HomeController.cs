using MusicStore.Entities;
using MusicStore.Web.DataContexts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MusicStore.Web.Controllers
{
    public class HomeController : Controller
    {
        Db db = new Db();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Automatisation(string AlbumName)
        {
            var albumQuery = from m in db.Albums
                           where m.ReleaseDate>DateTime.Today
                           select m.Name;
            DateTime temp = DateTime.Now;
            List<DateTime> dates = new List<DateTime>();
            //while(temp<)

            ViewData["DatesDD"] = new SelectList(dates.Distinct());

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
        public ActionResult SQLQuery(string query)
        {
            return View(GetQueryResult(query));
        }
        
        public ActionResult SQLQueryDefault(string query)
        {
            query = this.Request.Form["lol"];
            return View(GetQueryResult(query));
        }

        public static Query GetQueryResult(string sqlQuery)
        {
            
            var qryEntity = new Query();
            if (!String.IsNullOrEmpty(sqlQuery))
            {
                const string connectionString = @"Data Source=(LocalDb)\v11.0;AttachDbFilename=|DataDirectory|\aspnet-MusicStore.Web-20150316091009.mdf;Initial Catalog=aspnet-MusicStore.Web-20150316091009;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        //try
                        //{
                            if (sqlQuery.StartsWith("select"))
                            {
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        var row = new List<string>();
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            row.Add(reader[i].ToString());
                                            if (qryEntity.ColumnNamesList.Count != reader.FieldCount)
                                            {
                                                qryEntity.ColumnNamesList.Add(reader.GetName(i));
                                            }
                                        }
                                        qryEntity.TableContent.Add(row);
                                    }
                                }
                            }//select songs.name,songs.duration from artists,albums,songs where artists.artistid=albums.artistid and albums.albumid=songs.albumid and artists.name="Александр"
                            else
                            {
                                try
                                {
                                    qryEntity.ColumnNamesList.Add(command.ExecuteNonQuery() + " row updated");
                                }
                                catch (SqlException)
                                { qryEntity.ColumnNamesList.Add("Wrong syntax"); }
                            }
                        //}
                        //catch (SqlException)
                        //{
                        //    qryEntity.ColumnNamesList.Add("Wrong syntax");
                        //}
                    }
                }
            }
            return qryEntity;
        }

        
    }

    public static class Side
    { 
        public static void FileWork(Db db, Artist artist)
        {
            var dataFile = HttpContext.Current.Server.MapPath("~/Content/AboutArtist.html");
            var dataOut = HttpContext.Current.Server.MapPath("~/Content/AboutArtistOutput.html");
            string template = File.ReadAllText(dataFile);
            template = template.Replace("ArtistName", artist.Name);
            template = template.Replace("PhoneNumber",artist.PhoneNumber);
            template = template.Replace("YearOfBirth", Convert.ToString(artist.YearOfBeginning));
            template = template.Replace("CountOfAlbums", Convert.ToString(artist.Albums.Count));                           
            int songs=0;
            int lengthOfSongs = 0;
            StringBuilder genres = new StringBuilder();
            foreach (var album in artist.Albums)
            {
                songs += album.Songs.Count;
                foreach (var song in album.Songs)
                { 
                    lengthOfSongs += song.Duration;
                    genres.Append(song.Category).Append(", ");
                }
            }
            template = template.Replace("Genres", genres.ToString());
            template = template.Replace("CountOfSongs", Convert.ToString(songs));
            template = template.Replace("AllSongsDuration", Convert.ToString(lengthOfSongs));
            File.WriteAllText(dataOut, template);
        }

        public static void FileWork2(Db db, Album album)
        {
            var dataFile = HttpContext.Current.Server.MapPath("~/Content/AboutAlbum.html");
            var dataOut = HttpContext.Current.Server.MapPath("~/Content/AboutAlbumOutput.html");
            string template = File.ReadAllText(dataFile);
            template = template.Replace("AlbumName", album.Name);
            template = template.Replace("Cost", Convert.ToString(album.Cost));
            template = template.Replace("Circulation", Convert.ToString(album.Circulation));
            template = template.Replace("CountOfSongs", Convert.ToString(album.Songs.Count));
            int lengthOfSongs = 0;
            StringBuilder genres = new StringBuilder();
            foreach (var songs in album.Songs)
            {
                lengthOfSongs += songs.Duration;
                
            }
            template = template.Replace("Duration", Convert.ToString(lengthOfSongs));
            File.WriteAllText(dataOut, template);
        }
    }
}