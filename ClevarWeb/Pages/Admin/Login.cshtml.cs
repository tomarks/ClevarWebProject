using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.IO;
using ClevarWeb.Data.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using ClevarWeb.Utility;

namespace ClevarWeb
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration configuration;
        public LoginModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        #region Honeypot
        // Basic Honeypot
        [BindProperty]
        [Display(Name = "Email", GroupName = "Login Form", Prompt = "Email")]
        public string Email { get; set; }
        #endregion Honeypot

        [BindProperty]
        [Display(Name = "User Name", GroupName = "Login Form", Prompt = "Username")]
        public string UserName { get; set; }

        [BindProperty, DataType(DataType.Password)]
        [Display(Name = "Password", GroupName = "Login Form", Prompt = "Password")]
        public string Password { get; set; }

        public string Message { get; set; }


        public async Task<IActionResult> OnPost()
        {
            if(UserName.HasValue() && Password.HasValue() && Email.IsEmpty())
            {
                bool validLogin = (UserName == SystemProperties.Instance.Username) && (SystemProperties.Instance.Password == Password);

                if (Debugger.IsAttached)
                    if (!validLogin) validLogin = UserName.ToLower() == "admin" && Password.ToLower() == "admin";

                if (validLogin)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, UserName)
                    };

                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        // Refreshing the authentication session should be allowed.

                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30),
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        IsPersistent = false,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        //IssuedUtc = <DateTimeOffset>,
                        // The time at which the authentication ticket was issued.

                        //RedirectUri = <string>
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };


                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    return RedirectToPage("/Admin/Index");
                }

                if (!validLogin)
                    General.Instance.WriteToLog($"Invalid login attempt from - {Request.HttpContext.Connection.RemoteIpAddress.ToString()}");
            }

            Message = "Invalid attempt";
            return Page();
        }
    }
}