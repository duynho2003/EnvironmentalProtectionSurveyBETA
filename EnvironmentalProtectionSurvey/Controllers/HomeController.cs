﻿using EnvironmentalProtectionSurvey.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EnvironmentalProtectionSurvey.Controllers
{
    public class HomeController : Controller
    {
        private Project2Context _context;
        public HomeController(Project2Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //lay ra user đăng nhâp 
            DateTime currentDate = DateTime.Now;

            var username = HttpContext.Session.GetString("username");
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            var news = _context.News.ToList();
            // Lọc những cuộc thi còn hạn
            var contests = _context.Contests
                .Where(c => c.StartTime <= currentDate && c.EndTime >= currentDate)
                .OrderByDescending(c => c.Id)
                .Take(3)
                .ToList();
            List<Survey> survey;
            if (user == null)
            {

                survey = _context.Surveys
                    .Where(s => s.IsVisible == null && s.CreatedAt <= currentDate && (s.EndAt == null || s.EndAt >= currentDate))
                    .OrderByDescending(s => s.Id)
                    .Take(3)
                    .Include(s => s.Questions)
                        .ThenInclude(q => q.Options)
                    .ToList();
            }
            else if (user.Role == "Admin")
            {

                survey = _context.Surveys
                    .Where(s => s.IsVisible == null && s.CreatedAt <= currentDate && (s.EndAt == null || s.EndAt >= currentDate))
                    .OrderByDescending(s => s.Id)
                    .Take(3)
                    .Include(s => s.Questions)
                    .ThenInclude(q => q.Options)
                    .ToList();
            }
            else
            {

                survey = _context.Surveys
                    .Where(s => s.IsVisible == null && s.UserType == user!.Role || s.UserType == "All" && s.CreatedAt <= currentDate && (s.EndAt == null || s.EndAt >= currentDate))
                    .OrderByDescending(s => s.Id)
                    .Take(3)
                    .Include(s => s.Questions)
                        .ThenInclude(q => q.Options)
                    .ToList();

            }
            List<int> studentCounts = new List<int>();
            List<int> teacherCounts = new List<int>();

            foreach (var s in survey)
            {
                var studentCount = _context.Users
                    .Join(_context.FilledSurveys,
                        user => user.Id,
                        filledSurvey => filledSurvey.UserId,
                        (user, filledSurvey) => new { User = user, FilledSurvey = filledSurvey })
                    .Where(joinResult => joinResult.User.Role == "Student" && joinResult.FilledSurvey.SurveyId == s.Id)
                    .GroupBy(joinResult => joinResult.User.Id)
                    .Count();

                var teacherCount = _context.Users
                    .Join(_context.FilledSurveys,
                        user => user.Id,
                        filledSurvey => filledSurvey.UserId,
                        (user, filledSurvey) => new { User = user, FilledSurvey = filledSurvey })
                    .Where(joinResult => joinResult.User.Role == "Teacher" && joinResult.FilledSurvey.SurveyId == s.Id)
                    .GroupBy(joinResult => joinResult.User.Id)
                    .Count();

                // Add the counts to the lists
                studentCounts.Add(studentCount);
                teacherCounts.Add(teacherCount);
            }

            // Pass the lists to the view using ViewBag
            ViewBag.StudentParticipantCounts = studentCounts;
            ViewBag.TeacherParticipantCounts = teacherCounts;

            // Create the view model
            var viewModel = new NewsSurveyViewModel
            {
                NewsList = news,
                SurveyList = survey,
                ContestList = contests
            };
            return View(viewModel);
        }

        public IActionResult SurveyBoard()
        {
            //lay ra user đăng nhâp 
            DateTime currentDate = DateTime.Now;

            var username = HttpContext.Session.GetString("username");
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            var news = _context.News.ToList();
            // Lọc những cuộc thi còn hạn
            var contests = _context.Contests
                .Where(c => c.StartTime <= currentDate && c.EndTime >= currentDate)
                .OrderByDescending(c => c.Id)
                .ToList();
            List<Survey> survey;
            if (user == null)
            {

                survey = _context.Surveys
                    .Where(s => s.IsVisible == null && s.CreatedAt <= currentDate && (s.EndAt == null || s.EndAt >= currentDate))
                    .OrderByDescending(s => s.Id)
                    .Include(s => s.Questions)
                        .ThenInclude(q => q.Options)
                    .ToList();
            }
            else if (user.Role == "Admin")
            {

                survey = _context.Surveys
                    .Where(s => s.IsVisible == null && s.CreatedAt <= currentDate && (s.EndAt == null || s.EndAt >= currentDate))
                    .OrderByDescending(s => s.Id)
                    .Include(s => s.Questions)
                    .ThenInclude(q => q.Options)
                    .ToList();
            }
            else
            {

                survey = _context.Surveys
                    .Where(s => s.IsVisible == null && s.UserType == user!.Role || s.UserType == "All" && s.CreatedAt <= currentDate && (s.EndAt == null || s.EndAt >= currentDate))
                    .OrderByDescending(s => s.Id)
                    .Include(s => s.Questions)
                        .ThenInclude(q => q.Options)
                    .ToList();

            }
            List<int> studentCounts = new List<int>();
            List<int> teacherCounts = new List<int>();

            foreach (var s in survey)
            {
                var studentCount = _context.Users
                    .Join(_context.FilledSurveys,
                        user => user.Id,
                        filledSurvey => filledSurvey.UserId,
                        (user, filledSurvey) => new { User = user, FilledSurvey = filledSurvey })
                    .Where(joinResult => joinResult.User.Role == "Student" && joinResult.FilledSurvey.SurveyId == s.Id)
                    .GroupBy(joinResult => joinResult.User.Id)
                    .Count();

                var teacherCount = _context.Users
                    .Join(_context.FilledSurveys,
                        user => user.Id,
                        filledSurvey => filledSurvey.UserId,
                        (user, filledSurvey) => new { User = user, FilledSurvey = filledSurvey })
                    .Where(joinResult => joinResult.User.Role == "Teacher" && joinResult.FilledSurvey.SurveyId == s.Id)
                    .GroupBy(joinResult => joinResult.User.Id)
                    .Count();

                // Add the counts to the lists
                studentCounts.Add(studentCount);
                teacherCounts.Add(teacherCount);
            }

            // Pass the lists to the view using ViewBag
            ViewBag.StudentParticipantCounts = studentCounts;
            ViewBag.TeacherParticipantCounts = teacherCounts;

            // Create the view model
            var viewModel = new NewsSurveyViewModel
            {
                NewsList = news,
                SurveyList = survey,
                ContestList = contests
            };
            return View(viewModel);
        }

        public IActionResult Supports()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Supports([Bind("Id,Name,Email,TextMessage")] Support support)
        {
            if (ModelState.IsValid)
            {
                _context.Add(support);
                await _context.SaveChangesAsync();
                // Display success alert using SweetAlert2
                return Json(new { success = true });
            }
            return View(support);
        }

        public IActionResult TakeSurvey(int id)
        {
            var username = HttpContext.Session.GetString("username");
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);

            var FilledSurvey = _context.FilledSurveys.FirstOrDefault(f => f.SurveyId == id && f.UserId == user.Id);
            if (FilledSurvey == null)
            {
                var survey = _context.Surveys
                            .Include(s => s.Questions)
                            .ThenInclude(q => q.Options)
                            .SingleOrDefault(s => s.Id == id);
                if (survey == null)
                {
                    // Handle the case where the survey is not found
                    return NotFound();
                }
                return View(survey);
            }
            else
            {
                return RedirectToAction("Participated");
            }

        }

        public IActionResult Participated()
        {
            ViewBag.participated = "You have participated in this survey";
            return View();
        }



        [HttpPost]
        public IActionResult TakeSurvey(int id, List<int> selectedOptionIds)
        {
            var username = HttpContext.Session.GetString("username");
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);


            var survey = _context.Surveys
            .Include(s => s.Questions)
            .ThenInclude(q => q.Options)
            .SingleOrDefault(s => s.Id == id && s.IsVisible == null);
            if (survey == null)
            {
                // Handle the case where the survey is not found
                return NotFound();
            }

            foreach (var item in selectedOptionIds)
            {
                var FilledSurvey = new FilledSurvey
                {
                    CreatedAt = DateTime.Now,
                    UserId = user!.Id,
                    SurveyId = survey.Id,
                    OptionId = item
                };
                _context.FilledSurveys.Add(FilledSurvey);
                _context.SaveChanges();
            }
            return RedirectToAction("Thanks");
        }

        public IActionResult Thanks()
        {
            return View();
        }

        // GET: Faqs
        public async Task<IActionResult> Faqs()
        {
            return _context.Faqs != null ?
                        View(await _context.Faqs.ToListAsync()) :
                        Problem("Entity set 'Project2Context.Faqs'  is null.");
        }
    }
}