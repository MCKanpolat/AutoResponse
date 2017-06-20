﻿namespace AutoResponse.Data.Exceptions
{
    public class EntityNotFoundException<TEntity> : EntityNotFoundException
    {
        public EntityNotFoundException(string entityId)
            : base(typeof(TEntity).Name, entityId)
        {
        }
    }
}