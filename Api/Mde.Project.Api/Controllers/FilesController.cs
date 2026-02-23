using Mde.Project.Api.Core.Services.Files;
using Microsoft.AspNetCore.Mvc;

namespace Mde.Project.Api.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFilesService _filesService;
        public FilesController(IFilesService filesService)
        {
            _filesService = filesService;
        }
    }
}
