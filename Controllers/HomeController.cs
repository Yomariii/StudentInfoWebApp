using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace StudentInfoWebApp.Controllers
{
    public class HomeController : Controller
    {
        private static List<Student> students = new List<Student>();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitData(Student student)
        {
            students.Add(student);
            TempData["Message"] = "Data submitted successfully.";
            return RedirectToAction("Index");
        }

        public IActionResult BindDataToPDF()
        {
            if (students.Count == 0)
            {
                TempData["Message"] = "No data to bind. Please submit data first.";
                return RedirectToAction("Index");
            }

            string pdfPath = "wwwroot/reports/StudentReport.pdf";

            try
            {
                using (var writer = new PdfWriter(pdfPath))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);

                        foreach (var student in students)
                        {
                            document.Add(new Paragraph($"Name: {student.Name}"));
                            document.Add(new Paragraph($"Roll No: {student.RollNo}"));
                            document.Add(new Paragraph($"Address: {student.Address}"));
                            document.Add(new Paragraph($"Age: {student.Age}"));
                            document.Add(new AreaBreak());
                        }
                    }
                }

                TempData["Message"] = $"PDF report generated at {pdfPath}";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error generating PDF: {ex.Message}";
            }

            return RedirectToAction("Index");
        }



        public class Student
        {
            private const int MaxNameLength = 150;

            private string _name;

            public string Name
            {
                get => _name;
                set => _name = (value?.Length > MaxNameLength) ? value?.Substring(0, MaxNameLength) : value;
            }

            public int RollNo { get; set; }
            public string Address { get; set; }
            public int Age { get; set; }
        }

    }
