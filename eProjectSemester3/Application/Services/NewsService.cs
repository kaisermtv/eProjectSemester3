using eProjectSemester3.Application.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eProjectSemester3.Application.Services
{
    public class NewsService
    {
        public readonly CacheService _cacheService;
        public readonly AppDbContext _context;

        public NewsService(AppDbContext context,CacheService cacheService)
        {
            _cacheService = cacheService;
            _context = context;
        }



    }
}