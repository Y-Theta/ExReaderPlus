using System;
using System.Collections.Generic;
using System.Text;

namespace TestDB
{
    class StudentMission
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        /// <summary>
        /// 外键
        /// </summary>
        public Student Student { get; set; }

        public int MissionId { get; set; }

        public Mission Mission { get; set; }
    }
}
