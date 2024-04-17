using EvidencePractise_final1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EvidencePractise_final1.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDbCore1Context _db;
        private readonly IWebHostEnvironment _he;
        public EmployeeController(EmployeeDbCore1Context db, IWebHostEnvironment he)
        {
            _db = db;
            _he = he;
        }
        public async  Task<IActionResult> Index(string userText, string sortOrder, int page)
        {
            ViewBag.sWord = userText;
            ViewBag.sName = string.IsNullOrEmpty(sortOrder) ? "desc_Name" : "";

            IQueryable<Employee> employees = _db.Employees.Include(x => x.EmployeeSkills).ThenInclude(y => y.Skill);

            if (!string.IsNullOrEmpty(userText))
            {
                userText = userText.ToLower();
                employees = employees.Where(e => e.EmploeeName.ToLower().Contains(userText));
            }

            switch (sortOrder)
            {
                case "desc_Name":
                    employees = employees.OrderByDescending(e => e.EmploeeName);
                    break;
                default:
                    employees = employees.OrderBy(e => e.EmploeeName);
                    break;
            }
            ViewBag.count = employees.Count();
            if (page <= 0) page = 1;
            int pageSize = 2;
            ViewBag.pSize = pageSize;
            return View(await PaginatedList<Employee>.CreateAsync(employees,page,pageSize));
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult AddNewSkill(int id)
        {
            ViewBag.skills = new SelectList(_db.Skills, "SkillId", "SkillName", id.ToString() ?? "");
            return PartialView("_addSkill");
        }
        [HttpPost]
        public IActionResult Create(EmployeeVM employeeVM, int[] skillId)
        {
            if (ModelState.IsValid)
            {
                Employee employee = new Employee()
                {
                    EmploeeName = employeeVM.EmploeeName,
                    Joindate = employeeVM.Joindate,
                    ActiveStatus = employeeVM.ActiveStatus,
                };

                var file = employeeVM.ImageFile;
                string webroot = _he.WebRootPath;
                string folder = "Images";
                string imgFile = Path.GetFileName(employeeVM.ImageFile.FileName);
                string filetoSave = Path.Combine(webroot, folder, imgFile);
                if(file != null)
                {
                    using(var stream = new FileStream(filetoSave, FileMode.Create))
                    {
                        employeeVM.ImageFile.CopyTo(stream);
                        employee.Image = "/" + folder + "/" + imgFile;
                    }
                }
                foreach (var item in skillId)
                {
                    EmployeeSkill employeeSkill = new EmployeeSkill()
                    {
                        Employee = employee,
                        EmployeeId = employee.EmployeeId,
                        SkillId = item
                    };
                    _db.EmployeeSkills.Add(employeeSkill);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            var employee = _db.Employees.FirstOrDefault(x => x.EmployeeId == id);
            EmployeeVM employeeVM = new EmployeeVM()
            {
                EmployeeId = employee.EmployeeId,
                EmploeeName = employee.EmploeeName,
                Joindate = employee.Joindate,
                ActiveStatus = employee.ActiveStatus,
                Image = employee.Image,
            };
            var existSkill = _db.EmployeeSkills.Where(x => x.EmployeeId == id).ToList();
            foreach (var item in existSkill)
            {
                employeeVM.SkillList.Add((int)item.SkillId);
            }
            return View(employeeVM);
        }
        [HttpPost]
        public IActionResult Edit(EmployeeVM employeeVM, int[] skillId)
        {
            if (ModelState.IsValid)
            {
                Employee employee = new Employee()
                {
                    EmployeeId= employeeVM.EmployeeId,
                    EmploeeName = employeeVM.EmploeeName,
                    Joindate = employeeVM.Joindate,
                    ActiveStatus = employeeVM.ActiveStatus,
                };

                var file = employeeVM.ImageFile;
               
                if (file != null)
                {
                    string webroot = _he.WebRootPath;
                    string folder = "Images";
                    string imgFile = Path.GetFileName(employeeVM.ImageFile.FileName);
                    string filetoSave = Path.Combine(webroot, folder, imgFile);
                    using (var stream = new FileStream(filetoSave, FileMode.Create))
                    {
                        employeeVM.ImageFile.CopyTo(stream);
                        employee.Image = "/" + folder + "/" + imgFile;
                    }
                }
                else
                {
                    employee.Image = employeeVM.Image;
                }
                var existSkill = _db.EmployeeSkills.Where(x => x.EmployeeId == employee.EmployeeId).ToList();
                foreach (var item in existSkill)
                {
                    _db.EmployeeSkills.RemoveRange(@item);
                }
                foreach (var item in skillId)
                {
                    EmployeeSkill employeeSkill = new EmployeeSkill()
                    {
                        EmployeeId = employee.EmployeeId,
                        SkillId = item
                    };
                    _db.EmployeeSkills.Add(employeeSkill);
                }
                _db.Update(employee);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            var employee = _db.Employees.Find(id);
            if(employee != null)
            {
                var delete = _db.EmployeeSkills.Where(x=>x.EmployeeId == employee.EmployeeId).ToList();
                _db.EmployeeSkills.RemoveRange(delete);
                _db.Employees.Remove(employee);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
