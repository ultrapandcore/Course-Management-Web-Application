using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University.Data.EF;
using University.Web.Models;

namespace University.Web.Controllers
{
    public class GroupsController : Controller
    {
        private readonly UniversityContext _context;
        private readonly IMapper _mapper;

        public GroupsController(UniversityContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
            var groups = await _context.Groups
                .Include(g => g.Students)
                .Include(g => g.Courses)
                .AsNoTracking()
                .OrderBy(g => g.Name)
                .ToListAsync();

            var groupsModel = new List<GroupModel>();

            foreach (var group in groups)
            {
                var groupModel = _mapper.Map<GroupModel>(group);
                groupsModel.Add(groupModel);
            }

            return View(groupsModel);
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var group = await _context.Groups
                .Include(g => g.Students)
                .Include(g => g.Courses)
                .Where(g => g.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<GroupModel>(group);
            return View(model);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            var group = new Group();
            PopulateAssignedCourseData(group);
            return View();
        }

        // POST: Groups/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GroupModel groupModel, string[] selectedCourses)
        {
            if (ModelState.IsValid)
            {
                var group = _mapper.Map<Group>(groupModel);
                PopulateAssignedCourseData(group);

                _context.Add(group);
                await _context.SaveChangesAsync();

                if (selectedCourses != null)
                {
                    foreach (var course in selectedCourses)
                    {
                        var courseToAdd = new GroupCourse { GroupId = group.Id, CourseId = int.Parse(course) };
                        _context.GroupCourses.Add(courseToAdd);
                    }
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }

            return View(groupModel);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var group = await _context.Groups
                .Include(g => g.Courses)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            PopulateAssignedCourseData(group);
            var model = _mapper.Map<GroupModel>(group);
            return View(model);
        }

        // POST: Groups/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedCourses)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupToUpdate = await _context.Groups
                .Include(g => g.Courses)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (await TryUpdateModelAsync<Group>(
                groupToUpdate,
                "",
                g => g.Name))
            {
                UpdateGroupCourses(selectedCourses, groupToUpdate);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            UpdateGroupCourses(selectedCourses, groupToUpdate);
            PopulateAssignedCourseData(groupToUpdate);

            var model = _mapper.Map<GroupModel>(groupToUpdate);
            return View(model);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var group = await _context.Groups
                .Include(g => g.Students)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<GroupModel>(group);
            return View(model);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Groups == null)
            {
                return Problem("Entity set 'UniversityContext.Groups'  is null.");
            }

            var group = await _context.Groups.Include(g => g.Students).FirstOrDefaultAsync(g => g.Id == id);

            if (StudentsExists(group))
            {
                try
                {
                    throw new InvalidOperationException();
                }
                catch (InvalidOperationException)
                {
                    ViewBag.ErrorMessage = "Group can not be deleted if there is at least one student assigned to this group.";
                    ViewBag.ErrorTitle = "Invalid operation";
                    return View("Error");
                }
            }

            if (group != null)
            {
                _context.Groups.Remove(group);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsGroupNameAvailable(string name, int id)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Name == name);

            if (group == null || group.Id == id)
            {
                return Json(true);
            }
            else
            {
                return Json($"Group {name} is already in use.");
            }
        }

        private static bool StudentsExists(Group group) => (group.Students.Count > 0);

        private void PopulateAssignedCourseData(Group group)
        {
            var allCourses = _context.Courses;
            var groupCourses = new HashSet<int>(group.Courses.Select(c => c.Id));
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseId = course.Id,
                    Title = course.Name,
                    Assigned = groupCourses.Contains(course.Id)
                });
            }
            ViewData["Courses"] = viewModel;
        }

        private void UpdateGroupCourses(string[] selectedCourses, Group groupToUpdate)
        {
            if (selectedCourses == null)
            {
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var groupCourses = new HashSet<int>
                (groupToUpdate.Courses.Select(c => c.Id));

            foreach (var course in _context.Courses)
            {
                if (selectedCoursesHS.Contains(course.Id.ToString()))
                {
                    if (!groupCourses.Contains(course.Id))
                    {
                        groupToUpdate.Courses.Add(course);
                        _context.GroupCourses.Add(new GroupCourse { GroupId = groupToUpdate.Id, CourseId = course.Id });
                    }
                }
                else
                {
                    if (groupCourses.Contains(course.Id))
                    {
                        groupToUpdate.Courses.Remove(course);
                        GroupCourse groupCourseToRemove = _context.GroupCourses.FirstOrDefault(gc => gc.CourseId == course.Id);
                        _context.Remove(groupCourseToRemove);
                    }
                }
            }
        }
    }
}
