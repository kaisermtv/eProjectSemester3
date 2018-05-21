using eProjectSemester3.Application.Context;
using eProjectSemester3.Application.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace eProjectSemester3.Application.UnitOfWork
{
    public class UnitOfWorkManager 
    {
        private bool _isDisposed;
        private readonly AppDbContext _context;

        public UnitOfWorkManager(AppDbContext context)
        {
            _context = context;
            //http://www.entityframeworktutorial.net/code-first/automated-migration-in-code-first.aspx
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDbContext, Configuration>("AppDbContext"));
        }

        /// <summary>
        /// Provides an instance of a unit of work. This wrapping in the manager
        /// class helps keep concerns separated
        /// </summary>
        /// <returns></returns>
        public UnitOfWork NewUnitOfWork()
        {
            return new UnitOfWork(_context);
        }

        /// <summary>
        /// Make sure there are no open sessions.
        /// In the web app this will be called when the injected UnitOfWork manager
        /// is disposed at the end of a request.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _context.Dispose();
                _isDisposed = true;
            }
        }
    }
}