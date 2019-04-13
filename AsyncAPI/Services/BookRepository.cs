using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncAPI.Contexts;
using AsyncAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsyncAPI.Services
{
    public class BookRepository : IBookRepository, IDisposable
    {
        private BooksContext _context;

        public BookRepository(BooksContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Book> GetBooks()
        {
            _context.Database.ExecuteSqlCommandAsync("SELECT SLEEP(2);");
            return _context.Books.Include(b => b.Author)
                .ToList();
        }

        public Book GetBook(int id)
        {
            return _context.Books
                .Include(b => b.Author)
                .FirstOrDefault(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            await _context.Database.ExecuteSqlCommandAsync("SELECT SLEEP(2);");
            return await _context.Books
                .Include(b => b.Author).ToListAsync();
        }

        public async Task<Book> GetBookAsync(int id)
        {
            return await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
    }
}