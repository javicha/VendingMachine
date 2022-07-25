using Vending.Domain.Common;

namespace Vending.Domain.ExtensionMethods
{
    public static class EntityExtensions
    {
        public static T SetUpdateAuditParams<T>(this T entity, string userName) where T : EntityBase
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new Exception("Invalid operation");
            }

            dynamic dynEnt = (dynamic)entity;

            dynEnt.DateModified = DateTime.Now;
            dynEnt.UserModified = userName;

            return entity;
        }


        public static T SetInsertAuditParams<T>(this T entity, string userName) where T : EntityBase
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new Exception("Invalid operation");
            }

            dynamic dynEnt = (dynamic)entity;
            DateTime date = DateTime.Now;

            dynEnt.DateCreated = date;
            dynEnt.UserCreated = userName;
            dynEnt.DateModified = date;
            dynEnt.UserModified = userName;

            return entity;
        }

        public static T SetDeleteAuditParams<T>(this T entity, string userName) where T : EntityBase
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new Exception("Invalid operation");
            }

            dynamic dynEnt = (dynamic)entity;
            DateTime date = DateTime.Now;

            dynEnt.DateDeleted = date;
            dynEnt.UserDeleted = userName;
            dynEnt.DateModified = date;
            dynEnt.UserModified = userName;

            return entity;
        }

    }
}
