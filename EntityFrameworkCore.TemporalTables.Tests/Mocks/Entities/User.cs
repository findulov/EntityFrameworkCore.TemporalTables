using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.TemporalTables.Tests.Mocks.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
