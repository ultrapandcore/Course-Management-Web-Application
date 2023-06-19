using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using University.Data.EF;
using University.Web.Models;

namespace University.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly UniversityContext _context;
        private readonly IMapper _mapper;

        public CoursesController(UniversityContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Courses
        public async Task<IActionResult> Index(int? id, int? groupId)
        {
            var courses = await _context.Courses
                .Include(c => c.Groups)
                .ThenInclude(g => g.Students)
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();

            var viewModel = new CourseIndexData()
            {
                SelecterCourseId = id,
                SelectedGroupId = groupId,
                Courses = courses
            };

            if (id != null)
            {
                var course = viewModel.Courses.Where(
                    c => c.Id == id.Value).Single();
                viewModel.Groups = course.Groups;
            }

            if (groupId != null)
            {
                viewModel.Students = viewModel.Groups.Where(
                    g => g.Id == groupId).Single().Students;
            }

            return View(viewModel);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Groups)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<CourseModel>(course);
            return View(model);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseModel courseModel)
        {
            if (ModelState.IsValid)
            {
                var course = _mapper.Map<Course>(courseModel);

                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(courseModel);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<CourseModel>(course);
            return View(model);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseModel courseModel)
        {
            if (id != courseModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var course = _mapper.Map<Course>(courseModel);

                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(courseModel);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<CourseModel>(course);
            return View(model);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Courses == null)
            {
                return Problem("Entity set 'UniversityContext.Courses'  is null.");
            }

            var course = await _context.Courses.Include(c => c.Groups).FirstOrDefaultAsync(c => c.Id == id);

            if (GroupsExist(course))
            {
                try
                {
                    throw new InvalidOperationException();
                }
                catch (InvalidOperationException)
                {
                    ViewBag.ErrorMessage = "Course can not be deleted if there is at least one group assigned to this course.";
                    ViewBag.ErrorTitle = "Invalid operation";
                    return View("Error");
                }
            }

            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsCourseNameAvailable(string name, int id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(m => m.Name == name);

            if (course == null || course.Id == id)
            {
                return Json(true);
            }
            else
            {
                return Json($"Course {name} is already in use.");
            }
        }

        private static bool GroupsExist(Course course)
        {
            return course != null && course.Groups.Any();
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
