﻿namespace AutoResponse.Data.Exceptions
{
    using System;

    public class EntityNotFoundException : Exception, IEntityNotFoundException
    {
        public EntityNotFoundException(string entityType, string entityId)
        {
            if (string.IsNullOrWhiteSpace(entityType))
            {
                throw new ArgumentNullException(nameof(entityType));
            }

            if (string.IsNullOrWhiteSpace(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            this.EntityType = entityType;
            this.EntityId = entityId;
        }

        public string EntityType { get; }

        public string EntityId { get; }
    }
}
