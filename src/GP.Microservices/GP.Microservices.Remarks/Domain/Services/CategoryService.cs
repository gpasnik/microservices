using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GP.Microservices.Remarks.Data;
using GP.Microservices.Remarks.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GP.Microservices.Remarks.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly RemarksContext _context;

        public CategoryService(RemarksContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetAsync(Guid id)
        {
            return await _context.Categories.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            var entry = await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }
    }
}