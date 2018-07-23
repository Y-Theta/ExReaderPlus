using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.DB
{
    public class Mission
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public List<StudentMission> StudentMissions { get; set; }
    }
}
