using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace HairSalon.Models
{
    public class Specialty
    {
        private int _id;
        private string _description;
        
        public Specialty(int id, string description)
        {
            _id = id;
            _description = description;

        }

        public string GetDescription()
        {
            return _description;
        }

        public int GetId()
        {
            return _id;
        }

        public void setDescription(string newDescription)
        {
            _description = newDescription;
        }

        public static List<Specialty> GetAll()
        {
            List<Specialty> allSpecialties = new List<Specialty>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText=@"SELECT * FROM specialties;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int specialty_id = rdr.GetInt32(0);
                string specialtyDescription = rdr.GetString(1);
                Specialty newSpecialty = new Specialty(specialty_id, specialtyDescription);
                allSpecialties.Add(newSpecialty);
            }
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
            return allSpecialties;
        }

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM specialties;";
        }
    }
}