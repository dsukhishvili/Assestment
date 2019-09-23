using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Service.DAL;
using UserManagement.Service.Services;
using UserManagement.Service.Infrastructure;
using AutoMapper;
using UserManagement.ViewModels;
using UserManagement.Service.DTOModels;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private UserService _service { get; set; }
        private readonly IMapper _mapper;
        public ReportController(IUserRepository repo, IHostingEnvironment hostingEnv, ILogger logger, IMapper mapper)
        {
            _service = new UserService(repo, hostingEnv, logger);
            _mapper = mapper;
        }
        [HttpGet]
        [Route("RelationsReport")]
        public async Task<IActionResult> GetReport()
        {
            var reportDto = await _service.GetRelationReport();
            var reportVm = _mapper.Map<List<RelatedPersonReportViewModel>>(reportDto);
            return Ok(reportVm);
        }
    }
}