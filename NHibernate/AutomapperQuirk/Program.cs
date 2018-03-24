using System;
using System.Collections.Generic;
using AutoMapper;
using EntitiesAndMaps.Books;
using NHibernate;

namespace AutomapperQuirk
{
    
    public class BookDto
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }
    } 
    
    internal class Program
    {
        public static Book GetBook(BookDto bookDto, ISession session)
        {
            Book book;
            using (var tx = session.BeginTransaction())
            {
                book = session.Get<Book>(bookDto.Id);
                tx.Commit();
            }
            
            session.Evict(book);
            
            return book;
        }
        
        public static void Main(string[] args)
        {
            var session = SessionFactoryBuilder.SessionFactoryCreator.GetOrCreateSessionFactory().OpenSession();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<BookDto, Book>()
                    .ConstructUsing(dto => GetBook(dto,session));
            });
            
            var bookDto = new BookDto()
            {
                Id = 1,
                Title = Guid.NewGuid().ToString(),
                Year = 999
            };
            var bookDto2 = new BookDto()
            {
                Id = 2,
                Title = Guid.NewGuid().ToString(),
                Year = 999
            };

            var books = Mapper.Map<IReadOnlyList<Book>>(new[]{bookDto, bookDto2});
            
            session.Update(books[0]);
            session.Save(books[1]);
            session.Flush();
            session.Close();
        }
    }
}