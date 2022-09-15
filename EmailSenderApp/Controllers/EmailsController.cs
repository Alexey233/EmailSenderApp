using EmailSenderApplication.Data;
using EmailSenderApplication.Models;
using EmailSenderApplication.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Json;

namespace EmailSenderApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailsController(IEmailService emailService, ApplicationContext applicationContext)
        {
            _emailService = emailService;
        }

        [HttpGet]
        public Task<string> ShowSendEmail()
        {
            var jsonEmail = _emailService.ShowSendEmail();
            return jsonEmail;
        }


        [HttpPost]
        public IActionResult SendEmail(Email requests)
        {
            _emailService.SendEmail(requests);
            return Ok();
        }
    }
}
