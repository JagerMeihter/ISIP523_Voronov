using System;
using System.Collections.Generic;
using System.Linq;
using static Book;
class Book
{
    public int BookID { get; set; }
    public string BookName { get; set; }
    public string BookAuthor { get; set; }
    public Jenre BookJenre { get; set; }
    public int BookYear { get; set; }
    public decimal Price { get; set; }
    
    public Book(int bookID , string name , string author , Jenre jenre , int year , decimal price)
    {
        BookID = bookID;
        BookName = name;
        BookAuthor = author;    
        BookJenre = jenre;
        BookYear = year;
        Price = price;
    }
    public enum Jenre
    {
        Horror,
        Detective,
        Roman,
        Fantastik,
        Fantasy
        
    }
}