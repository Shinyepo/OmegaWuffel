    using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OWuffel.Extensions.API
{
    public class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ProcessID { get; set; }
        public bool isResponding { get; set; }
        public long VirtualMemory64 { get; set; }
        public long WorkingSet64 { get; set; }
        public TimeSpan TotalProcessorTime { get; set; }
        public DateTime StartTime { get; set; }

        public Student(int id, string name, int processid, bool isresponding, long virtualmemory64, long workingset64, TimeSpan totalprocessortime, DateTime starttime)
        {
            ID = id;
            Name = name;
            ProcessID = processid;
            isResponding = isresponding;
            VirtualMemory64 = virtualmemory64;
            WorkingSet64 = workingset64;
            TotalProcessorTime = totalprocessortime;
            StartTime = starttime;
        }
    }

    
    [Route("api/values")]
    [ApiController]
    public class Values : Controller
    {

        public Values()
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var main = Process.GetCurrentProcess();
            Student student = new Student(1, "Arek", main.Id, main.Responding, main.VirtualMemorySize64, main.WorkingSet64, main.TotalProcessorTime, main.StartTime);
            return Json(new { student });
        }
    }
}
