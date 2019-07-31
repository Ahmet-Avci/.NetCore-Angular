using Authors.Helpers;
using DtoLayer.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Authors.Extensions
{
    public class SpeacialController : Controller
    {
        public SpeacialController()
        {
            if (HttpContext != null)
            {
                SessionData<AuthorDto>.SessionValue = SessionManager.GetObject<AuthorDto>(HttpContext.Session, "LoginUser");
            }
        }
    }
}