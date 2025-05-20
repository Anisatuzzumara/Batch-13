// See https://aka.ms/new-console-template for more information


using System;
using System.Collections.Generic;

namespace ListOfBook
{
    class GFG
    {

        public static IEnumerable<string> GetMyList()
        {
            // Creating and adding elements in list
            List<string> my_list = new List<string>() {
                     "Harry Potter", "The Hobbit", "Game of Thrones", "Lord Of The Ring" };

       
            // Iterating the elements of my_list
            foreach(var items in my_list)
            {
            // Returning the element after every iteration
            yield return items;
            }
        }

        public static void Main()
        {

            IEnumerable<string> my_slist = GetMyList();

            foreach(var i in my_slist)
            {
                Console.WriteLine(i);
            }
        }
    }

}
 