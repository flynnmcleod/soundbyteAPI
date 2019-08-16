using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using soundbyteAPI.Helper;
using soundbyteAPI.Model;

namespace soundbyteAPI.Controllers
{

    public class URLDTO
    {
        public String URL { get; set; }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly soundbyteContext _context;

        public VideoController(soundbyteContext context)
        {
            _context = context;
        }

        // GET: api/Video
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Video>>> GetVideo()
        {
            return await _context.Video.ToListAsync();
        }

        // GET: api/Video/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Video>> GetVideo(int id)
        {
            var video = await _context.Video.FindAsync(id);

            if (video == null)
            {
                return NotFound();
            }

            return video;
        }

        // PUT: api/Video/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVideo(int id, Video video)
        {
            if (id != video.VideoId)
            {
                return BadRequest();
            }

            _context.Entry(video).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Video
        [HttpPost]
        public async Task<ActionResult<Video>> PostVideo([FromBody]URLDTO data)
        {
            Video video;
            String videoURL;
            String videoId;
            try
            {
                // Constructing the video object from our helper function
                videoURL = data.URL;
                videoId = YouTubeHelper.GetVideoIdFromURL(videoURL);
                video = YouTubeHelper.GetVideoInfo(videoId);
            }
            catch
            {
                return BadRequest("Invalid YouTube URL");
            }

            // Add this video object to the database
            _context.Video.Add(video);
            await _context.SaveChangesAsync();

            // Return success code and the info on the video object
            return CreatedAtAction("GetVideo", new { id = video.VideoId }, video);
        }

        // DELETE: api/Video/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Video>> DeleteVideo(int id)
        {
            var video = await _context.Video.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }

            _context.Video.Remove(video);
            await _context.SaveChangesAsync();

            return video;
        }

        private bool VideoExists(int id)
        {
            return _context.Video.Any(e => e.VideoId == id);
        }
    }
}
