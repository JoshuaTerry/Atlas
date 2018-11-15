using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DriveCentric.BaseService
{
    public class LinqQuery<T>
    { 
        private bool hasPredicate = false;
        private ExpressionStarter<T> predicate = PredicateBuilder.New<T>(); 
        private IQueryable<T> query;

        public List<string> OrderBy { get; set; } = new List<string>();
        public int Offset { get; set; }
        public int Limit { get; set; }

        public ExpressionStarter<T> Predicate
        {
            get { return predicate; }
        }
         
        public LinqQuery(IQueryable<T> query)
        {
            this.query = query;
        }
         
        public IQueryable<T> GetQueryable()
        {
            var query = this.query?.AsExpandable();
            if (hasPredicate)
            {
                query = query.Where(predicate);
            }
            query = AddSorting(query);
            query = AddPaging(query);

            return query;
        }

        public LinqQuery<T> Or(Expression<Func<T, bool>> expression)
        {
            if (expression != null)
            {
                hasPredicate = true;
                predicate = predicate.Or(expression);
            }

            return this;
        }
         
        protected void PredicateAnd(Expression<Func<T, bool>> expression)
        {
            if (expression != null)
            {
                hasPredicate = true;
                predicate = predicate.And(expression);
            }
        }
         
        private IQueryable<T> AddPaging(IQueryable<T> query)
        {
            if (Offset > 0)
            {
                query = query.Skip(Offset * Limit);
            }
            if (Limit > 0)
            {
                query = query.Take(Limit);
            }
            return query;
        }

        private IQueryable<T> AddSorting(IQueryable<T> query)
        {
            if (OrderBy?.Count > 0)
            {
                int orderNumber = 0;
                foreach (string orderByColumn in OrderBy)
                {
                    orderNumber++;
                    string propertyName = orderByColumn;
                    bool descending = false;
                    if (propertyName.StartsWith("-"))
                    {
                        descending = true;
                        propertyName = propertyName.TrimStart('-');
                    }
                    query = query.DynamicOrderBy(propertyName, descending, orderNumber == 1);
                }
            }
            else if (Offset > 0 || Limit > 0)
            {
                query = query.DynamicOrderBy("Id", true, true);
            }
            return query;
        }
    }
}
