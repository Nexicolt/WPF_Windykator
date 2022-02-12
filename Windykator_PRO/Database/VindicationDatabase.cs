using System;
using System.Data.Entity;
using System.Linq;
using Windykator_PRO.Database.Models;
using Windykator_PRO.Database.Models.Base;
using Windykator_PRO.Database.Views;

namespace Windykator_PRO.Database
{
    public class VindicationDatabase : DbContext
    {
        public VindicationDatabase() : base("name=Vindication")
        {
        }

        #region TABLES

        //U¿ytkownicy
        public DbSet<User> User { get; set; }

        //Klienic
        public DbSet<Customer> Customer { get; set; }

        public DbSet<Debtor> Debtor { get; set; }

        public DbSet<BankAccount> BankAccount { get; set; }

        public DbSet<Adress> Adress { get; set; }

        public DbSet<Currency> Currency { get; set; }

        public DbSet<PaymentMethod> PaymentMethod { get; set; }

        public DbSet<DocumentHeader> DocumentHeader { get; set; }
        public DbSet<DocumentItem> DocumentItem { get; set; }
        public DbSet<DocumentToIssue> DocumentToIssue { get; set; }
        public DbSet<Service> Service { get; set; }

        public DbSet<Assortment> Assortment { get; set; }

        public DbSet<DocumentType> DocumentType { get; set; }

        public DbSet<Issue> Issue { get; set; }
        public DbSet<IssueAddnotation> Addnotation { get; set; }

        #endregion TABLES

        #region VIEWS


        public IQueryable<Customer_GridView> Customer_GridView
        {
            get
            {
                return this.Customer.Where(row => row.IsEnable).Select(row => new Customer_GridView
                {
                    Id = row.Id,
                    Name = row.Name,
                    City = row.Adress.City,
                    Street = row.Adress.Street,
                    IssuesCount = this.Issue.Where(x => x.Customer.Id == row.Id).Count(),
                    Due = (this.Issue.Where(x => x.Customer.Id == row.Id).Count() > 0) ? this.Issue.Where(x => x.Customer.Id == row.Id).Sum(p=>p.Cost) : 0,
                    Currency = (row.IsForeignCustomer) ? "EUR" : "PLN",
                    IsIndyvidual = row.IsIndyvidual,
                    DuringDuringCourtProcess = false
                });
            }
        }

        public IQueryable<Debtor_GridView> Debtor_GridView
        {
            get
            {
                return this.Debtor.Where(row => row.IsEnable).Select(row => new Debtor_GridView
                {
                    Id = row.Id,
                    Name = row.Name,
                    City = row.Adress.City,
                    Street = row.Adress.Street,
                    IssuesCount = this.Issue.Where(x => x.Debtor.Id == row.Id).Count(),
                    Debt = (this.Issue.Where(x => x.Debtor.Id == row.Id).Count() > 0) ? this.Issue.Where(x => x.Debtor.Id == row.Id).Sum(p => p.Cost) : 0,
                    Currency = (row.IsForeignDebtor) ? "EUR" : "PLN",
                    IsIndyvidual = row.IsIndyvidual,
                    DuringDuringCourtProcess = false
                });
            }
        }

        #endregion VIEWS

        /// <summary>
        /// Nadpisana metoda, zapisuje z automatu aktualn¹ datê do kolumny "EditDate" lub "CreateDate"
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            this.ChangeTracker.DetectChanges();

            var addedRows = this.ChangeTracker.Entries()
                        .Where(t => t.State == EntityState.Added)
                        .Select(t => t.Entity)
                        .ToArray();

            foreach (var entity in addedRows)
            {
                if (entity != null && entity is TimeStampBaseModel)
                {
                    var track = entity as TimeStampBaseModel;
                    track.CreateDate = DateTime.Now;
                }
            }

            var editedRows = this.ChangeTracker.Entries()
                       .Where(t => t.State == EntityState.Modified)
                       .Select(t => t.Entity)
                       .ToArray();

            foreach (var entity in editedRows)
            {
                if (entity != null && entity is TimeStampBaseModel)
                {
                    var track = entity as TimeStampBaseModel;
                    track.EditDate = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
    }
}