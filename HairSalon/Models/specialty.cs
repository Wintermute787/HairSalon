using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace HairSalon.Models
{
    public class specialty
    {
        private int _id;
        private string _description;
        
        public specialty(int id, string description)
        {
            _id = id;
            _description = description;

        }
    }
}