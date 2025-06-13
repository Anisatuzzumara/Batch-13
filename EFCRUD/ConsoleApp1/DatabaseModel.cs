using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace ConsoleApp1
{
    //This is function that models how the Database should be designed
    class DatabaseModel : DbContext //Inherits from Microsofts EntityframeworkCore.DbContext to implement Database "things"
{
    //Constructor that makes sure that the database file is created
    //and if it isn't will create the database
    public DatabaseModel()
    {
        this.Database.EnsureCreated();
    }

    public DbSet<Student> Students { get; set; } //Same as a table or list of students

    protected override void OnConfiguring(DbContextOptionsBuilder options)//Overrides OnConfiguring method from DbContext and sets the Database file path
    {
        options.UseSqlite("Data Source=students.db"); ////determines the file path as the current directory with the file "students.db"
    }
}

public class Student
{
    public int Id { get; set; } //Primary key for the student
    public string firstName { get; set; } //Name of the student
    public string lastName { get; set; } //Last name of the student
    public int grade { get; set; } //Grade of the student
    public int absences { get; set; } //Number of absences for the student


    }
}


