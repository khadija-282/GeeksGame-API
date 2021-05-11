using GeeksGame.DTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GeeksGame.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DefaultController : ControllerBase
    {

        private readonly ILogger<DefaultController> _logger;
        private string _path;
        private int _totalcount;
        public DefaultController(ILogger<DefaultController> logger, IOptions<AppSetting> settings)
        {
            _logger = logger;
            _path = settings.Value.Path;
            _totalcount = settings.Value.TotalPictures;
        }

        [HttpGet] 
        [EnableCors("MyPolicy")]
        public Response GetFiles()
        {
            List<Files> files = new List<Files>();
            var count = 0;
            if (Directory.Exists(_path))
            {
                DirectoryInfo Folder = new DirectoryInfo(_path);
                FileInfo[] Images = Folder.GetFiles();
                foreach (var i in Images)
                {
                    if ( count< _totalcount && Regex.IsMatch(i.Name, @".jpg|.png|.gif|.jpeg|.jfif$"))
                    {
                        var extractName = i.Name.Split('-'); // file name has its nationality with seperated by - 
                        if (extractName.Length > 0)
                        {
                            files.Add(new Files()
                            {
                                Name = extractName[0],
                                Type = extractName[1].Replace(i.Extension, ""), // nationality for score calculation
                                Extension = i.Extension,
                                Path = i.Name
                            });
                        }
                    }
                }
            }
            var response = new Response();
            response.Result=files;
            return response;
        }
    }
}
