using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BrightTest.ViewModels;
using BrightTest.Models;

namespace BrightTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly RequestContext _context;

        public HomeController(HttpClient httpClient, RequestContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetUrlInfo(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                UrlInfoViewModel response = null;
                try
                {
                    var urlResponse = await _httpClient.GetAsync(url);

                    var regex = new Regex(@"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase);
                    var title = regex.Match(await urlResponse.Content.ReadAsStringAsync()).Groups["Title"].Value;
                    var statusCode = Convert.ToInt32(urlResponse.StatusCode);

                    response = new UrlInfoViewModel() { Title = title, StatusCode = statusCode };
                }
                catch (Exception)
                {
                    response = new UrlInfoViewModel()
                    {
                        Title = "Sorry, some error occured! Please check if url is valid..",
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }

                var request = new Request
                {
                    Date = DateTime.Now,
                    URL = url,
                    StatusCode = response.StatusCode,
                    Title = response.Title
                };

                _context.Requests.Add(request);
                await _context.SaveChangesAsync();

                return Ok(response);
            }

            return BadRequest(new { Message = "The passed url is invalid!" });
        }
    }
}