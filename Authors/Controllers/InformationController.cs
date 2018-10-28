using System;
using System.Collections.Generic;
using DbLayer;
using Microsoft.AspNetCore.Mvc;

namespace Authors.Controllers
{
    [Route("api/[controller]")]
    public class InformationController : Controller
    {

        List<AuthorEntity> authors = new List<AuthorEntity>();

        [HttpGet("[action]")]
        public List<AuthorEntity> GetAllAuthors()
        {
            authors.Add(new AuthorEntity()
            {
                CreatedBy   = 1,
                CreatedDate = DateTime.Now,
                MailAddress = "ahmet.avci@gmail.com",
                PhoneNumber = "05343883952",
                Name        = "Ahmet",
                Surname     = "Avcı"
            });

            authors.Add(new AuthorEntity()
            {
                CreatedBy = 2,
                CreatedDate = DateTime.Now.AddDays(-1),
                MailAddress = "baris.unlu@gmail.com",
                PhoneNumber = "0111111111",
                Name = "Barış",
                Surname = "Ünlü"
            });

            return authors;
        }
    }
}