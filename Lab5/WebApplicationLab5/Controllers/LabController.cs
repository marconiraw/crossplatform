using Microsoft.AspNetCore.Mvc;
using Library;
using System;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using WebApplicationLab5.Models;
using WebApplicationLab5.Services;

namespace WebApplicationLab5.Controllers
{
    public class LabController : Controller
    {
        [Authorize]
        public IActionResult Lab1()
        {
            return View();
        }

        [Authorize]
        public IActionResult Lab2()
        {
            return View();
        }

        [Authorize]
        public IActionResult Lab3()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RunLab(int labNumber, string inputFilePath, string outputFilePath)
        {
            try
            {
                switch (labNumber)
                {
                    case 1:
                        Library.Lab1.Run(inputFilePath, outputFilePath);
                        break;
                    case 2:
                        Library.Lab2.Run(inputFilePath, outputFilePath);
                        break;
                    case 3:
                        Library.Lab3.Run(inputFilePath, outputFilePath);
                        break;
                    default:
                        throw new Exception("Invalid lab number.");
                }

                ViewBag.Message = $"Лабораторна {labNumber} виконана успішно!";
                return View($"Lab{labNumber}");
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Помилка: {ex.Message}";
                return View($"Lab{labNumber}");
            }
        }

    }
}