using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1
{
    class DatabaseService
    {
        public static Student AddStudent(Student newStudent)
        {
            //Access the database and assign it to context
            using (var context = new DatabaseModel())
            {
                //Add the student to the database
                var entity = context.Students.Add(newStudent);
                entity.State = EntityState.Added;

                //Save the changes you made
                context.SaveChanges();

                return newStudent;
            }
        }

        public static Student RemoveStudent(int id)
        {
            //Access the database and assign it to context
            using (var context = new DatabaseModel())
            {
                //Finds the student by id and assigns it to result
                var result = context.Students.Find(id);

                //If there is no result stop executing this method
                if (result is null) return null;

                //Remove the student from the database
                var entity = context.Students.Remove(result);
                entity.State = EntityState.Deleted;

                //Save the changes you made
                context.SaveChanges();

                return result;
            }
        }

        public static Student GetStudent(int id)
        {
            //Access the database and assign it to context
            using (var context = new DatabaseModel())
            {
                //Get the student by id and return it
                var result = context.Students.Find(id);

                return result;
            }
        }

        public static Student EditStudent(Student student)
        {
            //Access the database and assign it to context
            using (var context = new DatabaseModel())
            {
                //Update the student information in the database from the Student passed in
                var entity = context.Students.Update(student);
                entity.State = EntityState.Modified;

                //Save the changes you made
                context.SaveChanges();

                return student;
            }
        }
    }
}

