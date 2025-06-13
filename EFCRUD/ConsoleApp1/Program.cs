using System;
using System.Dynamic;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        //Modify this however you'd and test out the DatabaseService methods 
        static void Main(string[] args)
        {
            //Creates some sample students so we can test out the database

            DatabaseService.EditStudent(new Student { firstName = "Lukas", lastName = "Jackson", grade = 97, absences = 2});
            DatabaseService.AddStudent(new Student { firstName = "Corey", lastName = "Hart", grade = 89, absences = 3 });
            DatabaseService.AddStudent(new Student { firstName = "Bon", lastName = "Jovi", grade = 76, absences = 6 });
            DatabaseService.AddStudent(new Student { firstName = "Bryan", lastName = "Adams", grade = 88, absences = 1 });
            DatabaseService.AddStudent(new Student { firstName = "Billy", lastName = "Idol", grade = 97, absences = 2 });
            DatabaseService.AddStudent(new Student { firstName = "Phil", lastName = "Collis", grade = 91, absences = 0 });
            DatabaseService.AddStudent(new Student { firstName = "Billy", lastName = "Joel", grade = 63, absences = 2 });

            //Gets the student with id 6 and assigns it to "tempStudent", or Phil Collins
            var tempStudent = DatabaseService.GetStudent(6);
            //Changes the value of the grade of the tempStudent
            tempStudent.grade = 95;
            //Updates the information of the tempStudent to the database
            DatabaseService.EditStudent(tempStudent);

            //Removes the student with the id 2
            //In this case it would remove the student "Corey Hart" from the database
            DatabaseService.RemoveStudent(2);

            //Prints something so that we know the code actually executed
            Console.WriteLine("Finished Running");
            //Prints the path so we know where the database file is generated 
            Console.WriteLine("Path: " + Directory.GetCurrentDirectory());
        }
    }
}