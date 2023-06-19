using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using University.Data.EF;
using University.Web.Models;

namespace University.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly UniversityContext _context;
        private readonly IMapper _mapper;

        public StudentsController(UniversityContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .Include(s => s.Group)
                .AsNoTracking()
                .OrderBy(s => s.FirstName)
                .ToListAsync();

            List<StudentModel> studentsModel = new List<StudentModel>();

            foreach (var student in students)
            {
                studentsModel.Add(_mapper.Map<StudentModel>(student));
            }

            return View(studentsModel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Group)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<StudentModel>(student);
            return View(model);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            PopulateGroupsDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentModel studentModel)
        {
            if (ModelState.IsValid)
            {
                var student = _mapper.Map<Student>(studentModel);

                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateGroupsDropDownList(studentModel.GroupId);
            return View(studentModel);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<StudentModel>(student);
            PopulateGroupsDropDownList(model.GroupId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentModel studentModel)
        {
            if (id != studentModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var student = _mapper.Map<Student>(studentModel);

                try
                {

                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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

            PopulateGroupsDropDownList(studentModel.GroupId);
            return View(studentModel);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<StudentModel>(student);
            return View(model);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'UniversityContext.Students'  is null.");
            }

            var student = await _context.Students.FindAsync(id);

            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsEmailAvailable(string email, int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == email);

            if (student == null || student.Id == id)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use.");
            }
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsPhoneAvailable(string phoneNumber, int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.PhoneNumber == phoneNumber);

            if (student == null || student.Id == id)
            {
                return Json(true);
            }
            else
            {
                return Json($"Phone {phoneNumber} is already in use.");
            }
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(s => s.Id == id);
        }

        private void PopulateGroupsDropDownList(object selectedGroup = null)
        {
            var groupsQuery = from g in _context.Groups
                              orderby g.Name
                              select g;
            ViewBag.GroupId = new SelectList(groupsQuery.AsNoTracking(), "Id", "Name", selectedGroup);
        }
    }
}
