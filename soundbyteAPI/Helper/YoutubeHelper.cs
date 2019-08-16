using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using soundbyteAPI.Model;

namespace soundbyteAPI.Helper
{
    public class YouTubeHelper
    {
        public static void testProgram()
        {
            Console.WriteLine(GetVideoInfo("fLexgOxsZu0"));

            Console.ReadLine();

        }

        public static String GetVideoIdFromURL(String videoURL)
        {
            int indexOfFirstId = videoURL.IndexOf("=") + 1;
            String videoId = videoURL.Substring(indexOfFirstId);
            return videoId;
        }

        public static Video GetVideoInfo(String videoId)
        {
            String APIKey = "AIzaSyCPepu95RKkKEYYR4oZ2RXnx1ZYgfHSvTo";
            String YouTubeAPIURL = "https://www.googleapis.com/youtube/v3/videos?id=" + videoId + "&key=" + APIKey + "&part=snippet,contentDetails";

            // Use an http client to grab the JSON string from the web.
            String videoInfoJSON = new WebClient().DownloadString(YouTubeAPIURL);

            // Using dynamic object helps us to more efficiently extract information from a large JSON String.
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(videoInfoJSON);

            // Extract information from the dynamic object.
            String title = jsonObj["items"][0]["snippet"]["title"];
            String thumbnailURL = jsonObj["items"][0]["snippet"]["thumbnails"]["medium"]["url"];
            String durationString = jsonObj["items"][0]["contentDetails"]["duration"];
            String categoryId = jsonObj["items"][0]["snippet"]["categoryId"];
            String videoUrl = "https://www.youtube.com/watch?v=" + videoId;

            // duration is given in this format: PT4M17S, we need to use a simple parser to get the duration in seconds.
            TimeSpan videoDuration = XmlConvert.ToTimeSpan(durationString);
            int duration = (int)videoDuration.TotalSeconds;

            // Create a new Video Object from the model defined in the API.
            Video video = new Video
            {
                VideoTitle = title,
                WebUrl = videoUrl,
                VideoLength = duration,
                VideoType = categoryId,
                ThumbnailUrl = thumbnailURL
            };
            return video;
        }
    }
}