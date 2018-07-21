using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.DB
{
    public class StudentMission
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public Student Student { get; set; }

        public int MissionId { get; set; }

        public Mission Mission { get; set; }
    }
}
