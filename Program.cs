﻿// See https://aka.ms/new-console-template for more information

using NLog;

string path = Directory.GetCurrentDirectory() + "\\nlog.config";
// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");

string scrubbedFile = FileScrubber.ScrubMovies("movies.csv");
logger.Info(scrubbedFile);
MovieFile movieFile = new MovieFile(scrubbedFile);
logger.Info("Program ended");
do
{
    Console.WriteLine("Media Library");
    Console.WriteLine("1) View all movies.");
    Console.WriteLine("2) Add to the movie file.");
    Console.WriteLine("Enter anything else to quit.");

    string resp = Console.ReadLine();
    //view movies
    if (resp == "1")
{
    var sortedMovies = movieFile.Movies.OrderBy(m => m.title);
    foreach(Movie m in sortedMovies)
    {
        Console.WriteLine(m.Display());
    }
}

else if (resp == "2")
{
    try
    {   
        Movie m = new Movie();
        Console.WriteLine("Enter title: ");
        m.title = Console.ReadLine();
        Console.WriteLine("Enter all the genres and when you are finished type 'done' and press enter.");
        string genre;
        while((genre = Console.ReadLine()) != "done")
        {
            m.genres.Add(genre);
        }
                   
        Console.WriteLine("Enter director: ");
        m.director = Console.ReadLine();
        Console.WriteLine("Enter runtime (h:m:s): ");
        string runtimeInput = Console.ReadLine();
        m.runningTime = TimeSpan.Parse(runtimeInput);   
        
        // Add movie to MovieFile
        movieFile.Movies.Add(m);
        
        // Write all movies to file
        using (StreamWriter sw = new StreamWriter(movieFile.filePath))
        {
            foreach (var movie in movieFile.Movies)
            {
                sw.WriteLine($"{movie.title},{string.Join("|", movie.genres)},{movie.director},{movie.runningTime}");
            }
        }
    }
    catch (IOException e)
    {
        logger.Error(e, "An error occured entering in the information.");    
    }      
}

} while (true);
{

}

